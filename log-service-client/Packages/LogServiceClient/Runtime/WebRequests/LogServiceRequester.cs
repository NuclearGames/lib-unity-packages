using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.Constants;
using LogServiceClient.Runtime.Mappers.Interfaces;
using LogServiceClient.Runtime.WebRequests.Interfaces;
using LogServiceClient.Runtime.WebRequests.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Networking;

namespace LogServiceClient.Runtime.WebRequests {
    public sealed class LogServiceRequester : ILogServiceRequester {
        private readonly LogServiceClientOptions _options;
        private readonly ILogMapper<LogServiceClientDeviceOptions, LogDeviceInfoEntity> _mapper;

        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private readonly Dictionary<string, object> _jsonMapBuffer = new Dictionary<string, object>();

        private LogDeviceInfoEntity _deviceInfoEntity;

        public LogServiceRequester(
            LogServiceClientOptions options, 
            ILogMapper<LogServiceClientDeviceOptions, LogDeviceInfoEntity> mapper) {

            _options = options;
            _mapper = mapper;
        }

        public async UniTask<LogServiceRequestResult> PutDevice(CancellationToken cancellation) {
            _deviceInfoEntity ??= new LogDeviceInfoEntity();
            _mapper.Copy(_options.DeviceOptions, _deviceInfoEntity);

            string dataJson = JsonConvert.SerializeObject(_deviceInfoEntity, _jsonSettings);

            UnityWebRequest www = CreatePutRequest($"{_options.ServiceAddress}/device_id/db/{_options.DbId}/device_id/{_options.DeviceId}",
                dataJson);

            var (result, _) = await PerformRequest(www, cancellation);

            return result;
        }

        public async UniTask<LogServiceGetSessionResult> GetSession(CancellationToken cancellation) {
            UnityWebRequest www = UnityWebRequest.Get($"{_options.ServiceAddress}/session_id/db/{_options.DbId}/device_id/{_options.DeviceId}");

            var (result, json) = await PerformRequest(www, cancellation);

            string sessionId = json?.Value<string>("sessionId");

            return new LogServiceGetSessionResult() {
                Request = result,
                SessionId = sessionId
            };
        }

        public async UniTask<LogServiceGetReportResult> GetReport(string sessionId, CancellationToken cancellation) {
            UnityWebRequest www = UnityWebRequest.Get($"{_options.ServiceAddress}/report_id/db/{_options.DbId}/session_id/{sessionId}");

            var (result, json) = await PerformRequest(www, cancellation);

            string reportId = json?.Value<string>("reportId");

            return new LogServiceGetReportResult() { 
                Request = result,
                ReportId = reportId
            };
        }

        public async UniTask<LogServiceRequestResult> PostEvents(string reportId, List<LogEventEntity> entities, 
            CancellationToken cancellation) {

            _jsonMapBuffer.Clear();
            _jsonMapBuffer["entities"] = entities;
            string dataJson = JsonConvert.SerializeObject(_jsonMapBuffer, _jsonSettings);
    
            UnityWebRequest www = CreatePostRequest($"{_options.ServiceAddress}/events/db/{_options.DbId}/report_id/{reportId}", dataJson);

            var (result, _) = await PerformRequest(www, cancellation);

            return result;
        }

        private async UniTask<(LogServiceRequestResult Result, JObject Json)> PerformRequest(UnityWebRequest www, CancellationToken cancellation) {
            try {
                await www.SendWebRequest().ToUniTask(cancellationToken: cancellation);
            } catch (Exception) { }

            string resultStringData = www.downloadHandler.text;
            bool succeed = www.result == UnityWebRequest.Result.Success || www.result == UnityWebRequest.Result.ProtocolError;

            TryParseData(resultStringData, out var json);
            var errorCode = (LogServiceInternalResultCodes)json?.Value<int>("errorCode");

            //UnityEngine.Debug.Log($"[LogServiceRequester] ({www.uri}) {www.result}, {resultStringData}");

            var result = succeed
                ? LogServiceRequestResult.Successful(www.responseCode, errorCode)
                : LogServiceRequestResult.Failed();

            return (result, json);
        }

        private UnityWebRequest CreatePostRequest(string url, string dataJson) {
            UnityWebRequest www = UnityWebRequest.Put(url, dataJson);

            www.method = UnityWebRequest.kHttpVerbPOST;
            www.SetRequestHeader("Content-Type", "application/json");

            return www;
        }

        private UnityWebRequest CreatePutRequest(string url, string dataJson) {
            UnityWebRequest www = UnityWebRequest.Put(url, dataJson);

            www.SetRequestHeader("Content-Type", "application/json");

            return www;
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
