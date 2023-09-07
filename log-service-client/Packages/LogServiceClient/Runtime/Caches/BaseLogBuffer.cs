using Codice.Client.BaseCommands;
using LogServiceClient.Runtime.Caches.Interfaces;
using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Exceptions;
using LogServiceClient.Runtime.Mappers.Interfaces;
using LogServiceClient.Runtime.Pools.Interfaces;

namespace LogServiceClient.Runtime {
    public abstract class BaseLogBuffer<T> : ILogBuffer<T> where T : BaseLogEntry<T> {
        public int Count { get; private set; }
        public T First { get; private set; }
        protected T Last { get; private set; }

        protected readonly object LockObj = new object();

        private readonly ILogPool<T> _pool;

        public BaseLogBuffer(ILogPool<T> pool) {
            _pool = pool;
        }

        public void MoveFirst<TTarget>(TTarget target, ILogMapper<T, TTarget> mapper) {
            lock (LockObj) {
                if (First == null) {
                    ExceptionsHelper.ThrowInvalidOperationException("Buffer is empty");
                    return;
                }

                mapper.Copy(First, target);
                RemoveFirst();
            }
        }

        public void Clear() {
            lock (LockObj) {
                while(Count > 0) {
                    RemoveFirst();
                }
            }
        }

        protected T GetFromPool() {
            return _pool.Get();
        }

        protected void AddLast(T entry) {
            if (Count == 0) {
                First = Last = entry;
            } else {
                Last.Next = entry;
                Last = entry;
            //    Last = Last.Next = entry;
            }
            Count++;
        }

        protected void RemoveFirst() {
            if(Count == 0) {
                return;
            }

            Count--;
            var itemToRemove = First;

            if(First == Last) {
                First = Last = null;
            } else {
                First = First.Next;
            }

            _pool.Return(itemToRemove);
        }
    }
}
