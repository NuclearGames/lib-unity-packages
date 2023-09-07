using LogServiceClient.Runtime.RequestMachine.Utils;

namespace LogServiceClient.Runtime.RequestMachine.Interfaces {
    public interface ILogRequestMachineInternal {
        public ILogRequestMachineContext Context { get; }
        LogServiceClientOptions Options { get; }
        LogRequestMachineVariables Variables { get; }
    }
}
