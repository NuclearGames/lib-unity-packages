using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Mappers.Interfaces;

namespace LogServiceClient.Runtime.Caches.Interfaces {
    public interface ILogBuffer<T> where T : BaseLogEntry<T> {
        /// <summary>
        /// Число элементов в буфере.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Перемещает данные первого элемента в указанный объект используя маппер.
        /// Исходный объект удаляется из буфера.
        /// </summary>
        void MoveFirst<TTarget>(TTarget target, ILogMapper<T, TTarget> mapper);

        /// <summary>
        /// Очищает буфер.
        /// </summary>
        void Clear();
    }
}
