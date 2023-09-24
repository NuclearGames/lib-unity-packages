namespace LogServiceClient.Runtime.Mappers.Interfaces {
    public interface ILogMapper<TFrom, TTo> {
        void Copy(TFrom from, TTo to);

        TTo Map(TFrom from);
    }
}
