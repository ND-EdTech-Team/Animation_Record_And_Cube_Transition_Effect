using System;
using UnityEngine;

namespace Plugins.Cube.Data
{
    [Serializable]
    public struct KeyFrame
    {
        public float time;
        public Vector3 position;
        public Quaternion rotation;

        public KeyFrame(float time, Vector3 position, Quaternion rotation)
        {
            this.time = time;
            this.position = position;
            this.rotation = rotation;
        }

        private bool Equals(KeyFrame other)
        {
            return position.Equals(other.position) && rotation.Equals(other.rotation);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((KeyFrame) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (position.GetHashCode() * 397) ^ rotation.GetHashCode();
            }
        }

        public KeyFrame Interpolate(KeyFrame to, float t)
        {
            var time = Mathf.LerpUnclamped(this.time, to.time, t);
            var pos = Vector3.LerpUnclamped(position, to.position, t);
            var rot = Quaternion.SlerpUnclamped(rotation, to.rotation, t);
            var result = new KeyFrame {time = time, position = pos, rotation = rot};
            return result;
        }
    }
}