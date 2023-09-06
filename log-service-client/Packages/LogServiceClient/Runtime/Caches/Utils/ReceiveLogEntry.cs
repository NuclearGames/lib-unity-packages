using LogServiceClient.Runtime.Caches.Utils;
using UnityEngine;

namespace LogServiceClient.Runtime {
    public sealed class ReceiveLogEntry : BaseLogEntry<ReceiveLogEntry> {
        public string Condition { get; set; }
        public string StackTrace { get; set; }
        public LogType Type { get; set; }
        public long Timestamp { get; set; }

        public override void Reset() {
            base.Reset();
            Condition = null;
            StackTrace = null;
            Type = default;
            Timestamp = default;
        }
    }
}
