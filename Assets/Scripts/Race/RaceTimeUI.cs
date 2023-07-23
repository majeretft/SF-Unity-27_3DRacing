using UnityEngine;
using UnityEngine.UI;

namespace SF3DRacing
{
    public class RaceTimeUI : MonoBehaviour, IDependency<RaceTimeTracker>, IDependency<RaceStateTracker>
    {
        [SerializeField] private Text _timeText;

        private RaceTimeTracker _timeTracker;
        private RaceStateTracker _raceStateTracker;

        #region Unity Events
        protected void Start()
        {
            _raceStateTracker.StartedEvent += OnRaceStarted;
            _raceStateTracker.CompletedEvent += OnRaceCompleted;

            _timeText.enabled = false;
        }

        protected void OnDestroy()
        {
            _raceStateTracker.StartedEvent -= OnRaceStarted;
            _raceStateTracker.CompletedEvent -= OnRaceCompleted;
        }

        protected void Update()
        {
            _timeText.text = StringTime.SecondsToString(_timeTracker.RaceTime);
        }
        #endregion

        #region Public API
        public void Construct(RaceTimeTracker dependency)
        {
            _timeTracker = dependency;
        }

        public void Construct(RaceStateTracker dependency)
        {
            _raceStateTracker = dependency;
        }
        #endregion

        private void OnRaceStarted()
        {
            _timeText.enabled = true;
            enabled = true;
        }

        private void OnRaceCompleted()
        {
            _timeText.enabled = false;
            enabled = false;
        }
    }
}
