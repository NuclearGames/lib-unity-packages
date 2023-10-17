using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.WebRequests.Utils;
using System.Collections.Generic;
using System.Threading;

namespace LogServiceClient.Runtime.WebRequests.Interfaces {
    public interface ILogServiceRequestModule {
        UniTask<LogServiceGetSessionResult> GetSession(CancellationToken cancellation);
        UniTask<LogServicePostReportResult> PostReport(string sessionId, CancellationToken cancellation);
        UniTask<LogServiceRequestResult> PostEvents(string reportId, List<LogEventEntity> entities, CancellationToken cancellation);
        UniTask<LogServiceRequestResult> PutDevice(CancellationToken cancellation);
    }
}
