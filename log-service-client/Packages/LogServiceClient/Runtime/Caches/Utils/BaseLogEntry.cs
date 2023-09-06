using LogServiceClient.Runtime.Pools.Interfaces;

namespace LogServiceClient.Runtime.Caches.Utils {
    public abstract class BaseLogEntry<TSelf> : ILogPoolItem where TSelf : BaseLogEntry<TSelf> {
        public TSelf Next { get; set; }

        public virtual void Reset() {
            Next = null;
        }
    }
}
