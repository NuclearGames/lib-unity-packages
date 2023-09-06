using LogServiceClient.Runtime;
using UnityEngine;

namespace Sandbox {
    internal class LogServiceClientComponent : MonoBehaviour {
        private LogServiceClientCore _client;

        private void Awake() {
            _client = new LogServiceClientCore(new LogServiceClientOptions() { 
                ServiceAddress = "",
                DbId = "warplane_inc_online",
                DeviceId = SystemInfo.deviceUniqueIdentifier,
                DeviceOptions = new LogServiceClientDeviceOptions() { 
                    Model = SystemInfo.deviceModel,
                    Name = SystemInfo.deviceName,
                    OperatingSystem = SystemInfo.operatingSystem,
                    OperatingSystemFamily = SystemInfo.operatingSystemFamily.ToString(),
                    ProcessorType = SystemInfo.processorType,
                },

                MaxRequestAttempts = 5,
                RequestRetryDelayMs = 1000,

                ReceiveBufferCapacity = 50,
                MaxLogsPerRequest = 50,

                ReceiveBufferPoolCapacity = 51,
                SendBufferPoolCapacity = 60,
                EventEntityPoolCapacity = 60
            });
        }

        private void OnDestroy() {
            _client.Dispose();
        }
    }
}
