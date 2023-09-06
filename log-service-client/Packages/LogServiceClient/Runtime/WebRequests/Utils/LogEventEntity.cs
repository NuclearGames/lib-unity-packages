using LogServiceClient.Runtime.Pools.Interfaces;

namespace LogServiceClient.Runtime.WebRequests.Utils {
    public sealed class LogEventEntity : ILogPoolItem {
        public int Index { get; set; }
        public long Time { get; set; }
        public short Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public void Reset() {
            Index = default;
            Time = default;
            Type = default;
            Message = null;
            StackTrace = null;
        }
    }
}
