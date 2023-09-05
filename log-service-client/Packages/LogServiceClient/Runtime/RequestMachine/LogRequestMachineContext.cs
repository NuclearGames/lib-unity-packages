namespace LogServiceClient.Runtime.RequestMachine {
    public sealed class LogRequestMachineContext {
        public string DatabaseId { get; }
        public string DeviceId { get; private set; }
        public string SessionId { get; private set; }
        public string ReportId { get; private set; }
    }
}
