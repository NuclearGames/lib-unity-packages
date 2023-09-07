﻿using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.Constants;
using LogServiceClient.Runtime.RequestMachine.Enums;
using LogServiceClient.Runtime.RequestMachine.States;
using LogServiceClient.Runtime.WebRequests.Utils;
using NSubstitute;
using NUnit.Framework;
using System.Threading;

namespace UnitTests.Runtime.RequestMachine.States {
    internal class LogRequestGetReportUnitTests {
        [Test]
        public void ExecuteAsync_ReturnsExit_WhenWebRequestFailedAndMaxAttemptsReached() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestGetReportUnitTestsFixture()
                    .SetupOptions(x => x.MaxRequestAttempts = 0)
                    .WithRequesterGetReportResult(new LogServiceGetReportResult() {
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
                var fixture = new LogRequestGetReportUnitTestsFixture()
                    .SetupOptions(x => x.MaxRequestAttempts = 1)
                    .WithRequesterGetReportResult(new LogServiceGetReportResult() {
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
                var fixture = new LogRequestGetReportUnitTestsFixture()
                    .SetupOptions(x => x.MaxRequestAttempts = 1)
                    .WithRequesterGetReportResult(new LogServiceGetReportResult() {
                        Request = LogServiceRequestResult.Successful(
                            LogServiceResultCodes.GetReport.Internal.HTTP_CODE,
                            null)
                    })
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                await fixture.AssertReturnsRetry(result, LogRequestStateIndex.GetReport);
            });
        }

        [Test]
        public void ExecuteAsync_ReturnsExit_WhenNotFoundDbId() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestGetReportUnitTestsFixture()
                    .WithRequesterGetReportResult(new LogServiceGetReportResult() {
                        Request = LogServiceRequestResult.Successful(
                            LogServiceResultCodes.GetReport.NotFound.HTTP_CODE,
                            LogServiceResultCodes.GetReport.NotFound.DB_NOT_FOUND)
                    })
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                fixture.AssertReturnsExit(result);
            });
        }

        [Test]
        public void ExecuteAsync_ReturnsMoveToGetSession_WhenNotFoundSessionId() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestGetReportUnitTestsFixture()
                    .WithRequesterGetReportResult(new LogServiceGetReportResult() {
                        Request = LogServiceRequestResult.Successful(
                            LogServiceResultCodes.GetReport.NotFound.HTTP_CODE,
                            LogServiceResultCodes.GetReport.NotFound.SESSION_NOT_FOUND)
                    })
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                fixture.AssertReturnsMoveTo(result, LogRequestStateIndex.GetSession);
            });
        }

        [Test]
        public void ExecuteAsync_ReturnsMoveToPostEvents_WhenCreated() {
            AsyncTest.Run(async () => {
                // Arrange.
                var fixture = new LogRequestGetReportUnitTestsFixture()
                    .WithRequesterGetReportResult(new LogServiceGetReportResult() {
                        Request = LogServiceRequestResult.Successful(
                            LogServiceResultCodes.GetReport.Created.HTTP_CODE,
                            null),
                        ReportId = "ReportIdHere"
                    })
                    .Build();

                // Act.
                var result = await fixture.Unit.ExecuteAsync(default);

                // Assert.
                fixture.AssertReturnsMoveTo(result, LogRequestStateIndex.PostEvents);
                Assert.That(fixture.Machine.Variables.ReportId, Is.EqualTo("ReportIdHere"));
            });
        }
    }

    internal class LogRequestGetReportUnitTestsFixture : LogRequestBaseStateFixture<LogRequestGetReportUnitTestsFixture> {
        internal LogRequestGetReport Unit { get; }

        public LogRequestGetReportUnitTestsFixture() {
            Unit = new LogRequestGetReport(Machine);
        }

        internal LogRequestGetReportUnitTestsFixture WithRequesterGetReportResult(LogServiceGetReportResult result) {
            Machine.Context.Requester.GetReport(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(UniTask.FromResult(result));
            return this;
        }
    }
}
