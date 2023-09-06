namespace LogServiceClient.Runtime.WebRequests.Utils {
    public struct LogServiceRequestResult {
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
        /// Наш внутренний код ошибки.
        /// Если сервер не ответил или запрос успешен, будет иметь значение по умолчанию (null).
        /// </summary>
        public string ErrorCode;

        public static LogServiceRequestResult Successful(long httpCode, string errorCode) {
            return new LogServiceRequestResult() {
                Succeed = true,
                HttpCode = httpCode,
                ErrorCode = errorCode
            };
        }

        public static LogServiceRequestResult Failed() {
            return new LogServiceRequestResult() {
                Succeed = false,
                HttpCode = default,
                ErrorCode = default
            };
        }
    }
}
