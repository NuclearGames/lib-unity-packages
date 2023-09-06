using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.Constants;
using LogServiceClient.Runtime.RequestMachine.Enums;
using LogServiceClient.Runtime.RequestMachine.Interfaces;
using LogServiceClient.Runtime.RequestMachine.Utils;
using System.Threading;

namespace LogServiceClient.Runtime.RequestMachine.States {
    public sealed class LogRequestGetSession : LogRequestBaseState {
        public LogRequestGetSession(ILogRequestMachineInternal machine) : base(machine) {
        }

        protected override LogRequestStateIndex Index => LogRequestStateIndex.GetReport;

        public async override UniTask<LogRequestStateResult> ExecuteAsync(CancellationToken cancellation) {
            var result = await Machine.Context.Requester.GetSession(cancellation);

            if (!result.Request.Succeed) {
                return await Retry(cancellation);
            }

            if (LogServiceResultCodes.GetSession.Internal.Check(result.Request.HttpCode, result.Request.ErrorCode)) {
                return await Retry(cancellation);
            }

            if (LogServiceResultCodes.GetSession.NotFound.Check(result.Request.HttpCode, result.Request.ErrorCode)) {
                if (result.Request.ErrorCode == LogServiceResultCodes.GetSession.NotFound.DEVICE_NOT_FOUND) {
                    return MoveTo(LogRequestStateIndex.PutDevice);
                }
                return Exit();
            }

            if (LogServiceResultCodes.GetSession.Created.Check(result.Request.HttpCode, result.Request.ErrorCode)) {
                Machine.Variables.SessionId = result.SessionId;
                return MoveTo(LogRequestStateIndex.PostEvents);
            }

            return Exit();
        }
    }
}
