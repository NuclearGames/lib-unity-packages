using LogServiceClient.Runtime.RequestMachine.Enums;
using LogServiceClient.Runtime.RequestMachine.Interfaces;
using System;

namespace LogServiceClient.Runtime.RequestMachine.Factories {
    public class LogRequestStateFactory : ILogRequestStateFactory {
        public ILogRequestState Create(LogRequestStateIndex state) {
            throw new NotImplementedException();
        }
    }
}
