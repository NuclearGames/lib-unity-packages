using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.Caches.Interfaces;
using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Constants;
using LogServiceClient.Runtime.RequestMachine.Enums;
using LogServiceClient.Runtime.RequestMachine.States;
using LogServiceClient.Runtime.WebRequests.Utils;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using UnitTests.TestUtils.DataBuilders;

namespace UnitTests.Runtime.RequestMachine.States {
    internal class LogRequestPostEventsUnitTests {
        [Test]
        public void ExecuteAsync_ReturnsExit_WhenWebRequestFailedAndMaxAttemptsReached() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestPostEventsUnitTestsFixture()
                    .SetupOptions(x => x.MaxRequestAttempts = 0)
                    .WithRequesterPostEventsResult(LogServiceRequestResult.Failed())
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
                var fixture = new LogRequestPostEventsUnitTestsFixture()
                    .SetupOptions(x => x.MaxRequestAttempts = 1)
                    .WithRequesterPostEventsResult(LogServiceRequestResult.Failed())
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
                var fixture = new LogRequestPostEventsUnitTestsFixture()
                    .SetupOptions(x => x.MaxRequestAttempts = 1)
                    .WithRequesterPostEventsResult(LogServiceRequestResult.Successful(
                            LogServiceResultCodes.PostEvents.Internal.HTTP_CODE,
                            null))
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                await fixture.AssertReturnsRetry(result, LogRequestStateIndex.PostEvents);
            });
        }

        [Test]
        public void ExecuteAsync_ReturnsExit_WhenNotFoundDbId() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestPostEventsUnitTestsFixture()
                    .WithRequesterPostEventsResult(LogServiceRequestResult.Successful(
                            LogServiceResultCodes.PostEvents.NotFound.HTTP_CODE,
                            LogServiceResultCodes.PostEvents.NotFound.DB_NOT_FOUND))
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                fixture.AssertReturnsExit(result);
            });
        }

        [Test]
        public void ExecuteAsync_ReturnsExit_WhenCreatedAndSendBufferEmpty() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestPostEventsUnitTestsFixture()
                    .WithSendBuffer(2)
                    .WithRequesterPostEventsResult(LogServiceRequestResult.Successful(
                            LogServiceResultCodes.PostEvents.Created.HTTP_CODE,
                            null))
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                fixture.AssertReturnsExit(result);
            });
        }

        [Test]
        public void ExecuteAsync_ReturnsMoveToPostEvents_WhenCreatedAndSendBufferNotEmpty() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestPostEventsUnitTestsFixture()
                    .WithSendBuffer(3)
                    .WithRequesterPostEventsResult(LogServiceRequestResult.Successful(
                            LogServiceResultCodes.PostEvents.Created.HTTP_CODE,
                            null))
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                fixture.AssertReturnsMoveTo(result, LogRequestStateIndex.PostEvents);
            });
        }
    }

    internal class LogRequestPostEventsUnitTestsFixture : LogRequestBaseStateFixture<LogRequestPostEventsUnitTestsFixture> {
        internal LogRequestPostEvents Unit { get; }

        public LogRequestPostEventsUnitTestsFixture() {
            Unit = new LogRequestPostEvents(Machine);

            Options.MaxLogsPerRequest = 2;
        }

        internal LogRequestPostEventsUnitTestsFixture WithRequesterPostEventsResult(LogServiceRequestResult result) {
            Machine.Context.Requester.PostEvents(Arg.Any<string>(), Arg.Any<List<LogEventEntity>>(), Arg.Any<CancellationToken>())
                .Returns(UniTask.FromResult(result));
            return this;
        }

        internal LogRequestPostEventsUnitTestsFixture WithSendBuffer(int itemsCount) {
            var builder = new TestLogBufferBuilder<SendLogEntry>();
            for(int i = 0; i < itemsCount; i++) {
                builder.WithItem(new SendLogEntry() { 
                    Index = i
                });
            }
            var buffer = builder
                .WithMoveFirstGenericHandler<LogEventEntity>()
                .Build();

            Machine.Context.SendBuffer.Returns(buffer);
            return this;
        }
    }
}
