using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.Constants;
using LogServiceClient.Runtime.Exceptions;
using LogServiceClient.Runtime.External.Interfaces;
using LogServiceClient.Runtime.Mappers.Interfaces;
using LogServiceClient.Runtime.WebRequests.Constants;
using LogServiceClient.Runtime.WebRequests.Interfaces;
using LogServiceClient.Runtime.WebRequests.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace LogServiceClient.Runtime.WebRequests {
    public sealed class LogServiceRequestModule : ILogServiceRequestModule {
        private readonly LogServiceClientOptions _options;
        private readonly ILogMapper<LogServiceClientDeviceOptions, LogDeviceInfoEntity> _mapper;
        private readonly IUserSettingsProvider _userSettingsProvider;
        private readonly ILogServiceWebRequester _webRequester;

        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private readonly Dictionary<string, object> _jsonMapBuffer = new Dictionary<string, object>();

        private LogDeviceInfoEntity _deviceInfoEntity;

        public LogServiceRequestModule(
            LogServiceClientOptions options, 
            ILogMapper<LogServiceClientDeviceOptions, LogDeviceInfoEntity> mapper,
            ILogServiceWebRequester webRequester) {

            _options = options;
            _mapper = mapper;
            _userSettingsProvider = options.UserSettingsProvider;
            _webRequester = webRequester;
        }

        public async UniTask<LogServiceRequestResult> PutDevice(CancellationToken cancellation) {
            _deviceInfoEntity = new LogDeviceInfoEntity();
            _mapper.Copy(_options.DeviceOptions, _deviceInfoEntity);

            string url = $"{_options.ServiceAddress}/device_id/db/{_options.DbId}/device_id/{_options.DeviceId}";

            var rawResult = await _webRequester.Request(url, LogServiceWebRequestMethodTypes.PUT, _deviceInfoEntity, _jsonSettings, cancellation);
            ParseResult(url, rawResult, out var result, out var json);

            return result;
        }

        public async UniTask<LogServiceGetSessionResult> GetSession(CancellationToken cancellation) {
            string url = $"{_options.ServiceAddress}/session_id/db/{_options.DbId}/device_id/{_options.DeviceId}";

            var rawResult = await _webRequester.Request(url, LogServiceWebRequestMethodTypes.GET, null, null, cancellation);
            ParseResult(url, rawResult, out var result, out var json);

            string sessionId = json?.Value<string>("sessionId");

            return new LogServiceGetSessionResult() {
                Request = result,
                SessionId = sessionId
            };
        }

        public async UniTask<LogServicePostReportResult> PostReport(string sessionId, CancellationToken cancellation) {
            if(string.IsNullOrWhiteSpace(sessionId)) {
                ExceptionsHelper.ThrowArgumentException();
            }

            string url = $"{_options.ServiceAddress}/report_info/db/{_options.DbId}/session_id/{sessionId}";

            _jsonMapBuffer.Clear();
            _jsonMapBuffer["userSettings"] = _userSettingsProvider.Get();

            var rawResult = await _webRequester.Request(url, LogServiceWebRequestMethodTypes.POST, _jsonMapBuffer, _jsonSettings, cancellation);
            ParseResult(url, rawResult, out var result, out var json);

            string reportId = json?.Value<string>("reportId");

            return new LogServicePostReportResult() { 
                Request = result,
                ReportId = reportId
            };
        }

        public async UniTask<LogServiceRequestResult> PostEvents(string reportId, List<LogEventEntity> entities, 
            CancellationToken cancellation) {

            if (string.IsNullOrWhiteSpace(reportId)) {
                ExceptionsHelper.ThrowArgumentException();
            }

            if (entities.Count == 0) {
                ExceptionsHelper.ThrowArgumentException();
            }
            
            _jsonMapBuffer.Clear();
            _jsonMapBuffer["entities"] = entities;

            string url = $"{_options.ServiceAddress}/events/db/{_options.DbId}/report_id/{reportId}";

            var rawResult = await _webRequester.Request(url, LogServiceWebRequestMethodTypes.POST, _jsonMapBuffer, _jsonSettings, cancellation);
            ParseResult(url, rawResult, out var result, out var json);

            return result;
        }

        private void ParseResult(string method, LogServiceWebRequestResult srcResult, out LogServiceRequestResult result, out JObject json) {
            TryParseData(srcResult.ResultData, out json);

            var errorCode = (LogServiceInternalResultCodes?)json?.Value<int>("errorCode");

            if (_options.DebugMode) {
                Debug.Log($"[LogServiceRequester - Response]: ({method}) {srcResult.Succeed}, {srcResult.ResultData}");
            }

            result = srcResult.Succeed
                ? LogServiceRequestResult.Successful(srcResult.HttpCode, errorCode)
                : LogServiceRequestResult.Failed();
        }

        private static bool TryParseData(string data, out JObject json) {
            try {
                json = JObject.Parse(data);
                return true;
            } catch (JsonReaderException) {
                json = null;
                return false;
            }
        }
    }
}
