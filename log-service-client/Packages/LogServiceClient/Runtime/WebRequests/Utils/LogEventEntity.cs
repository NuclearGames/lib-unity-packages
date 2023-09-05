namespace LogServiceClient.Runtime.WebRequests.Utils {
    public sealed class LogEventEntity {
        public int Index { get; set; }
        public long Time { get; set; }
        public short Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
