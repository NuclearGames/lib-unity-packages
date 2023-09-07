using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.Constants;
using LogServiceClient.Runtime.RequestMachine.Enums;
using LogServiceClient.Runtime.RequestMachine.States;
using LogServiceClient.Runtime.WebRequests.Utils;
using NSubstitute;
using NUnit.Framework;
using System.Threading;

namespace UnitTests.Runtime.RequestMachine.States {
    internal class LogRequestGetSessionUnitTests {
        [Test]
        public void ExecuteAsync_ReturnsExit_WhenWebRequestFailedAndMaxAttemptsReached() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestGetSessionUnitTestsFixture()
                    .SetupOptions(x => x.MaxRequestAttempts = 0)
                    .WithRequesterGetSessionResult(new LogServiceGetSessionResult() {
                        Request = LogServiceRequestResult.Failed()
                    })
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                fixture.AssertReturnsExit(result);
            });
        }

        [Test]
        public void ExecuteAsync_ReturnsExit_WhenWebRequestFailedAndMaxAttemptsNotReached() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestGetSessionUnitTestsFixture()
                    .SetupOptions(x => x.MaxRequestAttempts = 1)
                    .WithRequesterGetSessionResult(new LogServiceGetSessionResult() {
                        Request = LogServiceRequestResult.Failed()
                    })
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                fixture.AssertReturnsExit(result);
            });
        }

        [Test]
        public void ExecuteAsync_ReturnsRetry_WhenInternalErrorAndMaxAttemptsNotReached() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestGetSessionUnitTestsFixture()
                    .SetupOptions(x => x.MaxRequestAttempts = 1)
                    .WithRequesterGetSessionResult(new LogServiceGetSessionResult() {
                        Request = LogServiceRequestResult.Successful(
                            LogServiceResultCodes.GetSession.Internal.HTTP_CODE, 
                            null)
                    })
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                await fixture.AssertReturnsRetry(result, LogRequestStateIndex.GetSession);
            });
        }

        [Test]
        public void ExecuteAsync_ReturnsExit_WhenNotFoundDbId() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestGetSessionUnitTestsFixture()
                    .WithRequesterGetSessionResult(new LogServiceGetSessionResult() {
                        Request = LogServiceRequestResult.Successful(
                            LogServiceResultCodes.GetSession.NotFound.HTTP_CODE,
                            LogServiceResultCodes.GetSession.NotFound.DB_NOT_FOUND)
                    })
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                fixture.AssertReturnsExit(result);
            });
        }

        [Test]
        public void ExecuteAsync_ReturnsMoveToPutDevice_WhenNotFoundDeviceId() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestGetSessionUnitTestsFixture()
                    .WithRequesterGetSessionResult(new LogServiceGetSessionResult() {
                        Request = LogServiceRequestResult.Successful(
                            LogServiceResultCodes.GetSession.NotFound.HTTP_CODE,
                            LogServiceResultCodes.GetSession.NotFound.DEVICE_NOT_FOUND)
                    })
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                fixture.AssertReturnsMoveTo(result, LogRequestStateIndex.PutDevice);
            });
        }

        [Test]
        public void ExecuteAsync_ReturnsMoveToGetReport_WhenCreated() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestGetSessionUnitTestsFixture()
                    .WithRequesterGetSessionResult(new LogServiceGetSessionResult() {
                        Request = LogServiceRequestResult.Successful(
                            LogServiceResultCodes.GetSession.Created.HTTP_CODE, 
                            null),
                        SessionId = "SessionIdHere"
                    })
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                fixture.AssertReturnsMoveTo(result, LogRequestStateIndex.GetReport);
                Assert.That(fixture.Machine.Variables.SessionId, Is.EqualTo("SessionIdHere"));
            });
        }
    }

    internal class LogRequestGetSessionUnitTestsFixture : LogRequestBaseStateFixture<LogRequestGetSessionUnitTestsFixture> {
        internal LogRequestGetSession Unit { get; }

        public LogRequestGetSessionUnitTestsFixture() {
            Unit = new LogRequestGetSession(Machine);
        }

        internal LogRequestGetSessionUnitTestsFixture WithRequesterGetSessionResult(LogServiceGetSessionResult result) {
            Machine.Context.Requester.GetSession(Arg.Any<CancellationToken>())
                .Returns(UniTask.FromResult(result));
            return this;
        }
    }
}
