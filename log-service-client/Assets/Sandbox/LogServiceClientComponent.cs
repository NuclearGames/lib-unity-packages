using LogServiceClient.Runtime;
using LogServiceClient.Runtime.Caches;
using LogServiceClient.Runtime.External.Interfaces;
using UnityEngine;

namespace Sandbox {
    internal class LogServiceClientComponent : MonoBehaviour {
        private LogServiceClientCore _client;

        private void Awake() {
            _client = new LogServiceClientCore(new LogServiceClientOptions() { 
                ValidationOptions = new LogServiceClientValidationOptions(),

                ServiceAddress = "https://warplane-inc-online.logservice.game-logs.online/log",
                DbId = "warplane_inc_online",

                DeviceId = SystemInfo.deviceUniqueIdentifier,
                DeviceOptions = new LogServiceClientDeviceOptions() {
                    Model = SystemInfo.deviceModel,
                    Name = SystemInfo.deviceName,
                    OperatingSystem = SystemInfo.operatingSystem,
                    OperatingSystemFamily = SystemInfo.operatingSystemFamily.ToString(),
                    ProcessorType = SystemInfo.processorType,
                },

                StackTraceDeep = 2,

                //LogIdProvider = new ConditionLogIdProvider()
                LogIdProvider = new StackTraceLogIdProvider(),

                UserSettingsProvider = new SettingsProvider()
            });
        }

        private void OnDestroy() {
            _client.Dispose();
        }

        [ContextMenu("ShowDeviceInfoSize")]
        private void ShowDeviceInfoSize() {
            Debug.Log($"Id: {SystemInfo.deviceUniqueIdentifier.Length} ({SystemInfo.deviceUniqueIdentifier})");
            Debug.Log($"Model: {SystemInfo.deviceModel.Length} ({SystemInfo.deviceModel})");
            Debug.Log($"Name: {SystemInfo.deviceName.Length} ({SystemInfo.deviceName})");
            Debug.Log($"OperatingSystem: {SystemInfo.operatingSystem.Length} ({SystemInfo.operatingSystem})");
            Debug.Log($"OperatingSystemFamily: {SystemInfo.operatingSystemFamily.ToString().Length} ({SystemInfo.operatingSystemFamily.ToString()})");
            Debug.Log($"ProcessorType: {SystemInfo.processorType.Length} ({SystemInfo.processorType})");
        }

        private class SettingsProvider : IUserSettingsProvider {
            public string Get() {
                return "some settings...";
            }
        }
    }
}
