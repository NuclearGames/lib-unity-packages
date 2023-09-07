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

            if (string.IsNullOrEmpty(options.DeviceId)) {
                ExceptionsHelper.ThrowArgumentException("Device Id not provided");
            }

            if (string.IsNullOrEmpty(options.DeviceOptions.Model)) {
                ExceptionsHelper.ThrowArgumentException("Device Model not provided");
            }

            if (string.IsNullOrEmpty(options.DeviceOptions.Name)) {
                ExceptionsHelper.ThrowArgumentException("Device Name not provided");
            }

            if (string.IsNullOrEmpty(options.DeviceOptions.OperatingSystem)) {
                ExceptionsHelper.ThrowArgumentException("Device OperatingSystem not provided");
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
