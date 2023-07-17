using UnityEngine;

namespace SF3DRacing
{
    [RequireComponent(typeof(AudioSource))]
    public class EngineSound : MonoBehaviour
    {
        [SerializeField] private Car _car;

        [SerializeField] private float _volumeCoef;
        [SerializeField] private float _pitchCoef;
        [SerializeField] private float _rpmCoef;

        [SerializeField] private float _pitchBase = 1f;
        [SerializeField] private float _volumeBase = 0.4f;

        private AudioSource _audio;

        protected void Start()
        {
            _audio = GetComponent<AudioSource>();
        }

        protected void Update()
        {
            _audio.pitch = _pitchBase + _pitchCoef * (_car.MotorRpm / _car.MaxRpm * _rpmCoef);
            _audio.volume = _volumeBase + _volumeCoef * (_car.MotorRpm / _car.MaxRpm);
        }
    }
}
