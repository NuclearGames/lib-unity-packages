namespace LogServiceClient.Runtime.Interfaces {
    public interface ILogEntryPool {
        LogEntry Get();
        void Reclaim(LogEntry entry);
    }
}
