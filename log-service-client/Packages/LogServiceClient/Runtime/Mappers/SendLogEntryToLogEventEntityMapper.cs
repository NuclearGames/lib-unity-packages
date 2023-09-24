using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Mappers.Interfaces;
using LogServiceClient.Runtime.WebRequests.Utils;
using UnityEngine;

namespace LogServiceClient.Runtime.Mappers {
    public class SendLogEntryToLogEventEntityMapper : ILogMapper<SendLogEntry, LogEventEntity> {
        public void Copy(SendLogEntry from, LogEventEntity to) {
            to.Index = from.Index;
            to.Message = from.Condition;
            to.StackTrace = from.StackTrace;
            to.Type = MapType(from.Type);
            to.Time = from.Time;
        }

        public LogEventEntity Map(SendLogEntry from) {
            throw new System.NotImplementedException();
        }

        private short MapType(LogType type) {
            return type switch {
                LogType.Error => 0,
                LogType.Assert => 1,
                LogType.Warning => 2,
                LogType.Log => 3,
                LogType.Exception => 4,
                _ => -1
            };
        }
    }
}
