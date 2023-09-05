using LogServiceClient.Runtime;
using LogServiceClient.Runtime.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace UnitTests.Runtime {
    internal class LogBufferUnitTests {
        [Test]
        public void Test() {
            var pool = Substitute.For<ILogEntryPool>();
            pool.Get().Returns(new LogEntry());
            var buffer = new LogBuffer( 2);

        }
    }
}
