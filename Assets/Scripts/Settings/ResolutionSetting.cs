using UnityEngine;

namespace SF3DRacing
{
    [CreateAssetMenu]
    public class ResolutionSetting : SettingBase
    {
        [SerializeField]
        private Vector2Int[] _resolutions = new Vector2Int[]
        {
            new Vector2Int(1920, 1080),
            new Vector2Int(1600, 900),
            new Vector2Int(1280, 720),
            new Vector2Int(1024, 768),
        };

        private int _currentIndex = 0;

        public override bool IsMinValue { get => _currentIndex <= 0; }
        public override bool IsMaxValue { get => _currentIndex >= _resolutions.Length - 1; }

        public override void SetNextValue()
        {
            if (IsMaxValue == false)
            {
                _currentIndex++;
            }
        }

        public override void SetPrevValue()
        {
            if (IsMinValue == false)
            {
                _currentIndex--;
            }
        }

        public override object GetValue()
        {
            return _resolutions[_currentIndex];
        }

        public override string GetStringValue()
        {
            return $"{_resolutions[_currentIndex].x}x{_resolutions[_currentIndex].y}";
        }

        public override void Load()
        {
            _currentIndex = PlayerPrefs.GetInt(Title, 0);
        }

        public override void Apply()
        {
            Screen.SetResolution(_resolutions[_currentIndex].x, _resolutions[_currentIndex].y, Screen.fullScreenMode);
            Save();
        }

        private void Save()
        {
            PlayerPrefs.GetInt(Title, _currentIndex);
        }
    }
}
