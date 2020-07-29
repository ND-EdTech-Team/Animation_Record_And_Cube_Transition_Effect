using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace Plugins.Cube.Statistics
{
    /// <summary>
    /// 异步加载场景
    /// </summary>
    public class LoadSceneSync : MonoBehaviour
    {
        public string sceneName;

        private readonly Stopwatch _sw = new Stopwatch();

        private void Awake()
        {
            _sw.Start();
            SceneManager.LoadScene(sceneName);
            _sw.Stop();
            Debug.Log($"切换场景 {sceneName} 耗时:{_sw.ElapsedMilliseconds} ms");
        }
    }
}