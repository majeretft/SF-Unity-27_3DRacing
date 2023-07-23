using UnityEngine;

namespace SF3DRacing
{
    public class RaceInputController : MonoBehaviour, IDependency<RaceStateTracker>, IDependency<CarInputControl>
    {
        private CarInputControl _carControl;
        private RaceStateTracker _tracker;

        protected void Start()
        {
            _carControl.enabled = false;

            _tracker.StartedEvent += OnRaceStarted;
            _tracker.CompletedEvent += OnRaceCompleted;
        }

        protected void OnDestroy()
        {
            _tracker.StartedEvent -= OnRaceStarted;
            _tracker.CompletedEvent -= OnRaceCompleted;
        }

        public void Construct(CarInputControl dependency) 
        {
            _carControl = dependency;
        }

        public void Construct(RaceStateTracker dependency)
        {
            _tracker = dependency;
        }

        private void OnRaceCompleted()
        {
            _carControl.enabled = false;
            _carControl.Stop();
        }

        private void OnRaceStarted()
        {
            _carControl.enabled = true;
        }
    }
}
