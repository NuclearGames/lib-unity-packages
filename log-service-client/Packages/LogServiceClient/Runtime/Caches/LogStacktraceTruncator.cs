using LogServiceClient.Runtime.Caches.Interfaces;


namespace LogServiceClient.Runtime.Caches {
    public sealed class LogStacktraceTruncator : ILogStacktraceTruncator {
        private readonly LogServiceClientOptions _options;

        public LogStacktraceTruncator(LogServiceClientOptions options) {
            _options = options;
        }

        public string Truncate(string src) {
            int lineEndingIndex = -1;

            for (int i = 0; i < _options.StackTraceDeep; i++) {             
                int nextIndex = src.IndexOf('\n', lineEndingIndex + 1);

                lineEndingIndex = nextIndex;

                if (nextIndex == -1) {
                    break;
                }
            }

            if(lineEndingIndex != -1) {
                return src.Substring(0, lineEndingIndex);
            }

            return src;
        }
    }
}
