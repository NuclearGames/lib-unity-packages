using System;
using UnityEngine;

namespace LogServiceClient.Runtime.Caches.Interfaces {
    public interface IReceiveLogBuffer : ILogBuffer<ReceiveLogEntry> {
        void StoreEntry(string condition, string stackTrace, LogType logType, DateTime time);
    }
}
