using System;
using LogServiceClient.Runtime.Pools.Interfaces;
using Newtonsoft.Json;


namespace LogServiceClient.Runtime.WebRequests.Utils {
    
    [Serializable]
    public sealed class LogEventEntity : ILogPoolItem {
        [JsonProperty]
        public int Index { get; set; }
        
        [JsonProperty]
        public long Time { get; set; }
        
        [JsonProperty]
        public short Type { get; set; }
        
        [JsonProperty]
        public string Message { get; set; }
        
        [JsonProperty]
        public string StackTrace { get; set; }

        public void Reset() {
            Index = default;
            Time = default;
            Type = default;
            Message = null;
            StackTrace = null;
        }
    }
}
