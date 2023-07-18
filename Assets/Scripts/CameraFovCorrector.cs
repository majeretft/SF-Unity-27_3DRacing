using UnityEngine;

namespace SF3DRacing
{
    public class CameraFovCorrector : MonoBehaviour
    {
        [SerializeField] private Car _car;
        [SerializeField] private Camera _camera;

        [SerializeField] private float _fovMin;
        [SerializeField] private float _fovMax;

        private float _fovDefault;

        protected void Start()
        {
            _camera.fieldOfView = _fovDefault;
        }

        protected void Update()
        {
            _camera.fieldOfView = Mathf.Lerp(_fovMin, _fovMax, _car.LinearVelocityNormalized);
        }
    }
}
