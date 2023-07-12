using System;
using System.Linq;
using UnityEngine;

namespace SF3DRacing
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarChassis : MonoBehaviour
    {
        [SerializeField] private WheelAxle[] _wheelAxles;
        [SerializeField] private Transform _centerOfMass;

        [Header("Down Force")]
        [SerializeField] private float _downForceMin;
        [SerializeField] private float _downForceMax;
        [SerializeField] private float _downForceCoef;

        [Header("Angular Drag")]
        [SerializeField] private float _angularDragMin;
        [SerializeField] private float _angularDragMax;
        [SerializeField] private float _angularDragCoef;

        private Rigidbody _rb;
        private const float SPEED_CONVERT_COEF = 3.6f;

        public float MotorTorque;
        public float BreakTorque;
        public float SteerAngle;

        public float ChassisLength;
        public float LinearVelocity => _rb.velocity.magnitude * SPEED_CONVERT_COEF;

        [SerializeField] private bool _isPrintLog;

        protected void Start()
        {
            _rb = GetComponent<Rigidbody>();

            if (_centerOfMass)
                _rb.centerOfMass = _centerOfMass.localPosition;
        }

        protected void FixedUpdate()
        {
            UpdateDownForce();
            UpdateAngularDrag();

            UpdateWheelAxles();
        }

        private void UpdateDownForce()
        {
            var downForce = Mathf.Clamp(_downForceCoef * LinearVelocity, _downForceMin, _downForceMax);
            _rb.AddForce(-transform.up * downForce);
        }

        private void UpdateAngularDrag()
        {
            _rb.angularDrag = Mathf.Clamp(_angularDragCoef * LinearVelocity, _angularDragMin, _angularDragMax);
        }

        private void UpdateWheelAxles()
        {
            var chassisLength = CalcChassisLength();
            ChassisLength = chassisLength;

            var motorWheelAmount = _wheelAxles.Count(x => x.IsMotor) * 2;

            foreach (var axle in _wheelAxles)
            {
                axle.Update();

                axle.ApplyMotorTorque(MotorTorque / motorWheelAmount);
                axle.ApplyBrakeTorque(BreakTorque);
                axle.ApplySteerAngle(SteerAngle, chassisLength);
            }
        }

        private float CalcChassisLength()
        {
            if (_isPrintLog)
                Debug.Log($"Z[0] = {_wheelAxles[0].AxleLocalPosition.z} || Z[^1] = {_wheelAxles[^1].AxleLocalPosition.z}");

            return Mathf.Abs(_wheelAxles[0].AxleLocalPosition.z - _wheelAxles[^1].AxleLocalPosition.z);
        }
    }
}
