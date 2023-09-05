using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.WebRequests.Utils;
using System.Collections.Generic;
using System.Threading;

namespace LogServiceClient.Runtime.WebRequests.Interfaces {
    public interface ILogServiceRequester {
        UniTask<LogServiceRequestResult> PostEvents(string dbId, string reportId, List<LogEventEntity> entities, CancellationToken cancellation);
    }
}
