namespace LogServiceClient.Runtime.Pools.Interfaces {
    public interface ILogPool<T> where T : ILogPoolItem {
        T Get();
        void Return(T entry);
    }
}
