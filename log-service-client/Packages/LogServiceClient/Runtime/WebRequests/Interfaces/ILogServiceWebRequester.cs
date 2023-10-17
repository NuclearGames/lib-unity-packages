using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.WebRequests.Utils;
using Newtonsoft.Json;
using System.Threading;

namespace LogServiceClient.Runtime.WebRequests.Interfaces {
    public interface ILogServiceWebRequester {
        UniTask<LogServiceWebRequestResult> Request(string method, string methodType, object payload, JsonSerializerSettings jsonSettings, CancellationToken cancellation);
    }
}
