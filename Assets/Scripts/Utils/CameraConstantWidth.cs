using UnityEngine;

namespace Utils
{
    public class CameraConstantWidth : MonoBehaviour
    {
        public float minSize = 0;
        public Vector2 defaultResolution = new (1920, 1080);
        [Range(0f, 1f)] public float widthOrHeight = .5f;

        private Camera _componentCamera;
    
        private float _initialSize;
        private float _targetAspect;

        private float _initialFov;
        private float _horizontalFov = 120f;

        private void Start()
        {
            _componentCamera = GetComponent<Camera>();
            _initialSize = _componentCamera.orthographicSize;

            _targetAspect = defaultResolution.x / defaultResolution.y;

            _initialFov = _componentCamera.fieldOfView;
            _horizontalFov = CalcVerticalFov(_initialFov, 1 / _targetAspect);
        }

        private void FixedUpdate()
        {
            if (_componentCamera.orthographic)
            {
                var constantWidthSize = _initialSize * (_targetAspect / _componentCamera.aspect);
                var size = Mathf.Lerp(constantWidthSize, _initialSize, widthOrHeight);
                _componentCamera.orthographicSize = size < minSize ? minSize : size;
            }
            else
            {
                var constantWidthFov = CalcVerticalFov(_horizontalFov, _componentCamera.aspect);
                _componentCamera.fieldOfView = Mathf.Lerp(constantWidthFov, _initialFov, widthOrHeight);
            }
        }

        private static float CalcVerticalFov(float hFovInDeg, float aspectRatio)
        {
            var hFovInRads = hFovInDeg * Mathf.Deg2Rad;
            var vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio);

            return vFovInRads * Mathf.Rad2Deg;
        }
    }
}