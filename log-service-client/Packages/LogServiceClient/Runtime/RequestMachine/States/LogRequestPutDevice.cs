using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.Constants;
using LogServiceClient.Runtime.RequestMachine.Enums;
using LogServiceClient.Runtime.RequestMachine.Interfaces;
using LogServiceClient.Runtime.RequestMachine.Utils;
using System.Threading;

namespace LogServiceClient.Runtime.RequestMachine.States {
    public sealed class LogRequestPutDevice : LogRequestBaseState {
        protected override LogRequestStateIndex Index => LogRequestStateIndex.PutDevice;

        public LogRequestPutDevice(ILogRequestMachineInternal machine) : base(machine) {
        }

        public async override UniTask<LogRequestStateResult> ExecuteAsync(CancellationToken cancellation) {
            var result = await Machine.Context.Requester.PutDevice(cancellation);

            if (!result.Succeed) {
                return await Retry(cancellation);
            }

            if(LogServiceResultCodes.PutDevice.Internal.Check(result.HttpCode, result.ErrorCode)) {
                return await Retry(cancellation);
            }

            if (LogServiceResultCodes.PutDevice.NotFound.Check(result.HttpCode, result.ErrorCode)) {
                return Exit();
            }

            if (LogServiceResultCodes.PutDevice.Ok.Check(result.HttpCode, result.ErrorCode)) {
                return MoveTo(LogRequestStateIndex.GetSession);
            }

            if (LogServiceResultCodes.PutDevice.Created.Check(result.HttpCode, result.ErrorCode)) {
                return MoveTo(LogRequestStateIndex.GetSession);
            }

            return Exit();
        }
    }
}
