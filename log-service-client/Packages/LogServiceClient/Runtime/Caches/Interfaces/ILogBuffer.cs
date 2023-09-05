using LogServiceClient.Runtime.Mappers.Interfaces;

namespace LogServiceClient.Runtime.Caches.Interfaces {
    public interface ILogBuffer {
        /// <summary>
        /// Число элементов в буфере.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Перемещает данные первого элемента в указанный объект используя маппер.
        /// Исходный объект удаляется из буфера.
        /// </summary>
        void MoveFirst<T>(T target, ILogMapper<LogEntry, T> mapper);
    }
}
