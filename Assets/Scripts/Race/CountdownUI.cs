using UnityEngine;
using UnityEngine.UI;

namespace SF3DRacing
{
    public class CountdownUI : MonoBehaviour, IDependency<RaceStateTracker>
    {
        [SerializeField] private Text _text;
        [SerializeField] private Timer _timer;
        
        private RaceStateTracker _raceStateTracker;

        protected void Start()
        {
            _raceStateTracker.PreparationStartedEvent += OnPreparationStarted;
            _raceStateTracker.StartedEvent += OnRaceStarted;

            _text.enabled = false;
        }

        protected void OnDestroy()
        {
            _raceStateTracker.PreparationStartedEvent -= OnPreparationStarted;
            _raceStateTracker.StartedEvent -= OnRaceStarted;
        }

        private void OnRaceStarted()
        {
            _text.enabled = false;
            enabled = false;
        }

        private void OnPreparationStarted()
        {
            _text.enabled = true;
            enabled = true;
        }

        protected void Update()
        {
            _text.text = _timer.CurrentTime.ToString("F0");

            if (_text.text == "0")
                _text.text = "GO!";
        }

        public void Construct(RaceStateTracker dependency)
        {
            _raceStateTracker = dependency;
        }
    }
}
