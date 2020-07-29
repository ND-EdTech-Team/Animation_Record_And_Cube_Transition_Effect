using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Plugins.Cube.Statistics
{
    /// <summary>
    /// 异步加载场景
    /// </summary>
    public class LoadSceneAsync : MonoBehaviour
    {
        public UnityEvent onLoadFinished;
        
        public string sceneName;

        private AsyncOperation _asyncOperation;
        private readonly Stopwatch _sw = new Stopwatch();

        private void Awake()
        {
            _sw.Start();
            _asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            StartCoroutine(TimeTest());
        }

        private IEnumerator TimeTest()
        {
            while (!_asyncOperation.isDone)
                yield return null;
            
            _sw.Stop();
            onLoadFinished.Invoke();
            //Debug.Log($"加载场景时间: {_sw.ElapsedMilliseconds} ms");
        }
    }
}