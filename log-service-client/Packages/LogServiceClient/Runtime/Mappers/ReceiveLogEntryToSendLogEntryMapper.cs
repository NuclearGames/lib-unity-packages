using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Mappers.Interfaces;

namespace LogServiceClient.Runtime.Mappers {
    public sealed class ReceiveLogEntryToSendLogEntryMapper : ILogMapper<ReceiveLogEntry, SendLogEntry> {
        public void Copy(ReceiveLogEntry from, SendLogEntry to) {
            to.Condition = from.Condition;
            to.StackTrace = from.StackTrace;
            to.Type = from.Type;
            to.Time = from.Time;
        }

        public SendLogEntry Map(ReceiveLogEntry from) {
            throw new System.NotImplementedException();
        }
    }
}
