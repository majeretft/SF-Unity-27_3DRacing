using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SF3DRacing
{
    public enum MenuActions
    {
        Hover,
        ButtonClick,
        SettingChange,
    }

    [Serializable]
    public class SoundsMap
    {
        public AudioClip sound;
        public MenuActions action;
    }

    [RequireComponent(typeof(AudioSource))]
    public class MenuSfx : MonoBehaviour
    {
        [SerializeField] private List<SoundsMap> _soundsMap;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(MenuActions action)
        {
            var sound = _soundsMap.Where(x => x.action == action).Select(x => x.sound).FirstOrDefault();
            if (!sound)
                return;

            _audioSource.PlayOneShot(sound);
        }
    }
}
