using UnityEngine;

namespace SF3DRacing
{
    public class SettingLoader : MonoBehaviour
    {
        [SerializeField] private SettingBase[] _settings;

        private void Awake()
        {
            foreach (var setting in _settings)
            {
                setting.Load();
            }
        }

        private void Start()
        {
            foreach (var setting in _settings)
            {
                setting.Apply();
            }
        }
    }
}
