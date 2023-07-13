using System;
using UnityEngine;

namespace SF3DRacing
{
    [Serializable]
    public class WheelAxle
    {
        [SerializeField] private WheelCollider _leftWheelCollider;
        [SerializeField] private WheelCollider _rightWheelCollider;

        [SerializeField] private Transform _leftWheelTransform;
        [SerializeField] private Transform _rightWheelTransform;

        [SerializeField] private bool _isMotor;
        public bool IsMotor => _isMotor;
        [SerializeField] private bool _isSteer;
        public bool IsSteer => _isSteer;

        [SerializeField] private float _chassisWidth;
        [SerializeField] private float _antiRollCoef;
        [SerializeField] private float _additionalDownForce;
        [SerializeField] private float _baseForwardStifness = 1.5f;
        [SerializeField] private float _stabilityForwardCoef = 1f;
        [SerializeField] private float _baseSideStifness = 2f;
        [SerializeField] private float _stabilitySideCoef = 1f;

        [SerializeField] private bool _isPrintLog;

        private WheelHit _leftWheelHit;
        private WheelHit _rightWheelHit;

        public Vector3 AxleLocalPosition => new(0, _leftWheelCollider.transform.localPosition.y, _leftWheelCollider.transform.localPosition.z);

        public void Update()
        {
            _chassisWidth = Vector3.Distance(_leftWheelCollider.transform.localPosition, _rightWheelCollider.transform.localPosition);

            UpdateWheelHit();

            ApplyAntiRoll();
            ApplyDownForce();
            CorrectSwiftness();

            SyncMeshTransform();
        }

        private void UpdateWheelHit()
        {
            _leftWheelCollider.GetGroundHit(out _leftWheelHit);
            _rightWheelCollider.GetGroundHit(out _rightWheelHit);
        }

        private void CorrectSwiftness()
        {
            var leftFwdFriction = _leftWheelCollider.forwardFriction;
            var rightFwdFriction = _rightWheelCollider.forwardFriction;

            var leftSideFriction = _leftWheelCollider.sidewaysFriction;
            var rightSideFriction = _rightWheelCollider.sidewaysFriction;

            leftFwdFriction.stiffness = _baseForwardStifness + Mathf.Abs(_leftWheelHit.forwardSlip) * _stabilityForwardCoef;
            rightFwdFriction.stiffness = _baseForwardStifness + Mathf.Abs(_rightWheelHit.forwardSlip) * _stabilityForwardCoef;

            leftSideFriction.stiffness = _baseSideStifness + Mathf.Abs(_leftWheelHit.forwardSlip) * _stabilitySideCoef;
            rightSideFriction.stiffness = _baseSideStifness + Mathf.Abs(_rightWheelHit.forwardSlip) * _stabilitySideCoef;

            _leftWheelCollider.forwardFriction = leftFwdFriction;
            _rightWheelCollider.forwardFriction = rightFwdFriction;

            _leftWheelCollider.sidewaysFriction = leftSideFriction;
            _rightWheelCollider.sidewaysFriction = rightSideFriction;
        }

        private void ApplyDownForce()
        {
            if (_leftWheelCollider.isGrounded)
                _leftWheelCollider.attachedRigidbody.AddForceAtPosition(
                    _leftWheelHit.normal * _additionalDownForce * _leftWheelCollider.attachedRigidbody.velocity.magnitude * -1,
                    _leftWheelCollider.transform.position);

            if (_rightWheelCollider.isGrounded)
                _rightWheelCollider.attachedRigidbody.AddForceAtPosition(
                    _rightWheelHit.normal * _additionalDownForce * _rightWheelCollider.attachedRigidbody.velocity.magnitude * -1,
                    _rightWheelCollider.transform.position);
        }

        private void ApplyAntiRoll()
        {
            var travelL = 1f;
            var travelR = 1f;

            if (_leftWheelCollider.isGrounded)
                travelL = -_leftWheelCollider.transform.InverseTransformPoint(_leftWheelHit.point).y - _leftWheelCollider.radius / _leftWheelCollider.suspensionDistance;

            if (_rightWheelCollider.isGrounded)
                travelR = -_rightWheelCollider.transform.InverseTransformPoint(_rightWheelHit.point).y - _rightWheelCollider.radius / _rightWheelCollider.suspensionDistance;

            var forceDir = travelL - travelR;

            if (_leftWheelCollider.isGrounded)
                _leftWheelCollider.attachedRigidbody.AddForceAtPosition(_leftWheelCollider.transform.up * -forceDir * _antiRollCoef, _leftWheelCollider.transform.position);

            if (_rightWheelCollider.isGrounded)
                _rightWheelCollider.attachedRigidbody.AddForceAtPosition(_rightWheelCollider.transform.up * forceDir * _antiRollCoef, _rightWheelCollider.transform.position);
        }

        public void ApplySteerAngle(float angle, float chassisLength)
        {
            if (_isSteer == false)
                return;

            var angleAbs = Mathf.Abs(angle);

            if (angleAbs < 0.1f)
            {
                _leftWheelCollider.steerAngle = 0;
                _rightWheelCollider.steerAngle = 0;

                return;
            }

            var angleSign = Mathf.Sign(angle);
            var xc = Mathf.Abs(chassisLength / Mathf.Tan(angleAbs * Mathf.Deg2Rad));

            var angleLeft = Mathf.Atan(chassisLength / (xc - _chassisWidth * 0.5f)) * angleSign * Mathf.Rad2Deg;
            var angleRight = Mathf.Atan(chassisLength / (xc + _chassisWidth * 0.5f)) * angleSign * Mathf.Rad2Deg;

            _leftWheelCollider.steerAngle = angleLeft;
            _rightWheelCollider.steerAngle = angleRight;

            if (_isPrintLog)
                Debug.Log($"Xc = {xc} || Angle_C = {angle} || Angle_L = {angleLeft} || Angle_R = {angleRight} || Chassis Length = {chassisLength} || Chassis Width = {_chassisWidth}");
        }

        public void ApplyMotorTorque(float torque)
        {
            if (_isMotor == false)
                return;


            _leftWheelCollider.motorTorque = torque;
            _rightWheelCollider.motorTorque = torque;

            // Debug.Log($"Motor Torque = {torque}");
        }

        public void ApplyBrakeTorque(float torque)
        {
            _leftWheelCollider.brakeTorque = torque;
            _rightWheelCollider.brakeTorque = torque;
        }

        public float GetAvarageRmp()
        {
            // Debug.Log($"Left RMP = {_leftWheelCollider.rpm} || Right RPM = {_rightWheelCollider.rpm}");
            return (_leftWheelCollider.rpm + _rightWheelCollider.rpm) / 2;
        }

        public float GetWheelRadius()
        {
            return _leftWheelCollider.radius;
        }

        private void SyncMeshTransform()
        {
            UpdateWheelTransform(_leftWheelCollider, _leftWheelTransform);
            UpdateWheelTransform(_rightWheelCollider, _rightWheelTransform);
        }

        private void UpdateWheelTransform(WheelCollider collider, Transform transform)
        {
            collider.GetWorldPose(out var position, out var rotation);

            transform.position = position;
            transform.rotation = rotation;
        }
    }
}
