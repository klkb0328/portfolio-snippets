using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common.Messaging
{
    /// <summary>
    /// LateUpdate마다 MessageSystem.Flush()를 호출하여
    /// 구독, 해제, 퍼블리시 순서를 일괄 처리한다.
    /// DontDestroyOnLoad로 앱 수명 동안 유지.
    /// </summary>
    internal class MessageSystemRunner : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            var runnerGo = new GameObject("MessageSystemRunner");
            runnerGo.AddComponent<MessageSystemRunner>();
            DontDestroyOnLoad(runnerGo);
        }

        private void Awake()
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void LateUpdate()
        {
            MessageSystem.Instance.Flush();
        }

        private void OnSceneUnloaded(Scene current)
        {
            ClearAll();
        }

        private void OnDestroy()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            ClearAll();
        }

        private void ClearAll()
        {
            MessageSystem.Instance.ClearAllQueues();
            MessagePool.ClearAllPools();
        }
    }
}
