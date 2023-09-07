using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.RequestMachine.Utils;
using System.Threading;

namespace LogServiceClient.Runtime.RequestMachine.Interfaces {
    public interface ILogRequestState {
        UniTask<LogRequestStateResult> ExecuteAsync(CancellationToken cancellation);
        void Reset();
    }
}
