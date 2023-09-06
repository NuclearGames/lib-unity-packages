using LogServiceClient.Runtime;
using LogServiceClient.Runtime.Caches;
using LogServiceClient.Runtime.Caches.Interfaces;
using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Mappers.Interfaces;
using LogServiceClient.Runtime.Pools.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests.Runtime.Caches {
    internal class SendLogBufferUnitTests {
        [Test]
        public void MoveAllFrom_MovesEntriesFromSourceBuffer() {
            // Arrange.
            var fixture = new SendLogBufferUnitTestsFixture()
                .WithSourceEntry(new ReceiveLogEntry() { Condition = "item1" })
                .WithSourceEntry(new ReceiveLogEntry() { Condition = "item2" })
                .Build();

            // Act.
            fixture.Unit.MoveAllFrom(fixture.SourceBuffer, fixture.Mapper);

            // Assert.
            fixture.AssertEntriesInBuffer();
        }

        [Test]
        public void MoveAllFrom_SetsEntryIndex() {
            // Arrange.
            var fixture = new SendLogBufferUnitTestsFixture()
                .WithSourceEntry(new ReceiveLogEntry() { Condition = "item1" })
                .WithSourceEntry(new ReceiveLogEntry() { Condition = "item2" })
                .Build();

            // Act.
            fixture.Unit.MoveAllFrom(fixture.SourceBuffer, fixture.Mapper);

            // Assert.
            fixture.AssertEntriesHasSerialIndexes();
        }
    }

    internal class SendLogBufferUnitTestsFixture {
        internal SendLogBuffer Unit { get; }
        internal ILogPool<SendLogEntry> Pool { get; }
        internal ILogMapper<ReceiveLogEntry, SendLogEntry> Mapper { get; }
        internal ILogMapper<SendLogEntry, SendLogEntry> SelfMapper { get; }
        internal ILogBuffer<ReceiveLogEntry> SourceBuffer { get; }

        private readonly List<ReceiveLogEntry> _sourceEntries = new List<ReceiveLogEntry>();
        private readonly Queue<ReceiveLogEntry> _sourceEntriesQueue = new Queue<ReceiveLogEntry>();

        public SendLogBufferUnitTestsFixture() {
            Pool = Substitute.For<ILogPool<SendLogEntry>>();
            Pool.Get().Returns(x => new SendLogEntry());

            Mapper = Substitute.For<ILogMapper<ReceiveLogEntry, SendLogEntry>>();
            SelfMapper = Substitute.For<ILogMapper<SendLogEntry, SendLogEntry>>();

            SourceBuffer = Substitute.For<ILogBuffer<ReceiveLogEntry>>();

            Unit = new SendLogBuffer(Pool);
        }

        internal SendLogBufferUnitTestsFixture WithSourceEntry(ReceiveLogEntry entry) {
            _sourceEntries.Add(entry);
            return this;
        }

        internal SendLogBufferUnitTestsFixture Build() {
            BuildSourceBuffer();

            return this;
        }

        private void BuildSourceBuffer() {
            foreach (var x in _sourceEntries) {
                _sourceEntriesQueue.Enqueue(x);
            }
            SourceBuffer.Count.Returns(x => _sourceEntriesQueue.Count);
            SourceBuffer.When(x => x.MoveFirst(Arg.Any<SendLogEntry>(), Mapper)).Do(x => {
                x.Arg<ILogMapper<ReceiveLogEntry, SendLogEntry>>().Copy(
                    _sourceEntriesQueue.Dequeue(),
                    x.Arg<SendLogEntry>());
            });
        }

        internal void AssertEntriesInBuffer() {
            Assert.That(Unit.Count, Is.EqualTo(_sourceEntries.Count));

            foreach(var entry in _sourceEntries) {
                Mapper.Received().Copy(Arg.Is<ReceiveLogEntry>(
                    x => x.Condition == entry.Condition
                    && x.StackTrace == entry.StackTrace
                    && x.Time == entry.Time
                    && x.Type == entry.Type), Arg.Any<SendLogEntry>());
            }
        }

        internal void AssertEntriesHasSerialIndexes() {
            Assert.That(Unit.Count, Is.EqualTo(_sourceEntries.Count));
            
            var item = new SendLogEntry();
            int index = 0;

            while(Unit.Count > 0) {
                SelfMapper.ClearReceivedCalls();

                Unit.MoveFirst(item, SelfMapper);

                SelfMapper.Received().Copy(Arg.Is<SendLogEntry>(
                    x => x.Index == index), item);

                index++;
            }
        }
    }
}
