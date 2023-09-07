using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Mappers.Interfaces;

namespace LogServiceClient.Runtime.Caches.Interfaces {
    public interface ISendLogBuffer : ILogBuffer<SendLogEntry> {
        void MoveAllFrom<TSource>(ILogBuffer<TSource> source, ILogMapper<TSource, SendLogEntry> mapper) where TSource : BaseLogEntry<TSource>;
    }
}
