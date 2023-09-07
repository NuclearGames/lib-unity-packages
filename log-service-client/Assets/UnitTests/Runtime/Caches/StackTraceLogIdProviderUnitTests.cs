using LogServiceClient.Runtime.Caches;
using NUnit.Framework;

namespace UnitTests.Runtime.Caches {
    internal class StackTraceLogIdProviderUnitTests {
        [TestCaseSource(nameof(Get_ReturnsExpectedId_Cases))]
        public void Get_ReturnsExpectedId(string expectedId, string stackTrace) {
            var provider = new StackTraceLogIdProvider();

            string id = provider.Get("condition", stackTrace);

            Assert.That(id, Is.EqualTo(expectedId));
        }

        private static readonly object[] Get_ReturnsExpectedId_Cases = new object[] { 
            new object[] {
                "UnityEngine.Debug:Log|Sandbox.TestLogGenerator/<LogSequence>d__6:MoveNext",
                @"UnityEngine.Debug:Log (object)
Sandbox.TestLogGenerator/<LogSequence>d__6:MoveNext () (at Assets/Sandbox/TestLogGenerator.cs:40)
System.Runtime.CompilerServices.AsyncVoidMethodBuilder:Start<Sandbox.TestLogGenerator/<LogSequence>d__6> (Sandbox.TestLogGenerator/<LogSequence>d__6&)
Sandbox.TestLogGenerator:LogSequence ()
"
            },

            new object[] {
                "UnityEngine.Debug:LogError|Sandbox.TestLogGenerator/<LogSequence>d__6:MoveNext", 
                @"UnityEngine.Debug:LogError (object)
Sandbox.TestLogGenerator/<LogSequence>d__6:MoveNext () (at Assets/Sandbox/TestLogGenerator.cs:38)
System.Runtime.CompilerServices.AsyncVoidMethodBuilder:Start<Sandbox.TestLogGenerator/<LogSequence>d__6> (Sandbox.TestLogGenerator/<LogSequence>d__6&)
Sandbox.TestLogGenerator:LogSequence ()"
            },

            new object[] {
                "BattleScene.SceneMangers.Spectate.SpectateManager:Enable",
                @"BattleScene.SceneMangers.Spectate.SpectateManager:Enable(Int32)
BattleScene.SceneMangers.Spectate.SpectateManager:Enable(SpectateType, Int32)
BattleScene.SceneMangers.Spectate.RespawnKillerSpectate:TryStartSpectate(Boolean)
BattleScene.SceneMangers.BattleManagement.RespawnLoops.AirSupermacyRespawnLoopRunner:EnableKillerSpectate(RespawnLoopContext, CancellationToken)
BattleScene.SceneMangers.BattleManagement.RespawnLoops.<Call>d__5:MoveNext()
Cysharp.Threading.Tasks.UniTaskCompletionSourceCore`1:TrySetResult(TResult)
BattleScene.SceneMangers.BattleManagement.RespawnLoops.<DisableCharacters>d__11:MoveNext()
Cysharp.Threading.Tasks.UniTaskCompletionSourceCore`1:TrySetResult(TResult)
Cysharp.Threading.Tasks.WaitUntilPromise:MoveNext()
Cysharp.Threading.Tasks.Internal.PlayerLoopRunner:RunCore()"
            },

            new object[] {
                "UnityEngine.UI.Graphic.SetVerticesDirty|MenuScene.UI.Hangar.Lobby.LobbyWindow.UpdateInfo",
                @"UnityEngine.UI.Graphic.SetVerticesDirty () (at <00000000000000000000000000000000>:0)
UnityEngine.UI.Text.set_text (System.String value) (at <00000000000000000000000000000000>:0)
MenuScene.UI.Hangar.Lobby.LobbyWindow.UpdateInfo () (at <00000000000000000000000000000000>:0)
MenuScene.UI.Hangar.Lobby.LobbyWindow.InitializeAsync () (at <00000000000000000000000000000000>:0)
Cysharp.Threading.Tasks.UniTaskCompletionSourceCore`1[TResult].TrySetResult (TResult result) (at <00000000000000000000000000000000>:0)
Utilities.Extensions.FirebaseFieldExtensions.AwaitFullInitialized[T] (T rdbSource, Utilities.Utils.CancellationTokenSourceWrapper ctsw) (at <00000000000000000000000000000000>:0)
Cysharp.Threading.Tasks.UniTaskCompletionSourceCore`1[TResult].TrySetResult (TResult result) (at <00000000000000000000000000000000>:0)
Cysharp.Threading.Tasks.UniTask+WaitUntilPromise.MoveNext () (at <00000000000000000000000000000000>:0)
Cysharp.Threading.Tasks.Internal.PlayerLoopRunner.RunCore () (at <00000000000000000000000000000000>:0)
MenuScene.UI.Hangar.Lobby.<InitializeAsync>d__16:MoveNext()
Cysharp.Threading.Tasks.UniTaskCompletionSourceCore`1:TrySetResult(TResult)
Utilities.Extensions.<AwaitFullInitialized>d__0`1:MoveNext()
Cysharp.Threading.Tasks.UniTaskCompletionSourceCore`1:TrySetResult(TResult)
Cysharp.Threading.Tasks.WaitUntilPromise:MoveNext()
Cysharp.Threading.Tasks.Internal.PlayerLoopRunner:RunCore()
"
            },

            new object[]{
                "BattleScene.Airdrome.AirdromeController:set_OwnerTeamId",
                @"BattleScene.Airdrome.AirdromeController:set_OwnerTeamId(SByte)
BattleScene.Airdrome.AirdromeController:Deserialize(ReferencedArraySegment`1&, PhotonMessageInfo&)
Photon.Pun.PhotonView:TryDeserializeComponent(ReferencedArraySegment`1&, PhotonMessageInfo&, Boolean)
Photon.Pun.PhotonView:DeserializeView(Byte[]&, PhotonMessageInfo&)
Photon.Pun.PhotonNetwork:OnSerializeRead(Object[], Player, Int32, Int16)
Photon.Pun.PhotonNetwork:OnEvent(EventData)
Photon.Realtime.LoadBalancingClient:OnEvent(EventData)
ExitGames.Client.Photon.PeerBase:DeserializeMessageAndCallback(StreamBuffer)
ExitGames.Client.Photon.EnetPeer:DispatchIncomingCommands()
ExitGames.Client.Photon.PhotonPeer:DispatchIncomingCommands()
Photon.Pun.PhotonHandler:Dispatch()"
            }
        };
    }
}
