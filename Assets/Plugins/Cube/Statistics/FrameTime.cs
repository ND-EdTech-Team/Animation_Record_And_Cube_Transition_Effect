using UnityEngine;

namespace Plugins.Cube.Statistics
{
    public class FrameTime : MonoBehaviour
    {
        private float _frameTime;
        
        private void Awake()
        {
            _frameTime = Time.time;
        }

        private void LateUpdate()
        {
            var elapsedMilliseconds = (Time.time - _frameTime) * 1000;
            Debug.LogError($"帧耗时: {elapsedMilliseconds} ms");
            _frameTime = Time.time;
        }
    }
}