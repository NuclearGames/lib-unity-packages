using LogServiceClient.Runtime.Caches.Interfaces;

namespace LogServiceClient.Runtime.Caches {
    public sealed class ConditionLogIdProvider : ILogIdProvider {
        public string Get(string condition, string stackTrace) {
            return condition;
        }
    }
}
