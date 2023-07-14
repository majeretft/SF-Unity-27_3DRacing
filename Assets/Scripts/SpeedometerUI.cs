using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SF3DRacing
{
    public class SpeedometerUI : MonoBehaviour
    {
        [SerializeField] private Text _speedText;
        [SerializeField] private Text _rpmText;
        [SerializeField] private Text _gearText;
        [SerializeField] private GameObject _knotTemplate;
        [SerializeField] private Image _speedFill;
        [SerializeField] private Image _rpmFill;
        [SerializeField] private float _maxSpeed = 240;
        [SerializeField] private float _maxRpm = 6000;
        [SerializeField] private int _segmentCount = 12;

        private const float ANGLE_STEP = 22.5f;
        private const float SPEED_FILL_START = 0.12f;
        private const float SPEED_FILL_END = 0.88f;
        private const float RPM_FILL_START = 0.22f;
        private const float RPM_FILL_END = 0.78f;
        public const int SEGMENTS_MAX_COUNT = 12;

        [SerializeField] private float _currentSpeed;
        private Queue<float> _currentSpeedArray = new();
        public float CurrentSpeed
        {
            get => _currentSpeed;
            set
            {
                if (_currentSpeedArray.Count >= 50)
                    _currentSpeedArray.Dequeue();

                var temp = Mathf.Clamp(value, 0, _maxSpeed);
                _currentSpeedArray.Enqueue(temp);

                _currentSpeed = _currentSpeedArray.Average();
            }
        }
        [SerializeField] private float _currentRpm;
        private Queue<float> _currentRpmArray = new();
        public float CurrentRPM
        {
            get => _currentRpm;
            set
            {
                var temp = Mathf.Clamp(value, 0, _maxRpm);

                if (_currentRpmArray.Count >= 50)
                    _currentRpmArray.Dequeue();

                _currentRpmArray.Enqueue(temp);

                _currentRpm = _currentRpmArray.Average();
            }
        }

        private float _speedFillDiff = SPEED_FILL_END - SPEED_FILL_START;
        private float _rpmFillDiff = RPM_FILL_END - RPM_FILL_START;

        public int GearCurrent { get; set; }
        public int GearMax { get; set; }

        protected void Start()
        {
            _knotTemplate.gameObject.SetActive(false);

            Init(_maxSpeed, _segmentCount);
        }

        protected void Update()
        {
            var normalizedSpeed = Mathf.Clamp(CurrentSpeed / _maxSpeed, 0, 1);
            var speedFillCoef = normalizedSpeed * _speedFillDiff;

            var normalizedRpm = Mathf.Clamp(CurrentRPM / _maxRpm, 0, 1);
            var rpmFillCoef = normalizedRpm * _rpmFillDiff;

            _speedFill.fillAmount = Mathf.Clamp(speedFillCoef + SPEED_FILL_START, SPEED_FILL_START, SPEED_FILL_END);
            _rpmFill.fillAmount = Mathf.Clamp(rpmFillCoef + RPM_FILL_START, RPM_FILL_START, RPM_FILL_END);

            _speedText.text = Mathf.RoundToInt(CurrentSpeed).ToString();
            _rpmText.text = Mathf.RoundToInt(CurrentRPM).ToString();

            _gearText.text = $"{(GearCurrent > 0 ? GearCurrent : "R")} / {GearMax}";
        }

        private void Init(float maxSpeed, int segmentCount)
        {
            _speedFill.fillAmount = 0;
            _rpmFill.fillAmount = 0;

            _speedText.text = "-";
            _rpmText.text = "-";

            if (segmentCount > SEGMENTS_MAX_COUNT)
                segmentCount = SEGMENTS_MAX_COUNT;

            var speedStep = Mathf.RoundToInt(maxSpeed / segmentCount);

            for (var i = 0; i < segmentCount; i++)
            {
                var instance = Instantiate(_knotTemplate, _knotTemplate.transform.position, _knotTemplate.transform.rotation, transform);
                instance.transform.eulerAngles = new Vector3(0, 0, i * ANGLE_STEP * -1);

                var textTransform = instance.transform.Find("Text");
                if (i > 5)
                    textTransform.eulerAngles = textTransform.eulerAngles + new Vector3(0, 0, 180);

                var textElement = textTransform.GetComponent<Text>();
                textElement.text = ((i + 1) * speedStep).ToString();

                instance.SetActive(true);
            }
        }
    }
}
