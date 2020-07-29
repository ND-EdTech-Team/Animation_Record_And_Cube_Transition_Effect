using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Plugins.Cube.Camera
{
    /// <summary>
    /// 摄像机控制器
    /// </summary>
    internal class CameraController : MonoBehaviour
    {
        public float moveSpeedMinZoom = default;
        public float moveSpeedMaxZoom = default;
        
        public float swivelMinZoom = default;
        public float swivelMaxZoom = default;
        
        public float stickMinZoom = default;
        public float stickMaxZoom = default;

        public float rotationSpeed = default;
        
        private float _rotationAngle;
        private float _rotationXAngle;
        private Transform _swivel;
        private Transform _stick;
        private float _zoom = 1f;

        private void Awake()
        {
            _swivel = transform.GetChild(0);
            _stick = _swivel.GetChild(0);

            _rotationAngle = transform.localEulerAngles.y;
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        private void Update()
        {
            var zoomDelta = Input.GetAxis("Mouse ScrollWheel");
            if (zoomDelta != 0f) 
                AdjustZoom(zoomDelta);

            var rotationDelta = InputManager.GetMouse_X(); ;
            if (rotationDelta != 0f) 
                AdjustRotation(rotationDelta);
        
            var rotationYDelta = InputManager.GetMouse_Y(); ;
            if (rotationYDelta != 0f) 
                AdjustYRotation(rotationYDelta);
        
            transform.localRotation = Quaternion.Euler(_rotationXAngle, _rotationAngle, 0f);

            var xDelta = Input.GetAxis("Horizontal");
            var zDelta = Input.GetAxis("Vertical");
            if (xDelta != 0f || zDelta != 0f) 
                AdjustPosition(xDelta, zDelta);
        }

        private void AdjustZoom(float delta)
        {
            _zoom = Mathf.Clamp01(_zoom + delta);

            var distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, _zoom);
            _stick.localPosition = new Vector3(0f, 0f, distance);

            var angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom, _zoom);
            _swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
        }

        private void AdjustRotation(float delta)
        {
            _rotationAngle += delta * rotationSpeed * Time.deltaTime;
            if (_rotationAngle < 0f)
                _rotationAngle += 360f;
            else if (_rotationAngle >= 360f) _rotationAngle -= 360f;
        
        }

        private void AdjustYRotation(float delta)
        {
            _rotationXAngle += -delta * rotationSpeed * Time.deltaTime * 10f;
            if (_rotationXAngle <= -90f)
                _rotationXAngle = -90f;
            else if (_rotationXAngle >= 90) _rotationAngle = 90;
        }

        private void AdjustPosition(float xDelta, float zDelta)
        {
            var direction =
                transform.localRotation *
                new Vector3(xDelta, 0f, zDelta).normalized;
            var damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
            var distance =
                Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, _zoom) *
                damping * Time.deltaTime;

            var position = transform.localPosition;
            position += direction * distance;
            transform.localPosition = ClampPosition(position);
        }

        private static Vector3 ClampPosition(Vector3 position)
        {
            return position;
        }
    }
}