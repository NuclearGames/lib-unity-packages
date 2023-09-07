using LogServiceClient.Runtime.Caches.Interfaces;
using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Mappers.Interfaces;
using NSubstitute;
using System.Collections.Generic;

namespace UnitTests.TestUtils.DataBuilders {
    internal class TestLogBufferBuilder<T> where T : BaseLogEntry<T> {
        internal ILogBuffer<T> Buffer { get; }

        private readonly List<T> _items = new List<T>();
        private readonly Queue<T> _queue = new Queue<T>();

        public TestLogBufferBuilder() {
            Buffer = Substitute.For<ILogBuffer<T>>();
        }

        internal TestLogBufferBuilder<T> WithItem(T item) {
            _items.Add(item);
            return this;
        }

        internal TestLogBufferBuilder<T> WithMoveFirstGenericHandler<TTarget>() {
            Buffer.When(x => x.MoveFirst(Arg.Any<TTarget>(), Arg.Any<ILogMapper<T, TTarget>>()))
                .Do(x => {
                    var item = _queue.Dequeue();
                    x.Arg<ILogMapper<T, TTarget>>().Copy(
                        item,
                        x.Arg<TTarget>());
                });

            return this;
        }

        internal ILogBuffer<T> Build() {
            foreach(var item in _items) {
                _queue.Enqueue(item); 
            }
            Buffer.Count.Returns(x => _queue.Count);
          

            return Buffer;
        }
    }
}
