using LogServiceClient.Runtime.Mappers.Interfaces;
using LogServiceClient.Runtime.WebRequests.Utils;


namespace LogServiceClient.Runtime.Mappers {
    public sealed class LogServiceClientDeviceOptionsToLogDeviceInfoEntityMapper : ILogMapper<LogServiceClientDeviceOptions, LogDeviceInfoEntity> {
        public void Copy(LogServiceClientDeviceOptions from, LogDeviceInfoEntity to) {
            to.Model = from.Model;
            to.Name = from.Name;
            to.OperatingSystem = from.OperatingSystem;
            to.OperatingSystemFamily = from.OperatingSystemFamily;
            to.ProcessorType = from.ProcessorType;
        }

        public LogDeviceInfoEntity Map(LogServiceClientDeviceOptions from) {
            return new LogDeviceInfoEntity {
                Model = from.Model,
                Name = from.Name,
                OperatingSystem = from.OperatingSystem,
                OperatingSystemFamily = from.OperatingSystemFamily,
                ProcessorType = from.ProcessorType
            };
        }
    }
}
