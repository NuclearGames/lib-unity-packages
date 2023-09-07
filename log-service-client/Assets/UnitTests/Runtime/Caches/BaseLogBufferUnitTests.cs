using LogServiceClient.Runtime;
using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Mappers.Interfaces;
using LogServiceClient.Runtime.Pools.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace UnitTests.Runtime.Caches {
    internal class BaseLogBufferUnitTests {
        [Test]
        public void MoveFirst_RemovesInRightOrder() {
            var fixture = new BaseLogBufferUnitTestsFixture()
                .WithItem(1)
                .WithItem(2)
                .WithItem(3)
                .Build();

            fixture.AssertItemsExtractInRightOrder();
        }
    }

    internal class BaseLogBufferUnitTestsFixture {
        internal TestBuffer Unit { get; }
        internal ILogPool<Item> Pool { get; }
        internal ILogMapper<Item, Item> Mapper { get; }

        private readonly List<int> _items = new List<int>();

        public BaseLogBufferUnitTestsFixture() {
            Pool = Substitute.For<ILogPool<Item>>();
            Pool.Get().Returns(x => new Item());

            Mapper = Substitute.For<ILogMapper<Item, Item>>();

            Unit = new TestBuffer(Pool);
        }

        internal BaseLogBufferUnitTestsFixture WithItem(int value) {
            _items.Add(value);
            return this;
        }

        internal BaseLogBufferUnitTestsFixture Build() {
            foreach(var x in _items) {
                Unit.AddLast(x);
            }
            return this;
        }

        internal void AssertItemsExtractInRightOrder() {
            Assert.That(Unit.Count, Is.EqualTo(_items.Count));
            
            var item = new Item();

            foreach (var value in _items) {
                Mapper.ClearReceivedCalls();

                Unit.MoveFirst(item, Mapper);

                Mapper.Received().Copy(Arg.Is<Item>(x => ValidateValue(x.Value, value)), item);
            }

            Assert.That(Unit.Count, Is.EqualTo(0));
        }

        private bool ValidateValue(int value, int expectedValue) {
            if (value == expectedValue) { 
                return true; 
            }
            Debug.Log($"Expected: {expectedValue}. Was: {value}");
            return false;
        }
    }

    public class Item : BaseLogEntry<Item> {
        internal int Value { get; set; }
    }

    internal class TestBuffer : BaseLogBuffer<Item> {
        public TestBuffer(ILogPool<Item> pool) : base(pool) {
        }

        internal void AddLast(int value) {
            var item = GetFromPool();
            item.Value = value;
            AddLast(item);
        }
    }
}
