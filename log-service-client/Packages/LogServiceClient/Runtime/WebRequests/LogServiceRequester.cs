using Cysharp.Threading.Tasks;
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

        public async UniTask<LogServiceRequestResult> PostEvents(string dbId, string reportId, List<LogEventEntity> entities, 
            CancellationToken cancellation) {

            string dataJson = JsonConvert.SerializeObject(entities);
    
            UnityWebRequest www = CreatePostRequest($"{_options.ServiceAddress}/db{dbId}/report_id/{reportId}", dataJson);

            try {
                await www.SendWebRequest().ToUniTask(cancellationToken: cancellation);
            } catch (Exception) { }

            string resultStringData = www.downloadHandler.text;

            bool succeed = www.result == UnityWebRequest.Result.Success || www.result == UnityWebRequest.Result.ProtocolError;

            TryParseData(resultStringData, out var json);
            string errorCode = json?.Value<string>("errorCode");

            return succeed
                ? LogServiceRequestResult.Successful(www.responseCode, errorCode)
                : LogServiceRequestResult.Failed();
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
