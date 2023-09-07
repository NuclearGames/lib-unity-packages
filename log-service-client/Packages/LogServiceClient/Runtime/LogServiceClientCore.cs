using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.Caches;
using LogServiceClient.Runtime.Caches.Interfaces;
using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Mappers;
using LogServiceClient.Runtime.Pools;
using LogServiceClient.Runtime.RequestMachine;
using LogServiceClient.Runtime.RequestMachine.Factories;
using LogServiceClient.Runtime.WebRequests;
using LogServiceClient.Runtime.WebRequests.Utils;
using System;
using System.Threading;

namespace LogServiceClient.Runtime {
    public sealed class LogServiceClientCore : IDisposable {
        private readonly LogServiceClientOptions _options;
        private readonly LogServiceRequester _requester;

        private readonly LogPool<ReceiveLogEntry> _receivePool;
        private readonly LogPool<SendLogEntry> _sendPool;
        private readonly LogPool<LogEventEntity> _logEventEntityPool;

        private readonly ReceiveLogBuffer _receiveBuffer;
        private readonly SendLogBuffer _sendBuffer;
        private readonly LogErrorCache _logErrorCache;

        private readonly ILogIdProvider _logIdProvider;

        private readonly ReceiveLogEntryToSendLogEntryMapper _receiveLogEntryToSendLogEntryMapper;
        private readonly SendLogEntryToLogEventEntityMapper _sendLogEntryToLogEventEntityMapper;
        private readonly LogServiceClientDeviceOptionsToLogDeviceInfoEntityMapper _logServiceClientDeviceOptionsToLogDeviceInfoEntityMapper;
        private readonly LogMessageReceiveHandler _logMessageReceiveHandler;
        private readonly LogRequestMachineContext _context;
        private readonly LogRequestMachine _requestMachine;

        private readonly CancellationTokenSource _lifetimeCts;

        public LogServiceClientCore(LogServiceClientOptions options) {
            _options = options;

            _receiveLogEntryToSendLogEntryMapper = new ReceiveLogEntryToSendLogEntryMapper();
            _sendLogEntryToLogEventEntityMapper = new SendLogEntryToLogEventEntityMapper();
            _logServiceClientDeviceOptionsToLogDeviceInfoEntityMapper = new LogServiceClientDeviceOptionsToLogDeviceInfoEntityMapper();

            _receivePool = new LogPool<ReceiveLogEntry>(_options.ReceiveBufferPoolCapacity, _options.ReceiveBufferPoolCapacity);
            _sendPool = new LogPool<SendLogEntry>(_options.SendBufferPoolStartCapacity, _options.SendBufferPoolMaxCapacity);
            _logEventEntityPool = new LogPool<LogEventEntity>(_options.EventEntityPoolCapacity, _options.EventEntityPoolCapacity);

            _requester = new LogServiceRequester(_options, _logServiceClientDeviceOptionsToLogDeviceInfoEntityMapper);

            _receiveBuffer = new ReceiveLogBuffer(_receivePool, _options.ReceiveBufferCapacity);
            _sendBuffer = new SendLogBuffer(_sendPool, _options.SendBufferCapacity);
            _logErrorCache = new LogErrorCache(_options.ErrorCacheCapacity);

            _logIdProvider = options.LogIdProvider;

            _logMessageReceiveHandler = new LogMessageReceiveHandler(
                _receiveBuffer, _sendBuffer,
                _receiveLogEntryToSendLogEntryMapper,
                _logErrorCache,
                _logIdProvider);

            _context = new LogRequestMachineContext(
                _requester,
                _sendBuffer,
                _sendLogEntryToLogEventEntityMapper,
                _logEventEntityPool);

            _requestMachine = new LogRequestMachine(_options, _context, new LogRequestStateFactory());

            _lifetimeCts = new CancellationTokenSource();

            _sendBuffer.onLogsAdded += OnSendBufferLogsAdded;
        }

        public void Dispose() {
            _lifetimeCts.Cancel();

            _sendBuffer.onLogsAdded -= OnSendBufferLogsAdded;

            _logMessageReceiveHandler.Dispose();
        }

        public void TryRun() {
            if(!_requestMachine.IsRunning && _sendBuffer.Count > 0) {
                // UnityWebRequest падает с ошибкой: Create can only be called from the main thread.
                // UniTask.RunOnThreadPool(_runMachineAction, cancellationToken: _lifetimeCts.Token);

                _requestMachine.Run(_lifetimeCts.Token).Forget();
            }
        }

        private void OnSendBufferLogsAdded() {
            TryRun();
        }
    }
}
