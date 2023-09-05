using LogServiceClient.Runtime.Caches.Interfaces;
using LogServiceClient.Runtime.Exceptions;
using LogServiceClient.Runtime.Interfaces;
using LogServiceClient.Runtime.Mappers.Interfaces;

namespace LogServiceClient.Runtime {
    public sealed class LogBuffer : ILogBuffer {
        public int Count { get; private set; }

        private readonly int _maxCount;
        private readonly ILogEntryPool _pool;

        private LogEntry _first, _last;

        public LogBuffer(ILogEntryPool pool, int maxCount) {
            _maxCount = maxCount;
            _pool = pool;
        }

        /// <summary>
        /// Сохраняет лог.
        /// </summary>
        public void StoreEntry(LogEntry entry) {
            AddLast(entry);

            if (Count > _maxCount) {
                RemoveFirst();
            }
        }

      /*  public int CopyChain<T>(T target, ILogMapper<LogEntry, T> mapper, int maxCount) {
            if (_first == null) {
                ExceptionsHelper.ThrowInvalidOperationException("Buffer is empty");
                return 0;
            }

            var current = _first;
            int count = 0;
            while(current != null && count < maxCount) {
                mapper.Copy(_first, target);
                current = _first.Next;
            }
            
        }
*/
        public void MoveFirst<T>(T target, ILogMapper<LogEntry, T> mapper) {
            if (_first == null) {
                ExceptionsHelper.ThrowInvalidOperationException("Buffer is empty");
                return;
            }

            mapper.Copy(_first, target);
            RemoveFirst();
        }

        private void AddLast(LogEntry entry) {
            if (Count == 0) {
                _first = _last = entry;
            } else {
                _last = _last.Next = entry;
            }
            Count++;
        }

        private void RemoveFirst() {
            if(Count == 0) {
                return;
            }

            Count--;
            _first = _first.Next;
        }
    }
}
