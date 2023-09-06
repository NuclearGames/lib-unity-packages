using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.Caches.Interfaces;
using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Mappers.Interfaces;
using LogServiceClient.Runtime.Pools.Interfaces;
using LogServiceClient.Runtime.RequestMachine.Interfaces;
using LogServiceClient.Runtime.WebRequests.Interfaces;
using LogServiceClient.Runtime.WebRequests.Utils;
using System.Threading;

namespace LogServiceClient.Runtime.RequestMachine {
    public sealed class LogRequestMachineContext : ILogRequestMachineContext {
        public ILogServiceRequester Requester { get; }
        public ILogBuffer<SendLogEntry> SendBuffer { get; }
        public ILogMapper<SendLogEntry, LogEventEntity> SendLogEntryToLogEventEntityMapper { get; }
        public ILogPool<LogEventEntity> LogEventEntityPool { get; }

        public LogRequestMachineContext(
            ILogServiceRequester requester,
            ILogBuffer<SendLogEntry> sendBuffer, 
            ILogMapper<SendLogEntry, LogEventEntity> sendLogEntryToLogEventEntityMapper, 
            ILogPool<LogEventEntity> logEventEntityPool) {

            Requester = requester;
            SendBuffer = sendBuffer;
            SendLogEntryToLogEventEntityMapper = sendLogEntryToLogEventEntityMapper;
            LogEventEntityPool = logEventEntityPool;
        }

        public UniTask Delay(int ms, CancellationToken cancellation) {
            return UniTask.Delay(ms, cancellationToken: cancellation);
        }
    }
}
