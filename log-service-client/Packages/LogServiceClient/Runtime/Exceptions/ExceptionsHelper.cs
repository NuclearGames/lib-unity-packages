using System;

namespace LogServiceClient.Runtime.Exceptions {
    internal static class ExceptionsHelper {
        internal static void ThrowInvalidOperationException(string message = null) {
            throw new InvalidOperationException(message);
        }
    }
}
