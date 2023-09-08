using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.Constants;
using LogServiceClient.Runtime.RequestMachine.Enums;
using LogServiceClient.Runtime.RequestMachine.States;
using LogServiceClient.Runtime.WebRequests.Utils;
using NSubstitute;
using NUnit.Framework;
using System.Threading;

namespace UnitTests.Runtime.RequestMachine.States {
    internal class LogRequestPutDeviceUnitTests {
        [Test]
        public void ExecuteAsync_ReturnsExit_WhenWebRequestFailedAndMaxAttemptsReached() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestPutDeviceUnitTestsFixture()
                    .SetupOptions(x => x.MaxRequestAttempts = 0)
                    .WithRequesterPutDeviceResult(LogServiceRequestResult.Failed())
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                fixture.AssertReturnsExit(result);
            });
        }

        [Test]
        public void ExecuteAsync_ReturnsRetry_WhenWebRequestFailedAndMaxAttemptsNotReached() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestPutDeviceUnitTestsFixture()
                    .SetupOptions(x => x.MaxRequestAttempts = 1)
                    .WithRequesterPutDeviceResult(LogServiceRequestResult.Failed())
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                await fixture.AssertReturnsRetry(result, LogRequestStateIndex.PutDevice);
            });
        }

        [Test]
        public void ExecuteAsync_ReturnsExit_WhenInternalErrorAndMaxAttemptsNotReached() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestPutDeviceUnitTestsFixture()
                    .SetupOptions(x => x.MaxRequestAttempts = 1)
                    .WithRequesterPutDeviceResult(LogServiceRequestResult.Successful(
                            LogServiceResultCodes.PutDevice.Internal.HTTP_CODE,
                            null))
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                fixture.AssertReturnsExit(result);
            });
        }

        [Test]
        public void ExecuteAsync_ReturnsExit_WhenNotFoundDbId() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestPutDeviceUnitTestsFixture()
                    .WithRequesterPutDeviceResult(LogServiceRequestResult.Successful(
                            LogServiceResultCodes.PutDevice.NotFound.HTTP_CODE,
                            LogServiceResultCodes.PutDevice.NotFound.DB_NOT_FOUND))
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                fixture.AssertReturnsExit(result);
            });
        }

        [Test]
        public void ExecuteAsync_ReturnsMoveToGetSession_WhenOk() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestPutDeviceUnitTestsFixture()
                    .WithRequesterPutDeviceResult(LogServiceRequestResult.Successful(
                            LogServiceResultCodes.PutDevice.Ok.HTTP_CODE,
                            null))
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                fixture.AssertReturnsMoveTo(result, LogRequestStateIndex.GetSession);
            });
        }

        [Test]
        public void ExecuteAsync_ReturnsMoveToGetSession_WhenCreated() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestPutDeviceUnitTestsFixture()
                    .WithRequesterPutDeviceResult(LogServiceRequestResult.Successful(
                            LogServiceResultCodes.PutDevice.Created.HTTP_CODE,
                            null))
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                fixture.AssertReturnsMoveTo(result, LogRequestStateIndex.GetSession);
            });
        }
    }

    internal class LogRequestPutDeviceUnitTestsFixture : LogRequestBaseStateFixture<LogRequestPutDeviceUnitTestsFixture> {
        internal LogRequestPutDevice Unit { get; }

        public LogRequestPutDeviceUnitTestsFixture() {
            Unit = new LogRequestPutDevice(Machine);
        }

        internal LogRequestPutDeviceUnitTestsFixture WithRequesterPutDeviceResult(LogServiceRequestResult result) {
            Machine.Context.Requester.PutDevice(Arg.Any<CancellationToken>())
                .Returns(UniTask.FromResult(result));
            return this;
        }
    }
}
