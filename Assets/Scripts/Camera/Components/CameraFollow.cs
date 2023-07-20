using UnityEngine;

namespace SF3DRacing
{
    public class CameraFollow : CameraComponent
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Rigidbody _rigitbody;

        [Header("Offest")]
        [SerializeField] private float _viewHeight;
        [SerializeField] private float _height;
        [SerializeField] private float _distance;

        [Header("Damping")]
        [SerializeField] private float _rotattionDamping;
        [SerializeField] private float _heightDamping;
        [SerializeField] private float _speedThershold = 1f;

        protected void FixedUpdate()
        {
            var velocity = _rigitbody.velocity;
            var targetRotation = _target.eulerAngles;

            if (velocity.magnitude > _speedThershold) {
                targetRotation = Quaternion.LookRotation(velocity, Vector3.up).eulerAngles; 
            }

            // Lerp
            var currentAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetRotation.y, _rotattionDamping * Time.fixedDeltaTime);
            var currentHeight = Mathf.Lerp(transform.position.y, _target.position.y + _height, _heightDamping * Time.fixedDeltaTime);

            // Position
            var positionOffset = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * _distance;
            transform.position = _target.position - positionOffset;
            transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

            // Rotation
            transform.LookAt(_target.position + new Vector3(0, _viewHeight, 0));
        }

        public override void SetProperties(Car car, Camera camera)
        {
            base.SetProperties(car, camera);

            _target = car.transform;
            _rigitbody = car.Rigidbody;
        }
    }
}
