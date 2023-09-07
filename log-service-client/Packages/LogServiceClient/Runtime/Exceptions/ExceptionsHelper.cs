using System;

namespace LogServiceClient.Runtime.Exceptions {
    internal static class ExceptionsHelper {
        internal static void ThrowInvalidOperationException(string message = null) {
            throw new InvalidOperationException(message);
        }

        internal static void ThrowNotImplementedException(string message = null) {
            throw new NotImplementedException(message);
        }
    }
}
