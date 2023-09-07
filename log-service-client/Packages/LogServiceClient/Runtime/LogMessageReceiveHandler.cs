using LogServiceClient.Runtime.Caches.Interfaces;
using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Mappers.Interfaces;
using System;
using UnityEngine;

namespace LogServiceClient.Runtime {
    public sealed class LogMessageReceiveHandler : IDisposable {
        private readonly IReceiveLogBuffer _receiveBuffer;
        private readonly ISendLogBuffer _sendBuffer;
        private readonly ILogMapper<ReceiveLogEntry, SendLogEntry> _mapper;
        private readonly ILogErrorCache _errorCache;
        private readonly ILogIdProvider _logIdProvider;

        public LogMessageReceiveHandler(
            IReceiveLogBuffer receiveBuffer, 
            ISendLogBuffer sendBuffer,
            ILogMapper<ReceiveLogEntry, SendLogEntry> mapper,
            ILogErrorCache errorCache,
            ILogIdProvider logIdProvider) {

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
            _receiveBuffer.StoreEntry(condition, stackTrace, type, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds());

            if (type == LogType.Exception || type == LogType.Error) {
                string id = _logIdProvider.Get(condition, stackTrace);

                if (!_errorCache.Push(id)) {
                    _sendBuffer.MoveAllFrom(_receiveBuffer, _mapper);
                }
            }
        }
    }
}
