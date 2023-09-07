using LogServiceClient.Runtime;
using LogServiceClient.Runtime.Caches;
using LogServiceClient.Runtime.Mappers.Interfaces;
using LogServiceClient.Runtime.Pools.Interfaces;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace UnitTests.Runtime.Caches {
    internal class ReceiveLogBufferUnitTests {
        [Test]
        public void StoreEntry_SavesEntry() {
            // Arrange.
            var fixture = new ReceiveLogBufferUnitTestsFixture()
                .Build();

            var entry = new ReceiveLogEntry() {
                Condition = "item",
                StackTrace = "trace",
                Type = LogType.Log,
                Time = default
            };

            // Act.
            fixture.InvokeStoreEntry(entry);
            var item = new ReceiveLogEntry();
            fixture.Unit.MoveFirst(item, fixture.ReceiveLogEntrySelfMapper);

            // Assert.
            Assert.That(fixture.Unit.Count, Is.EqualTo(0));
            fixture.AssertReceiveLogEntrySelfMapperCopyInvoked(entry, item);
        }

        [Test]
        public void StoreEntry_RemovesFirstEntryWhenMaxCountReached() {
            // Arrange.
            var fixture = new ReceiveLogBufferUnitTestsFixture()
                .Build();

            var firstEntry = new ReceiveLogEntry() {
                Condition = "item_first",
                StackTrace = "trace",
                Type = LogType.Log,
                Time = default
            };

            var entry = new ReceiveLogEntry() {
                Condition = "item",
                StackTrace = "trace",
                Type = LogType.Log,
                Time = default
            };

            // Act.
            fixture.InvokeStoreEntry(firstEntry);
            fixture.InvokeStoreEntry(entry);
            fixture.InvokeStoreEntry(entry);
            var item = new ReceiveLogEntry();
            fixture.Unit.MoveFirst(item, fixture.ReceiveLogEntrySelfMapper);

            // Assert.
            Assert.That(fixture.Unit.Count, Is.EqualTo(1));
            fixture.AssertReceiveLogEntrySelfMapperCopyInvoked(entry, item);
        }

        [Test]
        public void StoreEntry_UsesPool() {
            // Arrange.
            var fixture = new ReceiveLogBufferUnitTestsFixture()
                .Build();

            var entry = new ReceiveLogEntry() {
                Condition = "item",
                StackTrace = "trace",
                Type = LogType.Log,
                Time = default
            };

            // Act.
            fixture.InvokeStoreEntry(entry);
            fixture.InvokeStoreEntry(entry);
            fixture.InvokeStoreEntry(entry);

            // Assert.
            fixture.Pool.Received(3).Get();
            fixture.Pool.Received(1).Return(Arg.Any<ReceiveLogEntry>());
        }
    }

    internal class ReceiveLogBufferUnitTestsFixture {
        internal ReceiveLogBuffer Unit { get; }
        internal ILogMapper<ReceiveLogEntry, ReceiveLogEntry> ReceiveLogEntrySelfMapper { get; }
        internal ILogPool<ReceiveLogEntry> Pool { get; }

        internal ReceiveLogBufferUnitTestsFixture() {
            ReceiveLogEntrySelfMapper = Substitute.For<ILogMapper<ReceiveLogEntry, ReceiveLogEntry>>();

            Pool = Substitute.For<ILogPool<ReceiveLogEntry>>();
            Pool.Get().Returns(x => new ReceiveLogEntry());

            Unit = new ReceiveLogBuffer(Pool, 2);
        }

        internal ReceiveLogBufferUnitTestsFixture Build() {
            return this;
        }

        internal void InvokeStoreEntry(ReceiveLogEntry entry) {
            Unit.StoreEntry(entry.Condition, entry.StackTrace, entry.Type, entry.Time);
        }

        internal void AssertReceiveLogEntrySelfMapperCopyInvoked(ReceiveLogEntry sourceValues, ReceiveLogEntry target) {
            ReceiveLogEntrySelfMapper.Received().Copy(Arg.Is<ReceiveLogEntry>(
                x => x.Condition == sourceValues.Condition
                && x.StackTrace == sourceValues.StackTrace
                && x.Type == sourceValues.Type
                && x.Time == sourceValues.Time), target);
        }
    }
}
