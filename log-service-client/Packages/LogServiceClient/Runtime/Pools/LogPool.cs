using LogServiceClient.Runtime.Pools.Interfaces;
using System.Collections.Generic;

namespace LogServiceClient.Runtime.Pools {
    public sealed class LogPool<T> : ILogPool<T> where T : ILogPoolItem, new() {
        private bool UnlimitedMaxSize => _maxSize == -1;

        private readonly int _maxSize;
        private readonly Queue<T> _items = new Queue<T>();

        public LogPool(int startSize, int maxSize = -1) {
            _maxSize = maxSize;

            for(int i = 0; i < startSize; i++) {
                _items.Enqueue(CreateInstance());
            }
        }

        public T Get() {
            if(_items.Count > 0) {
                return _items.Dequeue();
            }
            return CreateInstance();
        }

        public void Return(T entry) {
            entry.Reset();

            if (_items.Count < _maxSize || UnlimitedMaxSize) {
                _items.Enqueue(entry);
            }
        }

        private T CreateInstance() {
            return new T();
        }
    }
}
