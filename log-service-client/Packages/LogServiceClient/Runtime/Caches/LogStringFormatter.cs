using LogServiceClient.Runtime.Caches.Interfaces;

namespace LogServiceClient.Runtime.Caches {
    public sealed class LogStringFormatter : ILogStringFormatter {
        public string Format(string src) {
            return src.Replace("'", "''");
        }
    }
}
