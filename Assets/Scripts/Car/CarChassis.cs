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
        public Rigidbody Rigidbody => _rb ?? GetComponent<Rigidbody>();
        private const float SPEED_MS_TO_KMH_COEF = 3.6f;

        public float MotorTorque;
        public float BreakTorque;
        public float SteerAngle;

        public float ChassisLength;
        public float LinearVelocity => _rb.velocity.magnitude * SPEED_MS_TO_KMH_COEF;

        [SerializeField] private bool _isPrintLog;
        [SerializeField] private bool _isPrintWheelSpeedLog;

        protected void Start()
        {
            _rb = GetComponent<Rigidbody>();

            if (_centerOfMass)
                _rb.centerOfMass = _centerOfMass.localPosition;

            foreach (var axle in _wheelAxles)
            {
                axle.ConfigureVehicleSubsteps(50, 50, 50);
            }
        }

        protected void FixedUpdate()
        {
            UpdateDownForce();
            UpdateAngularDrag();

            UpdateWheelAxles();
        }

        public float GetAvarageRmp()
        {
            var sum = _wheelAxles.Aggregate(0f, (acc, axle) => acc += axle.GetAvarageRmp());

            // Debug.Log($"sum = {sum}");
            return sum / _wheelAxles.Length;
        }

        public float GetWheelSpeed()
        {
            const float coef = 2 * Mathf.PI / 60 * SPEED_MS_TO_KMH_COEF;
            // return GetAvarageRmp() * _wheelAxles[0].GetWheelRadius() * 2 * 0.1885f;
            var avgRpm = GetAvarageRmp();
            var radius = _wheelAxles[0].GetWheelRadius();
            var result = avgRpm * radius * coef;

            if (_isPrintWheelSpeedLog)
                Debug.Log($"Coef = {coef} || Avg RPM = {avgRpm} || Radius = {radius} || Res = {result}");

            return result;
        }

        public void Reset()
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
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
