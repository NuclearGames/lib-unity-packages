using LogServiceClient.Runtime.Caches.Interfaces;
using LogServiceClient.Runtime.Pools.Interfaces;
using UnityEngine;

namespace LogServiceClient.Runtime.Caches {
    public sealed class ReceiveLogBuffer : BaseLogBuffer<ReceiveLogEntry>, IReceiveLogBuffer {
        private readonly int _maxCount;

        public ReceiveLogBuffer(ILogPool<ReceiveLogEntry> pool, int maxCount) : base(pool) {
            _maxCount = maxCount;
        }

        public void StoreEntry(string condition, string stackTrace, LogType logType, long timestamp) {
            var entry = GetFromPool();

            entry.Condition = condition;
            entry.StackTrace = stackTrace;
            entry.Type = logType;
            entry.Timestamp = timestamp;

            AddLast(entry);

            if(Count > _maxCount) {
                RemoveFirst();
            }
        }
    }
}
