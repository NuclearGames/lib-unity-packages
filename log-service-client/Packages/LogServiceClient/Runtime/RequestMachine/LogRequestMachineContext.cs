using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.Caches.Interfaces;
using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Mappers.Interfaces;
using LogServiceClient.Runtime.RequestMachine.Interfaces;
using LogServiceClient.Runtime.WebRequests.Interfaces;
using LogServiceClient.Runtime.WebRequests.Utils;
using System.Threading;

namespace LogServiceClient.Runtime.RequestMachine {
    public sealed class LogRequestMachineContext : ILogRequestMachineContext {
        public ILogServiceRequester Requester { get; }

        public ISendLogBuffer SendBuffer { get; }

        public ILogMapper<SendLogEntry, LogEventEntity> SendLogEntryToLogEventEntityMapper { get; }

        public LogRequestMachineContext(
            ILogServiceRequester requester,
            ISendLogBuffer sendBuffer,
            ILogMapper<SendLogEntry, LogEventEntity> sendLogEntryToLogEventEntityMapper) {

            Requester = requester;
            SendBuffer = sendBuffer;
            SendLogEntryToLogEventEntityMapper = sendLogEntryToLogEventEntityMapper;
        }

        public UniTask Delay(int ms, CancellationToken cancellation) {
            throw new System.NotImplementedException();
        }
    }
}
