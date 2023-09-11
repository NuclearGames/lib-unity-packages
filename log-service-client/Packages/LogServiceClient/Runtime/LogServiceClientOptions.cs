using LogServiceClient.Runtime.Caches;
using LogServiceClient.Runtime.Caches.Interfaces;
using LogServiceClient.Runtime.Enums;
using LogServiceClient.Runtime.External.Interfaces;

namespace LogServiceClient.Runtime {
    public sealed class LogServiceClientOptions {
        /// <summary>
        /// Настройки для валидации содержимого.
        /// </summary>
        public LogServiceClientValidationOptions ValidationOptions { get; set; }

        /// <summary>
        /// Адрес сервиса. 
        /// (http(s)://ip:port)
        /// </summary>
        public string ServiceAddress { get; set; }

        /// <summary>
        /// Идентификатор бд.
        /// </summary>
        public string DbId { get; set; }

        /// <summary>
        /// Id устройства.
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// Информация о устройстве.
        /// </summary>
        public LogServiceClientDeviceOptions DeviceOptions { get; set; }


        /// <summary>
        /// Объект, возвращающий идентификатор лога.
        /// 
        /// <para>
        /// <see cref="ConditionLogIdProvider" /> - возвращает текст лога.
        /// </para>
        /// 
        /// <para>
        ///  <see cref="StackTraceLogIdProvider" /> - создает ключ из стактрейса.
        /// </para>
        /// </summary>
        public ILogIdProvider LogIdProvider { get; set; }

        /// <summary>
        /// Объект, предоставляющий инфу о настройках.
        /// <para>Должен быть реализован.</para>
        /// </summary>
        public IUserSettingsProvider UserSettingsProvider { get; set; }

        /// <summary>
        /// Нужно ли собирать стактрейсы.
        /// </summary>
        public bool CaptureStackTrace { get; set; } = true;

        /// <summary>
        /// Собираемые типы логов.
        /// </summary>
        public LogTypeFlags CaptureLogTypes { get; set; } = LogTypeFlags.All;

        /// <summary>
        /// Типы логов, которые инициируют отправку.
        /// </summary>
        public LogTypeFlags StartSendLogTypes { get; set; } = LogTypeFlags.Error | LogTypeFlags.Assert | LogTypeFlags.Exception;

        /// <summary>
        /// Глубина стактрейса.
        /// Все, что не влезло, выбрасывается.
        /// </summary>
        public int StackTraceDeep { get; set; } = 10;


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
        public int SendBufferPoolMaxCapacity { get; set; } = 80;

        /// <summary>
        /// Размера пула отправляемых сущностей.
        /// </summary>
        public int EventEntityPoolCapacity { get; set; } = 50;

    }

    public sealed class LogServiceClientValidationOptions {
        public int DeviceIdMaxLength { get; set; } = 128;
        public int DeviceModelMaxLength { get; set; } = 50;
        public int DeviceNameMaxLength { get; set; } = 50;
        public int DeviceOperatingSystemMaxLength { get; set; } = 30;
        public int DeviceOperatingSystemFamilyMaxLength { get; set; } = 30;
        public int DeviceProcessorTypeMaxLength { get; set; } = 30;
    }

    public sealed class LogServiceClientDeviceOptions {
        public string Model { get; set; }
        public string Name { get; set; }
        public string OperatingSystem { get; set; }
        public string OperatingSystemFamily { get; set; }
        public string ProcessorType { get; set; }
    }
}
