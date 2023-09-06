using LogServiceClient.Runtime.Caches.Utils;

namespace LogServiceClient.Runtime.Pools.Interfaces {
    public interface ILogEntryPool<T> where T : BaseLogEntry<T> {
        T Get();
        void Reclaim(T entry);
    }
}
