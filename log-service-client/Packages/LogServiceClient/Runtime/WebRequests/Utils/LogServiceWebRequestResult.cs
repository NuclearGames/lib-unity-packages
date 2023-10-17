using LogServiceClient.Runtime.Constants;

namespace LogServiceClient.Runtime.WebRequests.Utils {
    public struct LogServiceWebRequestResult {
        /// <summary>
        /// <para>
        /// True, если сервер ответил. Это может быть как успех, так и ошибка, возвращенная сервером.
        /// </para>
        /// <para>
        /// False, если не удалось достучаться до сервера. В этом случае остальные поля будут иметь значение по умолчанию.
        /// </para>
        /// </summary>
        public bool Succeed;

        /// <summary>
        /// Http код ответа сервера.
        /// Если сервер не ответил, будет иметь значение по умолчанию (0).
        /// </summary>
        public long HttpCode;

        /// <summary>
        /// Содержимое ответа.
        /// </summary>
        public string ResultData;

        public static LogServiceWebRequestResult Successful(long httpCode, string resultData) {
            return new LogServiceWebRequestResult() {
                Succeed = true,
                HttpCode = httpCode,
                ResultData = resultData
            };
        }

        public static LogServiceWebRequestResult Failed() {
            return new LogServiceWebRequestResult() {
                Succeed = false,
                HttpCode = default,
                ResultData = default
            };
        }
    }
}
