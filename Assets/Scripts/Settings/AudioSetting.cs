using UnityEngine;
using UnityEngine.Audio;

namespace SF3DRacing
{
    [CreateAssetMenu]
    public class AudioSetting : SettingBase
    {
        [SerializeField]
        private AudioMixer _audioMixer;
        [SerializeField]
        private string _mixerParameterName;

        [SerializeField]
        private float _minRealValue;
        [SerializeField]
        private float _maxRealValue;

        [SerializeField]
        private float _virtualStep;
        [SerializeField]
        private float _minVirtualValue;
        [SerializeField]
        private float _maxVirtualValue;

        private float _currentValue = 0;

        public override bool IsMinValue { get => _currentValue <= _minRealValue; }
        public override bool IsMaxValue { get => _currentValue >= _maxRealValue; }

        public override void SetNextValue()
        {
            AddValue(Mathf.Abs((_maxRealValue - _minRealValue) / _virtualStep));
        }

        public override void SetPrevValue()
        {
            AddValue(-Mathf.Abs((_maxRealValue - _minRealValue) / _virtualStep));
        }

        public override object GetValue()
        {
            return _currentValue;
        }

        public override string GetStringValue()
        {
            return Mathf
                .Lerp(_minVirtualValue, _maxVirtualValue, (_currentValue - _minRealValue) / (_maxRealValue - _minRealValue))
                .ToString();
        }

        public override void Apply()
        {
            _audioMixer.SetFloat(_mixerParameterName, _currentValue);
            Save();
        }

        public override void Load()
        {
            _currentValue = PlayerPrefs.GetFloat(Title, 0);
        }

        private void AddValue(float value)
        {
            _currentValue += value;
            _currentValue = Mathf.Clamp(_currentValue, _minRealValue, _maxRealValue);
        }

        private void Save()
        {
            PlayerPrefs.SetFloat(Title, _currentValue);
        }
    }
}
