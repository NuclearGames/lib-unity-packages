using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.Caches.Interfaces;
using LogServiceClient.Runtime.Mappers.Interfaces;
using LogServiceClient.Runtime.WebRequests.Interfaces;
using LogServiceClient.Runtime.WebRequests.Utils;
using System.Threading;

namespace LogServiceClient.Runtime.RequestMachine.Interfaces {
    public interface ILogRequestContext {
        ILogServiceRequester Requester { get; }
        ILogBuffer SendBuffer { get; }
        ILogMapper<LogEntry, LogEventEntity> LogEntryToLogEventEntityMapper { get; }

        UniTask Delay(int ms, CancellationToken cancellation);
    }
}
