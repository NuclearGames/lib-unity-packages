using UnityEngine;

namespace LogServiceClient.Runtime {
    public sealed class LogEntry {
        public string Condition { get; private set; }
        public string StackTrace { get; private set; }
        public LogType Type { get; private set; }
        public long Timestamp { get; private set; }

        public LogEntry Next { get; set; }

        public void Initialize(string condition, string stackTrace, LogType type, long timestamp) {
            Condition = condition;
            StackTrace = stackTrace;
            Type = type;
            Timestamp = timestamp;
        }

        public void Reset() {
            Condition = null;
            StackTrace = null;
            Type = default;
            Timestamp = default;
            Next = null;
        }
    }
}
