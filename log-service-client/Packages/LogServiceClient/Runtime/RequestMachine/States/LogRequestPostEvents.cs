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
                Machine.Variables.ReportId,
                _events, cancellation);

            if (!result.Succeed) {
                return await Retry(cancellation);
            }

            if (LogServiceResultCodes.PostEvents.Internal.Check(result.HttpCode, result.ErrorCode)) {
                return Exit();
            }

            if (LogServiceResultCodes.PostEvents.NotFound.Check(result.HttpCode, result.ErrorCode)) {
                if (result.ErrorCode == LogServiceResultCodes.PostEvents.NotFound.REPORT_NOT_FOUND) {
                    return MoveTo(LogRequestStateIndex.GetReport);
                }

                ClearBuffer();
                return Exit();
            }

            if (LogServiceResultCodes.PostEvents.Created.Check(result.HttpCode, result.ErrorCode)) {

                ClearBuffer();

                return Machine.Context.SendBuffer.Count > 0
                    ? MoveTo(Index)
                    : Exit();
            }

            ClearBuffer();
            return Exit();
        }

        public override void Reset() {
            base.Reset();
            ClearBuffer();
        }

        private void FillBuffer() {
            var sendBuffer = Machine.Context.SendBuffer;

            while (sendBuffer.Count > 0 && _events.Count < Machine.Options.MaxLogsPerRequest) {
                var entity =  Machine.Context.LogEventEntityPool.Get();
                sendBuffer.MoveFirst(entity, Machine.Context.SendLogEntryToLogEventEntityMapper);
                _events.Add(entity);
            }
        }

        private void ClearBuffer() {
            foreach(var item in _events) {
                Machine.Context.LogEventEntityPool.Return(item);
            }
            _events.Clear();
        }
    }
}
