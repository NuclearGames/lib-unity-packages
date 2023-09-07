using LogServiceClient.Runtime.Exceptions;

namespace LogServiceClient.Runtime.Validation {
    public static class LogServiceDeviceInfoValidation {
        public static void Validate(LogServiceClientOptions options) {
            options.DeviceId = Trunc(options.DeviceId, options.ValidationOptions.DeviceIdMaxLength);

            options.DeviceOptions.Model = Trunc(options.DeviceOptions.Model, options.ValidationOptions.DeviceModelMaxLength);
            options.DeviceOptions.Name = Trunc(options.DeviceOptions.Name, options.ValidationOptions.DeviceNameMaxLength);
            options.DeviceOptions.OperatingSystem = Trunc(options.DeviceOptions.OperatingSystem, options.ValidationOptions.DeviceOperatingSystemMaxLength);
            options.DeviceOptions.OperatingSystemFamily = Trunc(options.DeviceOptions.OperatingSystemFamily, options.ValidationOptions.DeviceOperatingSystemFamilyMaxLength);
            options.DeviceOptions.ProcessorType = Trunc(options.DeviceOptions.ProcessorType, options.ValidationOptions.DeviceProcessorTypeMaxLength);

            if (string.IsNullOrWhiteSpace(options.ServiceAddress)) {
                ExceptionsHelper.ThrowArgumentException("Service Address not provided");
            }

            if (string.IsNullOrWhiteSpace(options.DbId)) {
                ExceptionsHelper.ThrowArgumentException("Db Id not provided");
            }

            if (string.IsNullOrWhiteSpace(options.DeviceId)) {
                ExceptionsHelper.ThrowArgumentException("Device Id not provided");
            }

            if (string.IsNullOrWhiteSpace(options.DeviceOptions.Model)) {
                ExceptionsHelper.ThrowArgumentException("Device Model not provided");
            }

            if (string.IsNullOrWhiteSpace(options.DeviceOptions.Name)) {
                ExceptionsHelper.ThrowArgumentException("Device Name not provided");
            }

            if (string.IsNullOrWhiteSpace(options.DeviceOptions.OperatingSystem)) {
                ExceptionsHelper.ThrowArgumentException("Device OperatingSystem not provided");
            }

            if (options.LogIdProvider == null) {
                ExceptionsHelper.ThrowArgumentException("LogIdProvider not provided");
            }

            if(options.CaptureLogTypes == Enums.LogTypeFlags.Nothing) {
                ExceptionsHelper.ThrowArgumentException("CaptureLogTypes can not be empty");
            }

            if (options.StartSendLogTypes == Enums.LogTypeFlags.Nothing) {
                ExceptionsHelper.ThrowArgumentException("StartSendLogTypes can not be empty");
            }
        }

        private static string Trunc(string src, int maxLen) {
            if(src.Length > maxLen) {
                return src.Substring(0, maxLen);
            }
            return src;
        }
    }
}
