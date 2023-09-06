using LogServiceClient.Runtime.Pools.Interfaces;

namespace LogServiceClient.Runtime.Pools {
    public sealed class LogPool<T> : ILogPool<T> where T : ILogPoolItem, new() {
        public LogPool(int capacity) {
        }

        public T Get() {
            return new T();
        }

        public void Return(T entry) {
            entry.Reset();
        }
    }
}
