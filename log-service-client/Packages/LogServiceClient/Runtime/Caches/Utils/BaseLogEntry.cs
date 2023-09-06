namespace LogServiceClient.Runtime.Caches.Utils {
    public abstract class BaseLogEntry<TSelf> where TSelf : BaseLogEntry<TSelf> {
        public TSelf Next { get; set; }

        public virtual void Reset() {
            Next = null;
        }
    }
}
