using Cysharp.Threading.Tasks;
using System;

namespace UnitTests {
    internal static class AsyncTest {
        internal static void Run(Func<UniTask> func) {
            func().GetAwaiter().GetResult();
        }
    }
}
