using UnityEngine;

namespace SF3DRacing
{
    [RequireComponent(typeof(CarChassis))]
    public class Car : MonoBehaviour
    {

        [Header("Engine")]
        [SerializeField] private AnimationCurve _engineTorqueCurve;
        [SerializeField] private float _motorMaxTorque;
        [SerializeField] private float _motorMinRpm;
        [SerializeField] private float _motorMaxRpm;
        public float MaxRpm => _motorMaxRpm;

        [Header("Control")]
        [SerializeField, Range(0f, 60f)] private float _maxSteerAngle;
        [SerializeField] private float _maxBrakeTorque;
        [SerializeField] private bool _isPrintLog;
        [SerializeField] private float _maxSpeed;
        public float MaxSpeed => _maxSpeed;

        [Header("Gear Box")]
        [SerializeField] private float[] _gears;
        [SerializeField] private float _finalDriveRatio;
        [SerializeField] private float _motorUpshiftRpm;
        [SerializeField] private float _motorDownshiftRpm;

        //Debug
        [SerializeField] private float _motorTorque;
        [SerializeField] private float _motorRpm;
        [SerializeField] private float _selectedGear;
        
        [SerializeField] private int _selectedGearIndex;
        
        public int GearsCount => _gears.Length;
        public int SelectedGear => _selectedGearIndex + 1;
        
        [SerializeField] private float _rearGear;
        public float ThrottleControl;
        public float SteerControl;
        public float LinearVelocityDebug;
        public float BrakeControl;


        public float LinearVelocity => _chassis.LinearVelocity;
        public float WheelAvgSpeed => _chassis.GetWheelSpeed();
        public float MotorRpm => _motorRpm;

        private CarChassis _chassis;

        protected void Start()
        {
            _chassis = GetComponent<CarChassis>();
        }

        protected void Update()
        {
            LinearVelocityDebug = LinearVelocity;

            UpdateMotorTorque();
            ShiftGearAuto();

            if (LinearVelocity >= _maxSpeed)
                _motorTorque = 0;

            _chassis.BreakTorque = _maxBrakeTorque * BrakeControl;
            _chassis.MotorTorque = _motorTorque * ThrottleControl;
            _chassis.SteerAngle = _maxSteerAngle * SteerControl;

            if (_isPrintLog)
                Debug.Log($"Torque eval = {_motorTorque} || Brake = {_chassis.BreakTorque} || Motor = {_chassis.MotorTorque} || Steer = {_chassis.SteerAngle}");
        }

        private void UpdateMotorTorque()
        {
            _motorRpm = _motorMinRpm + Mathf.Abs(_chassis.GetAvarageRmp() * _selectedGear * _finalDriveRatio);
            _motorRpm = Mathf.Clamp(_motorRpm, _motorMinRpm, _motorMaxRpm);

            _motorTorque = _engineTorqueCurve.Evaluate(_motorRpm / _motorMaxRpm) * _motorMaxTorque * _finalDriveRatio * Mathf.Sign(_selectedGear);
        }

        private void ShiftGearAuto()
        {
            if (_selectedGear < 0)
                return;

            if (_motorRpm >= _motorUpshiftRpm)
                UpGear();
            else if (_motorRpm < _motorDownshiftRpm)
                DownGear();
        }

        private void ShiftGear(int gearIndex)
        {
            gearIndex = Mathf.Clamp(gearIndex, 0, _gears.Length - 1);
            _selectedGearIndex = gearIndex;
            _selectedGear = _gears[gearIndex];
        }

        public void UpGear()
        {
            ShiftGear(_selectedGearIndex + 1);
        }

        public void DownGear()
        {
            ShiftGear(_selectedGearIndex - 1);
        }

        public void ShiftToReverseGear()
        {
            _selectedGear = _rearGear;
            _selectedGearIndex = -1;
        }

        public void ShiftToFirstGear()
        {
            ShiftGear(0);
        }

        public void ShiftToNeutralGear()
        {
            _selectedGear = 0;
        }
    }
}
