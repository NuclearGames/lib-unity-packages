using LogServiceClient.Runtime;
using LogServiceClient.Runtime.Caches;
using NUnit.Framework;

namespace UnitTests.Runtime.Caches {
    internal class LogStacktraceTruncatorUnitTests {
        [TestCaseSource(nameof(Truncate_ReturnsExpectedString_Cases))]
        public void Truncate_ReturnsExpectedString(int deep, string input, string expectedResult) {
            // Arrange.
            var options = new LogServiceClientOptions() { 
                StackTraceDeep = deep
            };
            var truncator = new LogStacktraceTruncator(options);

            // Act.
            string result = truncator.Truncate(input);

            // Assert.
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private static readonly object[] Truncate_ReturnsExpectedString_Cases = new object[] { 
            new object[] { 
                1,
                "UnityEngine.Debug:Log (object)\nGoogle.Logger:Log (string,Google.LogLevel)\nGoogle.EditorMeasurement/<Report>c__AnonStorey3:<>m__6 ()\nGoogle.EditorMeasurement:PromptToEnable (System.Action)\nGoogle.EditorMeasurement:Report (string,string)\nGoogle.VersionHandlerImpl/<NotifyWhenCompliationComplete>c__AnonStoreyA:<>m__17 ()\nGoogle.RunOnMainThread:ExecutePollingJobs ()\nGoogle.RunOnMainThread:<ExecuteAllUnnested>m__12 ()",

                @"UnityEngine.Debug:Log (object)"
            },

            new object[] {
                5,
                "UnityEngine.Debug:Log (object)\nGoogle.Logger:Log (string,Google.LogLevel)\nGoogle.EditorMeasurement/<Report>c__AnonStorey3:<>m__6 ()\nGoogle.EditorMeasurement:PromptToEnable (System.Action)\nGoogle.EditorMeasurement:Report (string,string)\nGoogle.VersionHandlerImpl/<NotifyWhenCompliationComplete>c__AnonStoreyA:<>m__17 ()\nGoogle.RunOnMainThread:ExecutePollingJobs ()\nGoogle.RunOnMainThread:<ExecuteAllUnnested>m__12 ()",

                "UnityEngine.Debug:Log (object)\nGoogle.Logger:Log (string,Google.LogLevel)\nGoogle.EditorMeasurement/<Report>c__AnonStorey3:<>m__6 ()\nGoogle.EditorMeasurement:PromptToEnable (System.Action)\nGoogle.EditorMeasurement:Report (string,string)"
            },

            new object[] {
                5,
                @"UnityEngine.Debug:Log (object)",
                @"UnityEngine.Debug:Log (object)"
            },

            new object[] {
                5,
                "UnityEngine.Debug:Log (object)\nGoogle.Logger:Log (string,Google.LogLevel)",

                "UnityEngine.Debug:Log (object)\nGoogle.Logger:Log (string,Google.LogLevel)"
            },

            new object[]{ 
                2,
                "UnityEngine.Debug:LogError (object)\nSandbox.TestLogGenerator/<LogSequence>d__6:MoveNext () (at Assets/Sandbox/TestLogGenerator.cs:38)\nSystem.Runtime.CompilerServices.AsyncVoidMethodBuilder:Start<Sandbox.TestLogGenerator/<LogSequence>d__6> (Sandbox.TestLogGenerator/<LogSequence>d__6&)\nSandbox.TestLogGenerator:LogSequence ()",

                "UnityEngine.Debug:LogError (object)\nSandbox.TestLogGenerator/<LogSequence>d__6:MoveNext () (at Assets/Sandbox/TestLogGenerator.cs:38)"
            }
        };
    }
}
