using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.RequestMachine.Enums;
using LogServiceClient.Runtime.RequestMachine.Interfaces;
using LogServiceClient.Runtime.RequestMachine.Utils;
using System.Threading;

namespace LogServiceClient.Runtime.RequestMachine.States {
    public abstract class LogRequestBaseState : ILogRequestState {
        protected ILogRequestMachineInternal Machine { get; }
        protected int AttemptIndex { get; private set; }
        protected abstract LogRequestStateIndex Index { get; }

        protected LogRequestBaseState(ILogRequestMachineInternal machine) {
            Machine = machine;
        }

        public abstract UniTask<LogRequestStateResult> ExecuteAsync(CancellationToken cancellation);

        protected async UniTask<LogRequestStateResult> Retry(CancellationToken cancellation) {
            if (AttemptIndex < Machine.Options.MaxRequestAttempts) {
                AttemptIndex++;
                await Machine.Context.Delay(Machine.Options.RequestRetryDelayMs, cancellation);
                return new LogRequestStateResult() {
                    Index = Index
                };
            }

            return Exit();
        }

        protected LogRequestStateResult Exit() {
            AttemptIndex = 0;
            return new LogRequestStateResult() { 
                Index = LogRequestStateIndex.None
            };
        }

        protected LogRequestStateResult MoveTo(LogRequestStateIndex index) {
            AttemptIndex = 0;
            return new LogRequestStateResult() { 
                Index = index
            };
        }
    }
}
