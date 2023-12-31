﻿using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.Constants;
using LogServiceClient.Runtime.RequestMachine.Enums;
using LogServiceClient.Runtime.RequestMachine.Interfaces;
using LogServiceClient.Runtime.RequestMachine.Utils;
using System.Threading;

namespace LogServiceClient.Runtime.RequestMachine.States {
    public sealed class LogRequestPostReport : LogRequestBaseState {
        protected override LogRequestStateIndex Index => LogRequestStateIndex.PostReport;

        public LogRequestPostReport(ILogRequestMachineInternal machine) : base(machine) {
        }

        public async override UniTask<LogRequestStateResult> ExecuteAsync(CancellationToken cancellation) {
            var result = await Machine.Context.Requester.PostReport(Machine.Variables.SessionId, cancellation);

            if (!result.Request.Succeed) {
                return await Retry(cancellation);
            }

            if (LogServiceResultCodes.GetReport.Internal.Check(result.Request.HttpCode, result.Request.ErrorCode)) {
                return Exit();
            }

            if(LogServiceResultCodes.GetReport.NotFound.Check(result.Request.HttpCode, result.Request.ErrorCode)) {
                if (result.Request.ErrorCode == LogServiceResultCodes.GetReport.NotFound.SESSION_NOT_FOUND) {
                    return MoveTo(LogRequestStateIndex.GetSession);
                }
                return Exit();
            }

            if (LogServiceResultCodes.GetReport.Created.Check(result.Request.HttpCode, result.Request.ErrorCode)) {
                Machine.Variables.ReportId = result.ReportId;
                return MoveTo(LogRequestStateIndex.PostEvents);
            }

            return Exit();
        }
    }
}
