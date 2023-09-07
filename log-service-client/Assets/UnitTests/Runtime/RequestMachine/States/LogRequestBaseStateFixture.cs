using Cysharp.Threading.Tasks;
using LogServiceClient.Runtime.RequestMachine.Enums;
using LogServiceClient.Runtime.RequestMachine.Interfaces;
using LogServiceClient.Runtime.RequestMachine.Utils;
using LogServiceClient.Runtime;
using NSubstitute;
using System.Threading;
using System;
using NUnit.Framework;

namespace UnitTests.Runtime.RequestMachine.States {
    internal abstract class LogRequestBaseStateFixture<T> where T : LogRequestBaseStateFixture<T> {
        internal LogServiceClientOptions Options { get; }
        internal ILogRequestMachineInternal Machine { get; }

        public LogRequestBaseStateFixture() {
            Options = new LogServiceClientOptions();

            Machine = Substitute.For<ILogRequestMachineInternal>();
            Machine.Context.Delay(Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(UniTask.CompletedTask);
            Machine.Options.Returns(Options);
            Machine.Variables.Returns(new LogRequestMachineVariables());
        }

        internal T SetupOptions(Action<LogServiceClientOptions> action) {
            action(Options);
            return this as T;
        }

        internal T Build() {
            return this as T;
        }

        internal async UniTask AssertReturnsRetry(LogRequestStateResult result, LogRequestStateIndex index) {
            Assert.That(result.Index, Is.EqualTo(index));
            await Machine.Context.Received().Delay(Arg.Any<int>(), Arg.Any<CancellationToken>());
        }

        internal void AssertReturnsExit(LogRequestStateResult result) {
            Assert.That(result.Index, Is.EqualTo(LogRequestStateIndex.None));
        }

        internal void AssertReturnsMoveTo(LogRequestStateResult result, LogRequestStateIndex index) {
            Assert.That(result.Index, Is.EqualTo(index));
        }
    }
}
