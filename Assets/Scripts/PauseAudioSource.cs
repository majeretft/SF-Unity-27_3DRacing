using UnityEngine;

namespace SF3DRacing
{
    [RequireComponent(typeof(AudioSource))]
    public class PauseAudioSource : MonoBehaviour, IDependency<Pauser>
    {
        private AudioSource _audioSource;
        private Pauser _pauser;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();

            _pauser.PauseStateChanged += OnPauseStateChanged;
        }

        private void OnDestroy()
        {
            _pauser.PauseStateChanged -= OnPauseStateChanged;
        }

        public void Construct(Pauser dependency)
        {
            _pauser = dependency;
        }

        private void OnPauseStateChanged(bool isPause)
        {
            if (isPause)
                _audioSource.Pause();
            else
                _audioSource.UnPause();
        }
    }
}
