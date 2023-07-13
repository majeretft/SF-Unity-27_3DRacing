using UnityEngine;

namespace SF3DRacing
{
    [RequireComponent(typeof(CarChassis))]
    public class Car : MonoBehaviour
    {
        [SerializeField, Range(0f, 60f)] private float _maxSteerAngle;
        [SerializeField] private float _maxBrakeTorque;
        [SerializeField] private bool _isPrintLog;

        [SerializeField] private AnimationCurve _engineTorqueCurve;
        [SerializeField] private float _maxMotorTorque;
        [SerializeField] private float _maxSpeed;
        public float MaxSpeed => _maxSpeed;


        public float ThrottleControl;
        public float SteerControl;
        public float LinearVelocityDebug;
        public float BrakeControl;
        public float LinearVelocity => _chassis.LinearVelocity;
        public float WheelAvgSpeed => _chassis.GetWheelSpeed();

        private CarChassis _chassis;

        protected void Start()
        {
            _chassis = GetComponent<CarChassis>();
        }

        protected void Update()
        {
            var torque = _engineTorqueCurve.Evaluate(LinearVelocity / _maxSpeed);
            LinearVelocityDebug = LinearVelocity;

            if (LinearVelocity >= _maxSpeed)
                torque = 0;

            _chassis.BreakTorque = _maxBrakeTorque * BrakeControl;
            _chassis.MotorTorque = torque * _maxMotorTorque * ThrottleControl;
            _chassis.SteerAngle = _maxSteerAngle * SteerControl;

            if (_isPrintLog)
                Debug.Log($"Torque eval = {torque} || Brake = {_chassis.BreakTorque} || Motor = {_chassis.MotorTorque} || Steer = {_chassis.SteerAngle}");
        }
    }
}
