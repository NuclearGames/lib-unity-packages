using LogServiceClient.Runtime.Caches.Interfaces;
using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Exceptions;
using LogServiceClient.Runtime.Mappers.Interfaces;
using LogServiceClient.Runtime.Pools.Interfaces;

namespace LogServiceClient.Runtime {
    public abstract class BaseLogBuffer<T> : ILogBuffer<T> where T : BaseLogEntry<T> {
        public int Count { get; private set; }
        protected T First { get; private set; }
        protected T Last { get; private set; }

        private readonly ILogEntryPool<T> _pool;

        public BaseLogBuffer(ILogEntryPool<T> pool) {
            _pool = pool;
        }

        public void MoveFirst<TTarget>(TTarget target, ILogMapper<T, TTarget> mapper) {
            if (First == null) {
                ExceptionsHelper.ThrowInvalidOperationException("Buffer is empty");
                return;
            }

            mapper.Copy(First, target);
            RemoveFirst();
        }

        protected T GetFromPool() {
            return _pool.Get();
        }

        protected void AddLast(T entry) {
            if (Count == 0) {
                First = Last = entry;
            } else {
                Last = Last.Next = entry;
            }
            Count++;
        }

        protected void RemoveFirst() {
            if(Count == 0) {
                return;
            }

            Count--;
            First = First.Next;
        }
    }
}
