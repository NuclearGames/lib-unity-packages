using System;
using LogServiceClient.Runtime.Pools.Interfaces;
using Newtonsoft.Json;


namespace LogServiceClient.Runtime.WebRequests.Utils {
    
    [Serializable]
    public sealed class LogEventEntity : ILogPoolItem {
        [JsonProperty]
        public int Index;

        [JsonProperty]
        public long Time;

        [JsonProperty]
        public short Type;

        [JsonProperty]
        public string Message;

        [JsonProperty]
        public string StackTrace;

        public void Reset() {
            Index = default;
            Time = default;
            Type = default;
            Message = null;
            StackTrace = null;
        }
    }
}
