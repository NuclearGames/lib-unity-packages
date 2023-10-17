using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.Exceptions;
using LogServiceClient.Runtime.WebRequests.Constants;
using LogServiceClient.Runtime.WebRequests.Interfaces;
using LogServiceClient.Runtime.WebRequests.Utils;
using Newtonsoft.Json;
using System;
using System.Threading;
using UnityEngine.Networking;

namespace LogServiceClient.Runtime.WebRequests {
    public sealed class LogServiceDefaultWebRequester : ILogServiceWebRequester {
        public UniTask<LogServiceWebRequestResult> Request(string method, string methodType, object payload, JsonSerializerSettings jsonSettings, CancellationToken cancellation) {
            string payloadData = payload != null
                ? SerializePayload(payload, jsonSettings)
                : null;
            
            var www = methodType switch {
                LogServiceWebRequestMethodTypes.GET => CreateGetRequest(method),
                LogServiceWebRequestMethodTypes.POST => CreatePostRequest(method, payloadData),
                LogServiceWebRequestMethodTypes.PUT => CreatePutRequest(method, payloadData),
                _ => null
            };

            if(www == null) {
                ExceptionsHelper.ThrowArgumentException($"{nameof(methodType)} : {methodType}");
                return default;
            }

            return PerformRequest(www, cancellation);
        }

        private string SerializePayload(object payload, JsonSerializerSettings jsonSettings) {
            return jsonSettings != null
                ? JsonConvert.SerializeObject(payload, jsonSettings)
                : JsonConvert.SerializeObject(payload);
        }

        private async UniTask<LogServiceWebRequestResult> PerformRequest(UnityWebRequest www, CancellationToken cancellation) {
            try {
                await www.SendWebRequest().ToUniTask(cancellationToken: cancellation);
            } catch (Exception) { }

            string resultStringData = www.downloadHandler.text;
            bool succeed = www.result == UnityWebRequest.Result.Success || www.result == UnityWebRequest.Result.ProtocolError;

            var result = succeed
                ? LogServiceWebRequestResult.Successful(www.responseCode, resultStringData)
                : LogServiceWebRequestResult.Failed();

            return result;
        }

        private UnityWebRequest CreateGetRequest(string url) {


            UnityWebRequest www = UnityWebRequest.Get(url);
            www.certificateHandler = new IgnoreCertificateHandler();
            return www;
        }

        private UnityWebRequest CreatePostRequest(string url, string dataJson) {
            UnityWebRequest www = UnityWebRequest.Put(url, dataJson);

            www.method = UnityWebRequest.kHttpVerbPOST;
            www.SetRequestHeader("Content-Type", "application/json");
            www.certificateHandler = new IgnoreCertificateHandler();

            return www;
        }

        private UnityWebRequest CreatePutRequest(string url, string dataJson) {
            UnityWebRequest www = UnityWebRequest.Put(url, dataJson);

            www.SetRequestHeader("Content-Type", "application/json");
            www.certificateHandler = new IgnoreCertificateHandler();

            return www;
        }
    }
}
