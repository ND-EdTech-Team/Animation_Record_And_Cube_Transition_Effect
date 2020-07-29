using System.IO;
using UnityEngine;

namespace Plugins.Cube.Persist
{
    public class DataReader
    {
        private readonly BinaryReader _reader;

        public DataReader(BinaryReader reader)
        {
            _reader = reader;
        }

        public float ReadFloat()
        {
            return _reader.ReadSingle();
        }

        public int ReadInt()
        {
            return _reader.ReadInt32();
        }

        public Quaternion ReadQuaternion()
        {
            Quaternion result;
            result.x = _reader.ReadSingle();
            result.y = _reader.ReadSingle();
            result.z = _reader.ReadSingle();
            result.w = _reader.ReadSingle();
            return result;
        }

        public Vector3 ReadVector3()
        {
            Vector3 result;
            result.x = _reader.ReadSingle();
            result.y = _reader.ReadSingle();
            result.z = _reader.ReadSingle();
            return result;
        }
    }
}