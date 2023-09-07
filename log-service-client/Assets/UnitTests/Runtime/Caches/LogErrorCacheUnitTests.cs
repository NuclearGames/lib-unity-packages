using LogServiceClient.Runtime.Caches;
using NUnit.Framework;

namespace UnitTests.Runtime.Caches {
    internal class LogErrorCacheUnitTests {
        [Test]
        public void Push_ReturnFalse_WhenIdNotInCache() {
            // Arrange.
            var cache = new LogErrorCache(2);

            // Act / Assert.
            Assert.That(cache.Push("one"), Is.False);
        }

        [Test]
        public void Push_ReturnTrue_WhenIdInCache() {
            // Arrange.
            var cache = new LogErrorCache(2);
            cache.Push("one");

            // Act / Assert.
            Assert.That(cache.Push("one"), Is.True);
        }

        [Test]
        public void Push_ReturnFalse_WhenIdOustedFromCache() {
            // Arrange.
            var cache = new LogErrorCache(2);
            cache.Push("one");
            cache.Push("two");
            cache.Push("third");

            // Act / Assert.
            Assert.That(cache.Push("one"), Is.False);
        }
    }
}
