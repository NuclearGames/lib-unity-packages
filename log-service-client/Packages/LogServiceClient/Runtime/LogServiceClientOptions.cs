using LogServiceClient.Runtime.Caches.Interfaces;

namespace LogServiceClient.Runtime {
    public sealed class LogServiceClientOptions {
        public string ServiceAddress { get; set; }
        public string DbId { get; set; }
        public string DeviceId { get; set; }
        public LogServiceClientDeviceOptions DeviceOptions { get; set; }
        public ILogIdProvider LogIdProvider { get; set; }

        /// <summary>
        /// Максимальное число логов, которое может быть отправлено за один запрос.
        /// </summary>
        public int MaxLogsPerRequest { get; set; } = 50;

        /// <summary>
        /// Максимальное число повторных попыток выполнить запрос.
        /// </summary>
        public int MaxRequestAttempts { get; set; } = 5;

        /// <summary>
        /// Задержка между повторными попытками запроса.
        /// </summary>
        public int RequestRetryDelayMs { get; set; } = 1000;

        /// <summary>
        /// Хранимое число логов. Старые логи вытесняются новыми, когда достигнута максимальная вместимость.
        /// </summary>
        public int ReceiveBufferCapacity { get; set; } = 50;

        /// <summary>
        /// Максимальное кол-во логов, которые могут храниться в буфере для отправки.
        /// Служит священной цели не поймать переполнение кучи, если сервер не отвечает.
        /// </summary>
        public int SendBufferCapacity { get; set; } = 200;

        /// <summary>
        /// Вместимость кэша ошибок.
        /// </summary>
        public int ErrorCacheCapacity { get; set; } = 10;

        /// <summary>
        /// Размера пула хранимых логов.
        /// </summary>
        public int ReceiveBufferPoolCapacity { get; set; } = 51;

        /// <summary>
        /// Размера пула отправляемых логов.
        /// </summary>
        public int SendBufferPoolStartCapacity { get; set; } = 50;

        /// <summary>
        /// Размера пула отправляемых логов.
        /// </summary>
        public int SendBufferPoolMaxCapacity { get; set; } = 100;

        /// <summary>
        /// Размера пула отправляемых сущностей.
        /// </summary>
        public int EventEntityPoolCapacity { get; set; } = 50;

    }

    public sealed class LogServiceClientDeviceOptions {
        public string Model { get; set; }
        public string Name { get; set; }
        public string OperatingSystem { get; set; }
        public string OperatingSystemFamily { get; set; }
        public string ProcessorType { get; set; }
    }
}
