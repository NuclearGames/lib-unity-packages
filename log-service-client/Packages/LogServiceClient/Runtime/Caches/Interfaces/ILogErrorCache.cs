namespace LogServiceClient.Runtime.Caches.Interfaces {
    public interface ILogErrorCache {
        /// <summary>
        /// Добавляет идентификатор ошибки в кэш.
        /// </summary>
        /// <returns>Была ли такой идентификатор в кэше до добавления</returns>
        bool Push(string id);
    }
}
