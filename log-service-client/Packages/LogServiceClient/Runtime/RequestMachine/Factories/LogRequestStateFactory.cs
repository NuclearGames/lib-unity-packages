using LogServiceClient.Runtime.Exceptions;
using LogServiceClient.Runtime.RequestMachine.Enums;
using LogServiceClient.Runtime.RequestMachine.Interfaces;
using LogServiceClient.Runtime.RequestMachine.States;

namespace LogServiceClient.Runtime.RequestMachine.Factories {
    public class LogRequestStateFactory : ILogRequestStateFactory {
        public ILogRequestState Create(LogRequestStateIndex state, ILogRequestMachineInternal machine) {
            ILogRequestState instance = state switch {
                LogRequestStateIndex.PutDevice => new LogRequestPutDevice(machine),
                LogRequestStateIndex.GetSession => new LogRequestGetSession(machine),
                LogRequestStateIndex.PostReport => new LogRequestPostReport(machine),
                LogRequestStateIndex.PostEvents => new LogRequestPostEvents(machine),
                _ => null
            };

            if(instance == null) {
                ExceptionsHelper.ThrowNotImplementedException();
            }

            return instance;
        }
    }
}
