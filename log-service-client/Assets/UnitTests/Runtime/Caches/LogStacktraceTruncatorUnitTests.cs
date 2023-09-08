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
                @"UnityEngine.Debug:Log (object)
Google.Logger:Log (string,Google.LogLevel)
Google.EditorMeasurement/<Report>c__AnonStorey3:<>m__6 ()
Google.EditorMeasurement:PromptToEnable (System.Action)
Google.EditorMeasurement:Report (string,string)
Google.VersionHandlerImpl/<NotifyWhenCompliationComplete>c__AnonStoreyA:<>m__17 ()
Google.RunOnMainThread:ExecutePollingJobs ()
Google.RunOnMainThread:<ExecuteAllUnnested>m__12 ()
Google.RunOnMainThread:RunAction (System.Action)
Google.RunOnMainThread:ExecuteAllUnnested (bool)
Google.RunOnMainThread:ExecuteAll ()
UnityEditor.EditorApplication:Internal_CallUpdateFunctions ()",

                @"UnityEngine.Debug:Log (object)"
            },

            new object[] {
                5,
                @"UnityEngine.Debug:Log (object)
Google.Logger:Log (string,Google.LogLevel)
Google.EditorMeasurement/<Report>c__AnonStorey3:<>m__6 ()
Google.EditorMeasurement:PromptToEnable (System.Action)
Google.EditorMeasurement:Report (string,string)
Google.VersionHandlerImpl/<NotifyWhenCompliationComplete>c__AnonStoreyA:<>m__17 ()
Google.RunOnMainThread:ExecutePollingJobs ()
Google.RunOnMainThread:<ExecuteAllUnnested>m__12 ()
Google.RunOnMainThread:RunAction (System.Action)
Google.RunOnMainThread:ExecuteAllUnnested (bool)
Google.RunOnMainThread:ExecuteAll ()
UnityEditor.EditorApplication:Internal_CallUpdateFunctions ()",

                @"UnityEngine.Debug:Log (object)
Google.Logger:Log (string,Google.LogLevel)
Google.EditorMeasurement/<Report>c__AnonStorey3:<>m__6 ()
Google.EditorMeasurement:PromptToEnable (System.Action)
Google.EditorMeasurement:Report (string,string)"
            },

            new object[] {
                5,
                @"UnityEngine.Debug:Log (object)",
                @"UnityEngine.Debug:Log (object)"
            },

            new object[] {
                5,
                @"UnityEngine.Debug:Log (object)
Google.Logger:Log (string,Google.LogLevel)",

                @"UnityEngine.Debug:Log (object)
Google.Logger:Log (string,Google.LogLevel)"
            }
        };
    }
}
