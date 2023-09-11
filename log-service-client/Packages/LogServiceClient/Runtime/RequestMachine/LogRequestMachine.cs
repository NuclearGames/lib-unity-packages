using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.RequestMachine.Enums;
using LogServiceClient.Runtime.RequestMachine.Interfaces;
using LogServiceClient.Runtime.RequestMachine.Utils;
using System;
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
                stateFactory.Create(LogRequestStateIndex.PostReport, this),
                stateFactory.Create(LogRequestStateIndex.PostEvents, this)
            };
        }

        public async UniTask Run(CancellationToken cancellation = default) {
            IsRunning = true;
            Thread.MemoryBarrier();

            //Debug.Log($"[LogRequestMachine] Started");

            StateIndex = string.IsNullOrEmpty(Variables.SessionId)
              ? LogRequestStateIndex.GetSession
              : LogRequestStateIndex.PostReport;

            //Debug.Log($"[LogRequestMachine] Initial State: {StateIndex}");

            try {

                while (StateIndex != LogRequestStateIndex.None) {
                    var result = await GetCurrentState().ExecuteAsync(cancellation);
                    StateIndex = result.Index;
                    //Debug.Log($"[LogRequestMachine] MoveTo State: {StateIndex}");
                }

            } catch (Exception) { 

            } finally {

                StateIndex = LogRequestStateIndex.None;
                ResetStates();
                Context.SendBuffer.Clear();

                //Debug.Log($"[LogRequestMachine] Finished");

                Thread.MemoryBarrier();
                IsRunning = false;

            }

        }

        private void ResetStates() {
            foreach (var state in _states) {
                state.Reset();
            }
        }

        private ILogRequestState GetCurrentState() {
            return _states[(int)StateIndex];
        }
    }
}
