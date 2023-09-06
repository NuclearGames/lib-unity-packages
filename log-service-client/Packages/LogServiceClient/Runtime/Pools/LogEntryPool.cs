using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Pools.Interfaces;

namespace LogServiceClient.Runtime.Pools {
    public sealed class LogEntryPool<T> : ILogEntryPool<T> where T : BaseLogEntry<T>, new() {
        public LogEntryPool(int capacity) {
        }

        public T Get() {
            return new T();
        }

        public void Reclaim(T entry) {
           // entry.Reset();
        }
    }
}
