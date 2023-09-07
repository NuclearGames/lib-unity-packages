using System;
using UnityEngine;

namespace LogServiceClient.Runtime.Enums {
    [Flags]
    public enum LogTypeFlags {
        Error = 1 << 0,
        Assert = 1 << 1,
        Warning = 1 << 2,
        Log = 1 << 3,
        Exception = 1 << 4,

        Nothing = 0,
        All = Error | Assert | Warning | Log | Exception
    }

    public static class LogTypeFlagsExtensions {
        public static bool HasType(this LogTypeFlags mask, LogType type) {
            var typeFlag = GetFlagsFromLogType(type);
            return mask.HasFlag(typeFlag);
        }

        private static LogTypeFlags GetFlagsFromLogType(LogType type) {
            return type switch {
                LogType.Error => LogTypeFlags.Error,
                LogType.Assert => LogTypeFlags.Assert,
                LogType.Warning => LogTypeFlags.Warning,
                LogType.Log => LogTypeFlags.Log,
                LogType.Exception => LogTypeFlags.Exception,
                _ => LogTypeFlags.Nothing
            };
        }
    }
}
