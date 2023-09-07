using LogServiceClient.Runtime.RequestMachine.Enums;

namespace LogServiceClient.Runtime.RequestMachine.Interfaces {
    public interface ILogRequestStateFactory {
        ILogRequestState Create(LogRequestStateIndex state, ILogRequestMachineInternal machine);
    }
}
