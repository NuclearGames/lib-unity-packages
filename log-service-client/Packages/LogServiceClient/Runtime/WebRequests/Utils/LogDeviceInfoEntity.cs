using System;
using Newtonsoft.Json;


namespace LogServiceClient.Runtime.WebRequests.Utils {
    [Serializable]
    public sealed class LogDeviceInfoEntity {

        [JsonProperty]
        public string Model;

        [JsonProperty]
        public string Name;

        [JsonProperty]
        public string OperatingSystem;

        [JsonProperty]
        public string OperatingSystemFamily;

        [JsonProperty]
        public string ProcessorType;
    }
}
