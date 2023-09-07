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
        public LogRequestMachineVariables Variables { get; } = new LogRequestMachineVariables();


        private readonly ILogRequestState[] _states;

        public LogRequestMachine(
            LogServiceClientOptions options, 
            ILogRequestMachineContext context, 
            ILogRequestStateFactory stateFactory) {

            Options = options;
            Context = context;

            _states = new ILogRequestState[] { 
                stateFactory.Create(LogRequestStateIndex.PutDevice, this),
                stateFactory.Create(LogRequestStateIndex.GetSession, this),
                stateFactory.Create(LogRequestStateIndex.GetReport, this),
                stateFactory.Create(LogRequestStateIndex.PostEvents, this)
            };
        }

        public async UniTask Run(CancellationToken cancellation = default) {
            IsRunning = true;
            Thread.MemoryBarrier();

            foreach(var state in _states) {
                state.Reset();
            }

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
