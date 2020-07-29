using UnityEngine;

namespace Plugins.Cube.Camera
{
    internal static class InputManager
    {
        private static float _time;
        private static bool _doubleClick;
        private static Vector2 _currentPosXy;
        private static Vector2 _deltaXy;
        private const float Speed = 0.05f;

        public static float GetMouse_X()
        {
            if (IsMouseButton())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _currentPosXy.x = Input.mousePosition.x;
                    ;
                }
                else if (Input.GetMouseButton(0))
                {
                    var x = Input.mousePosition.x;
                    if (_currentPosXy.x != x)
                    {
                        _deltaXy.x = x - _currentPosXy.x;
                        _currentPosXy.x = x;
                    }
                    else
                    {
                        _deltaXy.x = 0;
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    _deltaXy.x = 0;
                }

                return _deltaXy.x * 1f;
            }

            if (Input.touchCount > 0)
                return Input.GetTouch(0).deltaPosition.x * 0.5f * Speed;
            return 0;
        }

        public static float GetMouse_Y()
        {
            if (IsMouseButton())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _currentPosXy.y = Input.mousePosition.y;
                }
                else if (Input.GetMouseButton(0))
                {
                    var y = Input.mousePosition.y;
                    if (_currentPosXy.y != y)
                    {
                        _deltaXy.y = y - _currentPosXy.y;
                        _currentPosXy.y = y;
                    }
                    else
                    {
                        _deltaXy.y = 0;
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    _deltaXy.y = 0;
                }

                return _deltaXy.y * 0.1f;
            }

            if (Input.touchCount > 0)
                return Input.GetTouch(0).deltaPosition.y * 0.5f * Speed;
            return 0;
        }

        public static float GetMouse_ScrollWheel()
        {
            if (IsMouseButton()) return Input.GetAxis("Mouse ScrollWheel");

            if (Input.touchCount == 2)
            {
                var off0 = Input.GetTouch(0);
                var off1 = Input.GetTouch(1);

                var offDis1 = off0.position - off1.position;
                var offDis2 = off0.position - off0.deltaPosition - (off1.position - off1.deltaPosition);

                return (offDis1.magnitude * 0.1f - offDis2.magnitude * 0.1f) * Speed;
            }

            return 0;
        }

        private static bool IsMouseButton()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WebGLPlayer:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXEditor:
                    return true;
            }

            return false;
        }

        public static bool GetMouseButton()
        {
            if (IsMouseButton())
            {
                if (Input.GetMouseButton(0))
                    return true;
                return false;
            }

            if (Input.touchCount == 1)
                return true;
            return false;
        }

        public static bool GetDoubleClick()
        {
            var isTrue = false;
            if (!IsMouseButton())
            {
                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended && !_doubleClick)
                {
                    _time = Time.time;
                    _doubleClick = true;
                }

                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended && Time.time - _time > 0f &&
                    Time.time - _time < 0.5f && _doubleClick)
                {
                    isTrue = true;
                    _doubleClick = false;
                }
                else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended && _doubleClick &&
                         Time.time - _time >= 0.5f)
                {
                    _doubleClick = false;
                    isTrue = false;
                }

                return isTrue;
            }

            if (Input.GetMouseButtonDown(0) && !_doubleClick)
            {
                _time = Time.time;
                _doubleClick = true;
            }

            if (Input.GetMouseButtonDown(0) && Time.time - _time > 0f && Time.time - _time < 0.5f && _doubleClick)
            {
                isTrue = true;
                _doubleClick = false;
            }
            else if (Input.GetMouseButtonDown(0) && _doubleClick && Time.time - _time >= 0.5f)
            {
                _doubleClick = false;
                isTrue = false;
            }

            return isTrue;
        }
    }
}