using LogServiceClient.Runtime.Caches.Interfaces;
using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Enums;
using LogServiceClient.Runtime.Mappers.Interfaces;
using System;
using UnityEngine;

namespace LogServiceClient.Runtime {
    public sealed class LogMessageReceiveHandler : IDisposable {
        private readonly LogServiceClientOptions _options;
        private readonly IReceiveLogBuffer _receiveBuffer;
        private readonly ISendLogBuffer _sendBuffer;
        private readonly ILogMapper<ReceiveLogEntry, SendLogEntry> _mapper;
        private readonly ILogErrorCache _errorCache;
        private readonly ILogIdProvider _logIdProvider;

        public LogMessageReceiveHandler(
            LogServiceClientOptions options,
            IReceiveLogBuffer receiveBuffer, 
            ISendLogBuffer sendBuffer,
            ILogMapper<ReceiveLogEntry, SendLogEntry> mapper,
            ILogErrorCache errorCache,
            ILogIdProvider logIdProvider) {

            _options = options;
            _receiveBuffer = receiveBuffer;
            _sendBuffer = sendBuffer;
            _mapper = mapper;
            _errorCache = errorCache;
            _logIdProvider = logIdProvider;

            Application.logMessageReceived += OnLogMessageReceived;
        }

        public void Dispose() {
            Application.logMessageReceived -= OnLogMessageReceived;
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType type) {
            if (!_options.CaptureLogTypes.HasType(type)) {
                return;
            }

            string storedStackTrace = _options.CaptureStackTrace
                ? stackTrace
                : null;

            _receiveBuffer
                .StoreEntry(condition, storedStackTrace, type, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds());

            if (_options.StartSendLogTypes.HasType(type)) {
                string id = _logIdProvider.Get(condition, stackTrace);

                if (!_errorCache.Push(id)) {
                    _sendBuffer.MoveAllFrom(_receiveBuffer, _mapper);
                }
            }
        }
    }
}
