using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.RequestMachine.Enums;
using LogServiceClient.Runtime.RequestMachine.Interfaces;
using LogServiceClient.Runtime.RequestMachine.Utils;
using System.Threading;

namespace LogServiceClient.Runtime.RequestMachine {
    public sealed class LogRequestMachine : ILogRequestMachineInternal {
        public ILogRequestMachineContext Context { get; }
        public bool IsRunning { get; private set; }
        public LogRequestStateIndex StateIndex { get; private set; } = LogRequestStateIndex.None;

        public LogServiceClientOptions Options { get; }
        public LogRequestMachineVariables Variables { get; }


        private readonly ILogRequestState[] _states;

        public LogRequestMachine(ILogRequestMachineContext context, ILogRequestStateFactory stateFactory) {
            Context = context;

            _states = new ILogRequestState[] { 
                stateFactory.Create(LogRequestStateIndex.PutDevice),
                stateFactory.Create(LogRequestStateIndex.GetSession),
                stateFactory.Create(LogRequestStateIndex.GetReport),
                stateFactory.Create(LogRequestStateIndex.PostEvents)
            };
        }

        public async UniTask Run(CancellationToken cancellation = default) {
            IsRunning = true;
            Thread.MemoryBarrier();

            StateIndex = string.IsNullOrEmpty(Variables.SessionId)
                ? LogRequestStateIndex.GetSession
                : LogRequestStateIndex.GetReport;

            while (StateIndex != LogRequestStateIndex.None) {
                var result = await GetCurrentState().ExecuteAsync(cancellation);
                StateIndex = result.Index;
            }

            Thread.MemoryBarrier();
            IsRunning = false;
        }

        private ILogRequestState GetCurrentState() {
            return _states[(int)StateIndex];
        }
    }
}
