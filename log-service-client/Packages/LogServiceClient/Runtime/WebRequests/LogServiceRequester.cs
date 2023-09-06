﻿using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.WebRequests.Interfaces;
using LogServiceClient.Runtime.WebRequests.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Networking;

namespace LogServiceClient.Runtime.WebRequests {
    public sealed class LogServiceRequester : ILogServiceRequester {
        private readonly LogServiceClientOptions _options;

        public LogServiceRequester(LogServiceClientOptions options) {
            _options = options;
        }

        public async UniTask<LogServiceGetSessionResult> GetSession(CancellationToken cancellation) {
            UnityWebRequest www = UnityWebRequest.Get($"{_options.ServiceAddress}/db{_options.DbId}/device_id/{_options.DeviceId}");

            var (result, json) = await PerformRequest(www, cancellation);

            string sessionId = json?.Value<string>("sessionId");

            return new LogServiceGetSessionResult() {
                Request = result,
                SessionId = sessionId
            };
        }

        public async UniTask<LogServiceGetReportResult> GetReport(string sessionId, CancellationToken cancellation) {
            UnityWebRequest www = UnityWebRequest.Get($"{_options.ServiceAddress}/db{_options.DbId}/session_id/{sessionId}");

            var (result, json) = await PerformRequest(www, cancellation);

            string reportId = json?.Value<string>("reportId");

            return new LogServiceGetReportResult() { 
                Request = result,
                ReportId = reportId
            };
        }

        public async UniTask<LogServiceRequestResult> PostEvents(string reportId, List<LogEventEntity> entities, 
            CancellationToken cancellation) {

            string dataJson = JsonConvert.SerializeObject(entities);
    
            UnityWebRequest www = CreatePostRequest($"{_options.ServiceAddress}/db{_options.DbId}/report_id/{reportId}", dataJson);

            var (result, _) = await PerformRequest(www, cancellation);

            return result;
        }

        private async UniTask<(LogServiceRequestResult Result, JObject Json)> PerformRequest(UnityWebRequest www, CancellationToken cancellation) {
            try {
                await www.SendWebRequest().ToUniTask(cancellationToken: cancellation);
            } catch (Exception) {
                return (LogServiceRequestResult.Failed(), null);
            }

            string resultStringData = www.downloadHandler.text;
            bool succeed = www.result == UnityWebRequest.Result.Success || www.result == UnityWebRequest.Result.ProtocolError;

            TryParseData(resultStringData, out var json);
            string errorCode = json?.Value<string>("errorCode");

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