namespace LogServiceClient.Runtime.Caches.Interfaces {
    public interface ILogIdProvider {
        string Get(string condition, string stackTrace);
    }
}
