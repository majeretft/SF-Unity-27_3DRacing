using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SF3DRacing
{
    public class Pauser : MonoBehaviour
    {
        public event UnityAction<bool> PauseStateChanged;

        private bool _isPaused;
        public bool IsPaused => _isPaused;

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void Pause()
        {
            if (_isPaused)
                return;

            Time.timeScale = 0;
            _isPaused = true;
            PauseStateChanged?.Invoke(_isPaused);
        }

        public void UnPause()
        {
            if (!_isPaused)
                return;

            Time.timeScale = 1;
            _isPaused = false;
            PauseStateChanged?.Invoke(_isPaused);
        }

        public void TogglePause()
        {
            if (_isPaused)
                UnPause();
            else
                Pause();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            UnPause();
        }
    }
}
