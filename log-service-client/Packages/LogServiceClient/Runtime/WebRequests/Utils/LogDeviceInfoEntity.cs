using Newtonsoft.Json;


namespace LogServiceClient.Runtime.WebRequests.Utils {
    public sealed class LogDeviceInfoEntity {
        
        [JsonProperty]
        public string Model { get; set; }
        
        [JsonProperty]
        public string Name { get; set; }
        
        [JsonProperty]
        public string OperatingSystem { get; set; }
        
        [JsonProperty]
        public string OperatingSystemFamily { get; set; }
        
        [JsonProperty]
        public string ProcessorType { get; set; }
    }
}
