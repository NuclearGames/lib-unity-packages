using LogServiceClient.Runtime.Caches;
using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Mappers;
using LogServiceClient.Runtime.Pools;
using LogServiceClient.Runtime.RequestMachine;
using LogServiceClient.Runtime.RequestMachine.Factories;
using LogServiceClient.Runtime.WebRequests;
using LogServiceClient.Runtime.WebRequests.Utils;
using System;

namespace LogServiceClient.Runtime {
    public sealed class LogServiceClient : IDisposable {
        private readonly LogServiceClientOptions _options;
        private readonly LogServiceRequester _requester;
        private readonly LogPool<ReceiveLogEntry> _receivePool;
        private readonly LogPool<SendLogEntry> _sendPool;
        private readonly LogPool<LogEventEntity> _logEventEntityPool;
        private readonly ReceiveLogBuffer _receiveBuffer;
        private readonly SendLogBuffer _sendBuffer;
        private readonly ReceiveLogEntryToSendLogEntryMapper _receiveLogEntryToSendLogEntryMapper;
        private readonly SendLogEntryToLogEventEntityMapper _sendLogEntryToLogEventEntityMapper;
        private readonly LogMessageReceiveHandler _logMessageReceiveHandler;
        private readonly LogRequestMachineContext _context;
        private readonly LogRequestMachine _requestMachine;

        public LogServiceClient(LogServiceClientOptions options) {
            _options = options;

            _requester = new LogServiceRequester(options);

            _receivePool = new LogPool<ReceiveLogEntry>(options.ReceiveBufferPoolCapacity);
            _sendPool = new LogPool<SendLogEntry>(-1);
            _logEventEntityPool = new LogPool<LogEventEntity>(-1);

            _receiveBuffer = new ReceiveLogBuffer(_receivePool, options.ReceiveBufferCapacity);
            _sendBuffer = new SendLogBuffer(_sendPool);

            _receiveLogEntryToSendLogEntryMapper = new ReceiveLogEntryToSendLogEntryMapper();
            _sendLogEntryToLogEventEntityMapper = new SendLogEntryToLogEventEntityMapper();

            _logMessageReceiveHandler = new LogMessageReceiveHandler(_receiveBuffer, _sendBuffer,
                _receiveLogEntryToSendLogEntryMapper);

            _context = new LogRequestMachineContext(
                _requester,
                _sendBuffer,
                _sendLogEntryToLogEventEntityMapper,
                _logEventEntityPool);

            _requestMachine = new LogRequestMachine(_context, new LogRequestStateFactory());
        }

        public void Dispose() {
            _logMessageReceiveHandler.Dispose();
        }
    }
}
