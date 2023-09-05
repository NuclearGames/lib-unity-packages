using LogServiceClient.Runtime.Interfaces;

namespace LogServiceClient.Runtime.Pools {
    public sealed class LogEntryPool : ILogEntryPool {
        public LogEntry Get() {
            return new LogEntry();
        }

        public void Reclaim(LogEntry entry) {
            entry.Reset();
        }
    }
}
