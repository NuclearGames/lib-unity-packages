using LogServiceClient.Runtime;
using NSubstitute;
using NUnit.Framework;

namespace UnitTests.Runtime {
    internal class LogBufferUnitTests {
        [Test]
        public void Test() {
            var pool = Substitute.For<ILogEntryPool>();
            pool.Get().Returns(new ReceiveLogEntry());
            var buffer = new LogBuffer( 2);

        }
    }
}
