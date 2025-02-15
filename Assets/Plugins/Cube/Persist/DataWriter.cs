﻿using System.IO;
using UnityEngine;

namespace Plugins.Cube.Persist
{
    public class DataWriter
    {
        private readonly BinaryWriter _writer;

        public DataWriter(BinaryWriter writer)
        {
            _writer = writer;
        }

        public void Write(float value)
        {
            _writer.Write(value);
        }

        public void Write(int value)
        {
            _writer.Write(value);
        }

        public void Write(Quaternion value)
        {
            _writer.Write(value.x);
            _writer.Write(value.y);
            _writer.Write(value.z);
            _writer.Write(value.w);
        }

        public void Write(Vector3 value)
        {
            _writer.Write(value.x);
            _writer.Write(value.y);
            _writer.Write(value.z);
        }
    }
}