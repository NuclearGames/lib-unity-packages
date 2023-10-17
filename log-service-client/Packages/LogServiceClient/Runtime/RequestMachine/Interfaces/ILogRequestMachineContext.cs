using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.Caches.Interfaces;
using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Mappers.Interfaces;
using LogServiceClient.Runtime.Pools.Interfaces;
using LogServiceClient.Runtime.WebRequests.Interfaces;
using LogServiceClient.Runtime.WebRequests.Utils;
using System.Threading;

namespace LogServiceClient.Runtime.RequestMachine.Interfaces {
    public interface ILogRequestMachineContext {
        ILogServiceRequestModule Requester { get; }
        ILogBuffer<SendLogEntry> SendBuffer { get; }
        ILogMapper<SendLogEntry, LogEventEntity> SendLogEntryToLogEventEntityMapper { get; }
        ILogPool<LogEventEntity> LogEventEntityPool { get; }

        UniTask Delay(int ms, CancellationToken cancellation);
    }
}
