using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sandbox {
    internal class TestLogGenerator : MonoBehaviour {
        [Header("Log")]
        [SerializeField] private LogType type;
        [SerializeField] private string text;

        [ContextMenu("log")]
        public void Log() {
            switch (type) {
                case LogType.Log:
                    Debug.Log(text);
                    break;

                case LogType.Error:
                    Debug.LogError(text); 
                    break;

                case LogType.Warning:
                    Debug.LogWarning(text);
                    break;
            }
        }

        [Header("Sequence")]
        [SerializeField] private int sequenceLength = 70;
        [SerializeField] private int sequenceErrorCount = 3;
        [SerializeField] private int sequenceDelayMs = 100;

        [ContextMenu("Log Sequence")]
        public async void LogSequence() {
            int errorPeriod = sequenceLength / sequenceErrorCount;

            for(int i = 1; i <= sequenceLength; i++) {
                if(i % errorPeriod == 0) {
                    Debug.LogError($"Error : {i}");
                } else {
                    Debug.Log($"Log : {i}");
                }

                if (sequenceDelayMs > 0) {
                    await UniTask.Delay(sequenceDelayMs);
                }
            }
        }
    }
}
