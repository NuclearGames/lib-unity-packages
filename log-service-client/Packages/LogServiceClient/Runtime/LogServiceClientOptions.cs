namespace LogServiceClient.Runtime {
    public sealed class LogServiceClientOptions {
        public string ServiceAddress { get; set; }
        public string DbId { get; set; }

        /// <summary>
        /// Максимальное число логов, которое может быть отправлено за один запрос.
        /// </summary>
        public int MaxLogsPerRequest { get; set; }

        /// <summary>
        /// Максимальное число повторных попыток выполнить запрос.
        /// </summary>
        public int MaxRequestAttempts { get; set; }

        /// <summary>
        /// Задержка между повторными попытками запроса.
        /// </summary>
        public int RequestRetryDelayMs { get; set; }

        /// <summary>
        /// Хранимое число логов. Старые логи вытесняются новыми, когда достигнута максимальная вместимость.
        /// </summary>
        public int ReceiveBufferCapacity { get; set; }

        /// <summary>
        /// Размера пула хранимых логов.
        /// </summary>
        public int ReceiveBufferPoolCapacity { get; set; }
    }
}
