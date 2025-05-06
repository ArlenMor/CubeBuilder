using UnityEngine;

namespace CubeBuilder.CustomCamera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset = new Vector3(0f, 7f, 7f);
        [SerializeField] private float _rotationSpeed = 3f;
        [SerializeField] private float _zoomSpeed = 5f;
        [SerializeField] private float _minZoom = 5f;
        [SerializeField] private float _maxZoom = 20f;
        
        private Vector3 _currentOffset;
        private float _currentZoom;
    
        public void Start()
        {
            _currentOffset = _offset;
            _currentZoom = _currentOffset.magnitude;
        }

        public void LateUpdate()
        {
            if(_target == null) return;

            HandleMouseInput();
            UpdateCameraPosition();
        }

        void UpdateCameraPosition()
        {
            transform.position = _target.position + _currentOffset;
            transform.LookAt(_target.position);
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButton(1))
            {
                var horizontal = Input.GetAxis("Mouse X") * _rotationSpeed;
                var rotation = Quaternion.AngleAxis(horizontal, Vector3.up);
                _currentOffset = rotation * _currentOffset;
            }

            var scroll = Input.GetAxis("Mouse ScrollWheel");
            
            if (scroll != 0)
            {
                _currentZoom = Mathf.Clamp(_currentZoom - scroll * _zoomSpeed, _minZoom, _maxZoom);
                _currentOffset = _currentOffset.normalized * _currentZoom;
            }
        }
    }
}
