using System;
using System.Collections.Generic;

namespace Plugins.Cube.Data
{
    [Serializable]
    public class FrameTrack
    {
        public List<KeyFrame> frames = new List<KeyFrame>();

        public int Count => frames.Count;

        public KeyFrame GetFrame(int index) => frames[index];

        public void Clear()
        {
            frames.Clear();
        }

        public void Add(KeyFrame keyFrame)
        {
            if (frames.Count == 0)
            {
                frames.Add(keyFrame);
                return;
            }

            var lastFrameData = frames[frames.Count - 1];
            if (lastFrameData.time >= keyFrame.time)
                throw new ArgumentOutOfRangeException(
                    $"Added frame time {keyFrame.time} should greater than recently time {lastFrameData.time}");

            frames.Add(keyFrame);
        }

        public bool TryGet(float time, out KeyFrame keyFrame)
        {
            keyFrame = new KeyFrame();

            if (frames.Count == 0)
                return false;

            var first = frames[0];
            if (first.time > time)
            {
                keyFrame = first;
                return true;
            }

            var last = frames[frames.Count - 1];
            if (last.time < time)
            {
                keyFrame = last;
                return true;
            }

            for (var i = 0; i < frames.Count - 1; i++)
            {
                var curFrame = frames[i];
                var nxtFrame = frames[i + 1];
                if (curFrame.time <= time && time <= nxtFrame.time)
                {
                    var t = (time - curFrame.time) / (nxtFrame.time - curFrame.time);
                    keyFrame = curFrame.Interpolate(nxtFrame, t);
                    return true;
                }
            }

            return false;
        }
    }
}