using UnityEngine;

namespace SF3DRacing
{
    [CreateAssetMenu]
    public class DisplayModeSetting : SettingBase
    {
        [SerializeField]
        private string[] _modes = new string[]
        {
            "Exclusive Full Screen",
            "Fullscreen Window",
            "Maximized Window",
            "Windowed",
        };

        private int _currentIndex = 0;

        public override bool IsMinValue { get => _currentIndex <= 0; }
        public override bool IsMaxValue { get => _currentIndex >= _modes.Length - 1; }

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
            return GetMode();
        }

        public override string GetStringValue()
        {
            return _modes[_currentIndex];
        }

        public override void Apply()
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, GetMode());
            Save();
        }

        public FullScreenMode GetMode()
        {
            FullScreenMode mode = FullScreenMode.ExclusiveFullScreen;

            switch (_modes[_currentIndex])
            {
                case "Exclusive Full Screen": mode = FullScreenMode.ExclusiveFullScreen; break;
                case "Fullscreen Window": mode = FullScreenMode.FullScreenWindow; break;
                case "Maximized Window": mode = FullScreenMode.MaximizedWindow; break;
                case "Windowed": mode = FullScreenMode.Windowed; break;
                default: break;
            }

            return mode;
        }

        public override void Load()
        {
            _currentIndex = PlayerPrefs.GetInt(Title, 0);
        }

        private void Save()
        {
            PlayerPrefs.SetInt(Title, _currentIndex);
        }
    }
}
