using System;
using UnityEngine;

namespace SF3DRacing
{
    public class RaceInputController : MonoBehaviour
    {
        [SerializeField] private CarInputControl _carControl;
        [SerializeField] private RaceStateTracker _tracker;

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
