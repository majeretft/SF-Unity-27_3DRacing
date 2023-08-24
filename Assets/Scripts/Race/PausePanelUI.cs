using UnityEngine;

namespace SF3DRacing
{
    public class PausePanelUI : MonoBehaviour, IDependency<Pauser>
    {
        [SerializeField]
        private GameObject _pausePanel;

        private Pauser _pauser;

        private void Start()
        {
            _pausePanel.SetActive(false);
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

        public void UnPause()
        {
            _pauser.UnPause();
        }

        private void OnPauseStateChanged(bool isPaused)
        {
            _pausePanel.SetActive(isPaused);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _pauser.TogglePause();
            }
        }
    }
}
