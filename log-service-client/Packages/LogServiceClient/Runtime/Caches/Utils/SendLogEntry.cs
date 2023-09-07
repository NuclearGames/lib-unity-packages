using UnityEngine;

namespace LogServiceClient.Runtime.Caches.Utils {
    public sealed class SendLogEntry : BaseLogEntry<SendLogEntry> {
        public string Condition { get; set; }
        public string StackTrace { get; set; }
        public LogType Type { get; set; }
        public long Time { get; set; }
        public int Index { get; set; }

        public override void Reset() {
            base.Reset();
            Condition = null;
            StackTrace = null;
            Type = default;
            Time = default;
            Index = default;
        }
    }
}
