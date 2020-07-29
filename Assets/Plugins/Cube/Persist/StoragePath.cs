using System.IO;
using UnityEngine;

namespace Plugins.Cube.Persist
{
    internal class StoragePath : IStoragePath
    {
        public string GetPath(string savePath)
        {
            return Path.Combine(Application.streamingAssetsPath, savePath);
        }
    }
}