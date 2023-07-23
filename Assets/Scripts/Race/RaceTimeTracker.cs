using UnityEngine;

namespace SF3DRacing
{
    public class RaceTimeTracker : MonoBehaviour, IDependency<RaceStateTracker>
    {
        private float _raceTime;
        private RaceStateTracker _tracker;

        public float RaceTime { get { return _raceTime; } }

        #region Unity Events
        protected void Start()
        {
            _tracker.StartedEvent += OnRaceStarted;
            _tracker.CompletedEvent += OnRaceCompleted;

            enabled = false;
        }

        protected void Update()
        {
            _raceTime += Time.deltaTime;
        }

        protected void OnDestroy()
        {
            _tracker.StartedEvent -= OnRaceStarted;
            _tracker.CompletedEvent -= OnRaceCompleted;
        }
        #endregion

        #region Public API
        public void Construct(RaceStateTracker dependency)
        {
            _tracker = dependency;
        }
        #endregion

        private void OnRaceStarted()
        {
            _raceTime = 0;
            enabled = true;
        }

        private void OnRaceCompleted()
        {
            enabled = false;
        }
    }
}
