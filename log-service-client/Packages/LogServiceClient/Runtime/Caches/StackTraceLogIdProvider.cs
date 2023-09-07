using LogServiceClient.Runtime.Caches.Interfaces;

namespace LogServiceClient.Runtime.Caches {
    public sealed class StackTraceLogIdProvider : ILogIdProvider {
        private const string UNITY_ENGINE_UNIT_START = "UnityEngine.";
        private const string UNITY_CUSTOM_UNIT_DELIMITER = "|";

        public string Get(string condition, string stackTrace) {
            string unityUnit = null;
            string customUnit = null;
            
            int lineEndingIndex = -1;

            do {
                int lineStartIndex = lineEndingIndex + 1;

                int scopeIndex = stackTrace.IndexOf('(', lineStartIndex);

                int substringLength = stackTrace[scopeIndex - 1] == ' '
                    ? scopeIndex - lineStartIndex - 1
                    : scopeIndex - lineStartIndex;

                string unitName = stackTrace.Substring(lineStartIndex, substringLength);


                if (!unitName.StartsWith(UNITY_ENGINE_UNIT_START)) {
                    customUnit = unitName;
                    break;
                } else if (unityUnit == null) {
                    unityUnit = unitName;
                }

                lineEndingIndex = stackTrace.IndexOf('\n', lineStartIndex);

            } while (lineEndingIndex != -1);


            if (unityUnit != null) {
                return $"{unityUnit}{UNITY_CUSTOM_UNIT_DELIMITER}{customUnit}";
            } else {
                return customUnit;
            }
        }
    }
}
