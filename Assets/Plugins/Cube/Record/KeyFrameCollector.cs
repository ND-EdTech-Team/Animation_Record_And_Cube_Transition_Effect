using Plugins.Cube.Data;
using UnityEngine;

namespace Plugins.Cube.Record
{
    public class KeyFrameCollector : MonoBehaviour
    {
        public FrameTrack track = new FrameTrack();

        public void Clear()
        {
            track.Clear();
        }

        public bool TryCollect(float time)
        {
            var key = new KeyFrame {time = time};
            OnCollect(ref key);

            if (track.Count == 0 || !track.GetFrame(track.Count - 1).Equals(key))
            {
                track.Add(key);
                return true;
            }

            return false;

        }

        public void Sample(float time)
        {
            if (track.TryGet(time, out var frameData))
                OnSample(frameData);
        }

        private void OnCollect(ref KeyFrame keyFrame)
        {
            var t = transform;
            keyFrame.position = t.localPosition;
            keyFrame.rotation = t.localRotation;
        }

        private void OnSample(KeyFrame keyFrame)
        {
            var t = transform;
            t.localPosition = keyFrame.position;
            t.localRotation = keyFrame.rotation;
        }
    }
}