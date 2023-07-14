using System;
using UnityEngine;

namespace SF3DRacing
{
    public class CarInputControl : MonoBehaviour
    {
        [SerializeField] private Car _car;
        [SerializeField] private SpeedometerUI _speedometer;
        [SerializeField] private AnimationCurve _brakeCurve;
        [SerializeField] private AnimationCurve _slowDownCurve;
        [SerializeField] private AnimationCurve _steerCurve;

        [SerializeField] private bool _isPrintAutoBrakeLog;
        [SerializeField] private bool _isPrintThrottleLog;
        [SerializeField] private bool _isPrintSteerLog;

        private float _wheelSpeed;
        private float _wheelSpeedAbs;
        private float _verticalAxis;
        private float _horizontalAxis;
        private float _brakeAxis;

        protected void Update()
        {
            _wheelSpeed = _car.WheelAvgSpeed;
            _wheelSpeedAbs = Mathf.Abs(_wheelSpeed);

            UpdateAxis();

            UpdateThrottleAndBrake();
            UpdateSteer();
            UpdateAutoBrake();

            UpdateSpeedometer();

            // TODO: Debug
            if (Input.GetKeyDown(KeyCode.E))
                _car.UpGear();
            if (Input.GetKeyDown(KeyCode.Q))
                _car.DownGear();
        }

        private void UpdateSpeedometer()
        {
            if (!_speedometer)
                return;

            _speedometer.CurrentSpeed = _wheelSpeedAbs;
            _speedometer.CurrentRPM = _car.MotorRpm;
            _speedometer.GearCurrent = _car.SelectedGear;
            _speedometer.GearMax = _car.GearsCount;
        }

        private void UpdateAutoBrake()
        {
            if (_wheelSpeedAbs < 20)
                return;

            if (_verticalAxis == 0)
            {
                _car.BrakeControl = _slowDownCurve.Evaluate(_wheelSpeedAbs / _car.MaxSpeed);

                if (_isPrintAutoBrakeLog)
                    Debug.Log($"Auto brake || Brake eval = {_car.BrakeControl} || Wheel speed = {_wheelSpeed} || Max speed = {_car.MaxSpeed}");
            }
        }

        private void UpdateSteer()
        {
            _car.SteerControl = _steerCurve.Evaluate(_wheelSpeedAbs / _car.MaxSpeed) * _horizontalAxis;

            if (_isPrintSteerLog)
                Debug.Log($"Steer eval = {_car.SteerControl}");
        }

        private void UpdateThrottleAndBrake()
        {
            if (_verticalAxis == 0)
            {
                _car.ThrottleControl = 0;
                return;
            }

            if (Mathf.Sign(_verticalAxis) == Mathf.Sign(_wheelSpeed) || Mathf.Abs(_wheelSpeed) < 0.5f)
            {
                _car.ThrottleControl = Mathf.Abs(_verticalAxis);
                _car.BrakeControl = 0;

                if (_isPrintThrottleLog)
                    Debug.Log("Same direction - accelerating");
            }
            else
            {
                _car.ThrottleControl = 0;
                _car.BrakeControl = _brakeCurve.Evaluate(_wheelSpeedAbs / _car.MaxSpeed);

                if (_isPrintThrottleLog)
                    Debug.Log($"Change direction - stopping car || Brake eval = {_car.BrakeControl} || Wheel speed = {_wheelSpeed} || Max speed = {_car.MaxSpeed}");
            }

            // Gears
            if (_verticalAxis < 0 && _wheelSpeedAbs <= 0.5f)
                _car.ShiftToReverseGear();
            if (_verticalAxis > 0 && _wheelSpeedAbs <= 0.5f)
                _car.ShiftToFirstGear();
        }

        private void UpdateAxis()
        {
            _verticalAxis = Input.GetAxis("Vertical");
            _horizontalAxis = Input.GetAxis("Horizontal");
            _brakeAxis = Input.GetAxis("Jump");
        }
    }
}
