using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.RequestMachine.Enums;
using LogServiceClient.Runtime.RequestMachine.Interfaces;
using LogServiceClient.Runtime.RequestMachine.Utils;
using System.Threading;

namespace LogServiceClient.Runtime.RequestMachine {
    public sealed class LogRequestMachine : ILogRequestMachineInternal {
        public ILogRequestContext Context { get; }
        public LogRequestStateIndex StateIndex { get; private set; } = LogRequestStateIndex.None;
       // public LogRequestMachineStatus Status { get; private set; } = LogRequestMachineStatus.MoveNext;

        public LogServiceClientOptions Options { get; }
        public LogRequestMachineVariables Variables { get; }

        private readonly ILogRequestState[] _states;

        public LogRequestMachine(ILogRequestStateFactory stateFactory) {
            _states = new ILogRequestState[] { 
                stateFactory.Create(LogRequestStateIndex.PutDevice),
                stateFactory.Create(LogRequestStateIndex.GetSession),
                stateFactory.Create(LogRequestStateIndex.GetReport),
                stateFactory.Create(LogRequestStateIndex.PostEvents)
            };
        }

        public async UniTask Run(CancellationToken cancellation = default) {
            StateIndex = LogRequestStateIndex.GetSession;
            // TODO: GetReport, если есть id сессии. GetSession, если нет.

            while (StateIndex != LogRequestStateIndex.None) {
                var currentState = GetCurrentState();
                var result = await currentState.ExecuteAsync(cancellation);
                StateIndex = result.Index;
            }
        }

        private ILogRequestState GetCurrentState() {
            return _states[(int)StateIndex];
        }
    }
}
