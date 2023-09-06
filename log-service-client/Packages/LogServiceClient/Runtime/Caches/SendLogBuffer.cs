﻿using LogServiceClient.Runtime.Caches.Interfaces;
using LogServiceClient.Runtime.Caches.Utils;
using LogServiceClient.Runtime.Mappers.Interfaces;
using LogServiceClient.Runtime.Pools.Interfaces;

namespace LogServiceClient.Runtime.Caches {
    public sealed class SendLogBuffer : BaseLogBuffer<SendLogEntry>, ISendLogBuffer {
        public SendLogBuffer(ILogPool<SendLogEntry> pool) : base(pool) {
        }

        public void MoveAllFrom<TSource>(ILogBuffer<TSource> source, ILogMapper<TSource, SendLogEntry> mapper) 
            where TSource : BaseLogEntry<TSource> {

            while(source.Count > 0) {
                var entry = GetFromPool();
                source.MoveFirst(entry, mapper);
                entry.Index = Last.Index + 1;

                AddLast(entry);
            }
        }
    }
}