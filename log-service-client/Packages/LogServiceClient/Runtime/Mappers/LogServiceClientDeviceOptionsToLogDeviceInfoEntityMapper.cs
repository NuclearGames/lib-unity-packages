using LogServiceClient.Runtime.Mappers.Interfaces;
using LogServiceClient.Runtime.WebRequests.Utils;
using System;

namespace LogServiceClient.Runtime.Mappers {
    public sealed class LogServiceClientDeviceOptionsToLogDeviceInfoEntityMapper : ILogMapper<LogServiceClientDeviceOptions, LogDeviceInfoEntity> {
        public void Copy(LogServiceClientDeviceOptions from, LogDeviceInfoEntity to) {
            to.Model = from.Model;
            to.Name = from.Name;
            to.OperatingSystem = from.OperatingSystem;
            to.OperatingSystemFamily = from.OperatingSystemFamily;
            to.ProcessorType = from.ProcessorType;
        }
    }
}
