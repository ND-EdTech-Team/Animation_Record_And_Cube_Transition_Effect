using System.Collections.Generic;
using System.IO;
using System.Linq;
using Plugins.Cube.Persist;
using Plugins.Cube.Physic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Plugins.Cube.Record
{
    /// <summary>
    ///     大立方体动画
    /// </summary>
    [DisallowMultipleComponent]
    public class CompositeCubeAnimationDirector : MonoBehaviour
    {
        public string fileName;
        public Transform prefabCube;
        public Vector3 cubeSize = Vector3.one;
        public float spacing = 0.1f;
        public int xCount = 5;
        public int yCount = 3;
        public int zCount = 3;
        [SerializeField]
        private BaseForce[] forceAdder = new BaseForce[0];

        public UnityEvent onRecordBegin;
        public UnityEvent onRecordFinished;

        private readonly List<Transform> _cubes = new List<Transform>();
        private KeyFrameCollector[] _keyFrameCollectors;
        private bool _recording;
        private float _recordStartTime;
        private float _totalTime;

        public void Record()
        {
            ClearData();
            _recordStartTime = Time.time;
            _recording = true;
            onRecordBegin?.Invoke();
            AddForce();
        }

        private void AddForce()
        {
            foreach (var force in forceAdder)
                force.AddForce();
        }

        private void Awake()
        {
            Spawn();
            InitializeForceAdder();
        }

        #region spawn cubes

        private void Spawn()
        {
            DestroyCubes();
            CreateCubes();
            _keyFrameCollectors = GetComponentsInChildren<KeyFrameCollector>();
        }

        private void DestroyCubes()
        {
            foreach (var cube in _cubes.Where(cube => cube != null))
                DestroyCube(cube.gameObject);
        }

        private static void DestroyCube(GameObject o)
        {
            if (Application.isPlaying)
                Destroy(o);
            else
                DestroyImmediate(o);
        }

        private void CreateCubes()
        {
            _cubes.Clear();
            for (var z = 0; z < zCount; z++)
            for (var y = 0; y < yCount; y++)
            for (var x = 0; x < xCount; x++)
            {
                var cube = Instantiate(prefabCube, transform, false);
                cube.transform.localPosition = GetPosition(x, y, z);
                cube.gameObject.AddComponent<KeyFrameCollector>();
                _cubes.Add(cube);
            }
        }
        
        private Vector3 GetPosition(int x, int y, int z)
        {
            return new Vector3(GetPosition(x, xCount, 1, spacing),
                GetPosition(y, yCount, 1, spacing),
                GetPosition(z, zCount, 1, spacing));
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
        
        #region record

        private void ClearData()
        {
            foreach (var recorder in _keyFrameCollectors)
                recorder.Clear();
        }
        
        private void InitializeForceAdder()
        {
            var cubes = _cubes.Select(cube => cube.gameObject).ToArray();
            foreach (var force in forceAdder)
                force.Initialize(cubes);
        }

        private void FixedUpdate()
        {
            Recording();
        }

        private void Recording()
        {
            if (!_recording)
                return;

            var time = Time.time - _recordStartTime;
            if (TryCollect(time))
                _totalTime = time;
            else
            {
                _recording = false;
                OnRecordFinished();
            }
        }

        private bool TryCollect(float time)
        {
            var changed = false;
            foreach (var recorder in _keyFrameCollectors)
                if (recorder.TryCollect(time))
                    changed = true;
            return changed;
        }

        private void OnRecordFinished()
        {
            Save();
            onRecordFinished?.Invoke();
            Debug.Log($"{name} total time {_totalTime}");
        }

        #endregion

        #region storage

        private void Save()
        {
            var filePath = new StoragePath().GetPath(fileName);
            using (var writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                Save(new DataWriter(writer));
            }
        }

        private void Save(DataWriter writer)
        {
            writer.Write(xCount);
            writer.Write(yCount);
            writer.Write(zCount);
            writer.Write(cubeSize);
            writer.Write(spacing);
            writer.Write(_totalTime);
            foreach (var frameCollector in _keyFrameCollectors)
            {
                var frames = frameCollector.track.frames;
                writer.Write(frames.Count);
                foreach (var frame in frames)
                {
                    writer.Write(frame.time);   
                    writer.Write(frame.position);   
                    writer.Write(frame.rotation);   
                }
            }
        }

        #endregion
    }
}