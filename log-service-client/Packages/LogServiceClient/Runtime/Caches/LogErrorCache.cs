using LogServiceClient.Runtime.Caches.Interfaces;

namespace LogServiceClient.Runtime.Caches {
    public sealed class LogErrorCache : ILogErrorCache {
        private readonly LRUCache<string, bool> _cache;

        public LogErrorCache(int capacity) {
            _cache = new LRUCache<string, bool>(capacity);
        }

        public bool Push(string id) {
            bool contains = _cache.TryGet(id, out _);
            _cache.Set(id, true);
            return contains;
        }
    }
}
