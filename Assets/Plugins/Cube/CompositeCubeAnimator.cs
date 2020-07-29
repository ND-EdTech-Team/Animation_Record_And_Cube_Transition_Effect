using System;
using System.Collections;
using System.IO;
using Plugins.Cube.Data;
using Plugins.Cube.Persist;
using Plugins.Cube.Record;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Plugins.Cube
{
    [ExecuteInEditMode]
    public class CompositeCubeAnimator : MonoBehaviour
    {
        [SerializeField] private string filePath;
        public Transform prefabCube;
        public float speed = 1f;
        public float totalTime;
        public float sampleTime;
        
        [NonSerialized]
        private KeyFrameCollector[] _keyFrameCollectors = new KeyFrameCollector[0];

        private void OnEnable()
        {
            if (!Application.isPlaying)
                Load();
        }

        private void Start()
        {
            Load();
        }

        #region storage

        public void Load(string filePath)
        {
            this.filePath = filePath;
            filePath = GetFinalFilePath(filePath);
            try
            {
                using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
                    Load(new DataReader(reader));
                }
            }
            catch (Exception e)
            {
                Debug.Log($"加载失败：{e.Message}");
            }
        }

        private static string GetFinalFilePath(string filePath)
        {
            if (!Path.IsPathRooted(filePath))
                filePath = new StoragePath().GetPath(filePath);
            return filePath;
        }

        private void Load()
        {
            Load(filePath);
        }

        private void Load(DataReader reader)
        {
            var xCount = reader.ReadInt();
            var yCount = reader.ReadInt();
            var zCount = reader.ReadInt();
            var cubeSize = reader.ReadVector3();
            var spacing = reader.ReadFloat();
            Spawn(xCount, yCount, zCount, cubeSize, spacing);

            totalTime = reader.ReadFloat();
            foreach (var collector in _keyFrameCollectors)
                collector.track = ReadTrack(reader);
        }

        private static FrameTrack ReadTrack(DataReader reader)
        {
            var result = new FrameTrack();
            var frameCount = reader.ReadInt();
            for (var i = 0; i < frameCount; i++)
                result.Add(ReadFrame(reader));
            return result;
        }

        private static KeyFrame ReadFrame(DataReader reader)
        {
            var time = reader.ReadFloat();
            var position = reader.ReadVector3();
            var rotation = reader.ReadQuaternion();
            return new KeyFrame(time, position, rotation);
        }

        #endregion
        
        #region spawn cubes

        private void Spawn(int xCount, int yCount, int zCount, Vector3 cubeSize, float spacing)
        {
            DestroyCubes();
            CreateCubes(xCount, yCount, zCount, cubeSize, spacing);
        }

        private void DestroyCubes()
        {
            var collectors = GetComponentsInChildren<KeyFrameCollector>();
            foreach (var collector in collectors)
                DestroyCube(collector.gameObject);
        }

        private static void DestroyCube(Object o)
        {
            if (Application.isPlaying)
                Destroy(o);
            else
                DestroyImmediate(o);
        }

        private void CreateCubes(int xCount, int yCount, int zCount, Vector3 cubeSize, float spacing)
        {
            for (var z = 0; z < zCount; z++)
            for (var y = 0; y < yCount; y++)
            for (var x = 0; x < xCount; x++)
            {
                var cube = Instantiate(prefabCube, transform, false);
                cube.gameObject.hideFlags = HideFlags.DontSave;
                cube.transform.localPosition = new Vector3(GetPosition(x, xCount, cubeSize.x, spacing),
                    GetPosition(y, yCount, cubeSize.y, spacing),
                    GetPosition(z, zCount, cubeSize.z, spacing));
                cube.gameObject.AddComponent<KeyFrameCollector>();
            }
            _keyFrameCollectors = GetComponentsInChildren<KeyFrameCollector>();
        }

        private static float GetPosition(int index, int count, float cubeSize, float spacing)
        {
            var totalLength = count * cubeSize;
            if (count > 1)
                totalLength += (count - 1) * spacing;

            var s = index * (cubeSize + spacing);
            return s - totalLength / 2 + 0.5f;
        }

        #endregion

        #region Play

        private void LateUpdate()
        {
            Sample(sampleTime);
        }

        [ContextMenu("Play Forward")]
        public void PlayForward()
        {
            StartCoroutine(Playing(true));
        }

        [ContextMenu("Play Backward")]
        public void PlayBackward()
        {
            StartCoroutine(Playing(false));
        }

        private IEnumerator Playing(bool forward)
        {
            var startTime = Time.time;
            while (true)
            {
                var time = Time.time - startTime;
                time *= speed;
                if (time > totalTime)
                    time = totalTime;

                var normalizeTime = time / totalTime;
                var sTime = Mathf.Lerp(forward ? 0 : totalTime, forward ? totalTime : 0, normalizeTime);
                sampleTime = sTime;

                yield return null;
                if (time >= totalTime)
                    yield break;
            }
        }

        private void Sample(float time)
        {
            foreach (var recorder in _keyFrameCollectors)
                recorder.Sample(time);
        }

        #endregion
    }
}