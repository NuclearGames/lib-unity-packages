using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.Constants;
using LogServiceClient.Runtime.RequestMachine.Enums;
using LogServiceClient.Runtime.RequestMachine.Interfaces;
using LogServiceClient.Runtime.RequestMachine.Utils;
using LogServiceClient.Runtime.WebRequests.Utils;
using System.Collections.Generic;
using System.Threading;

namespace LogServiceClient.Runtime.RequestMachine.States {
    public sealed class LogRequestPostEvents : LogRequestBaseState {
        protected override LogRequestStateIndex Index => LogRequestStateIndex.PostEvents;

        private readonly List<LogEventEntity> _events = new List<LogEventEntity>();

        public LogRequestPostEvents(ILogRequestMachineInternal machine) : base(machine) {
        }

        public override async UniTask<LogRequestStateResult> ExecuteAsync(CancellationToken cancellation) {
            FillBuffer();

            var result = await Machine.Context.Requester.PostEvents(
                Machine.Options.DbId, Machine.Variables.ReportId,
                _events, cancellation);

            if (!result.Succeed) {
                return await Retry(cancellation);
            }

            if (LogServiceResultCodes.PostEvents.Internal.Check(result.HttpCode, result.ErrorCode)) {
                return await Retry(cancellation);
            }

            if (LogServiceResultCodes.PostEvents.NotFound.Check(result.HttpCode, result.ErrorCode)) {
                if (result.ErrorCode == LogServiceResultCodes.PostEvents.NotFound.REPORT_NOT_FOUND) {
                    return MoveTo(LogRequestStateIndex.GetReport);
                }
                return Exit();
            }

            if (LogServiceResultCodes.PostEvents.Created.Check(result.HttpCode, result.ErrorCode)) {
                return Machine.Context.SendBuffer.Count > 0
                    ? MoveTo(Index)
                    : Exit();
            }

            return Exit();
        }

        private void FillBuffer() {
            var sendBuffer = Machine.Context.SendBuffer;

            // TODO: пул и заполнение Index. Index мб стоит хранить еще в sendBuffer.
            while (sendBuffer.Count > 0 && _events.Count < Machine.Options.MaxLogsPerRequest) {
                var entity = new LogEventEntity();
                sendBuffer.MoveFirst(entity, Machine.Context.LogEntryToLogEventEntityMapper);
                _events.Add(entity);
            }
        }
    }
}
