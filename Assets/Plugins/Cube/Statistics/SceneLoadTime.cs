using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Plugins.Cube.Statistics
{
    public static class SceneLoadTime
    {
        private static readonly Stopwatch Sw = new Stopwatch();
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            Sw.Start();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnAfterSceneLoad()
        {            
            Sw.Stop();
            UnityEngine.Debug.Log($"加载场景 {SceneManager.GetActiveScene ().name} 耗时：{Sw.ElapsedMilliseconds} ms");
        }
    }
}