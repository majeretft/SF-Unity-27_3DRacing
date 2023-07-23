using UnityEngine;
using UnityEngine.Events;

namespace SF3DRacing
{
    public enum RaceStateEnum
    {
        Preparing,
        CountDown,
        Running,
        Finished
    }

    public class RaceStateTracker : MonoBehaviour, IDependency<TrackPointCircuit>
    {
        public event UnityAction PreparationStartedEvent;
        public event UnityAction StartedEvent;
        public event UnityAction CompletedEvent;
        public event UnityAction<TrackPoint> TrackPointTriggeredEvent;
        public event UnityAction<int> LapCompletedEvent;
        public event UnityAction<RaceStateEnum> StateChangedEvent;

        [SerializeField] private int _lapsToComplete;
        [SerializeField] private Timer _countdownTimer;

        private TrackPointCircuit _trackpointCircuit;
        private RaceStateEnum _state;
        public RaceStateEnum State => _state;

        #region Unity Events
        protected void Start()
        {
            ChangeState(RaceStateEnum.Preparing);

            _countdownTimer.enabled = false;

            _trackpointCircuit.TrackPointTriggeredEvent += OnTrackPointTriggered;
            _trackpointCircuit.LapCompletedEvent += OnLapCompleted;
            _countdownTimer.FinishedEvent += OnCountdownFinished;
        }

        protected void OnDestroy()
        {
            _trackpointCircuit.TrackPointTriggeredEvent -= OnTrackPointTriggered;
            _trackpointCircuit.LapCompletedEvent -= OnLapCompleted;
            _countdownTimer.FinishedEvent -= OnCountdownFinished;
        }
        #endregion

        #region Public API
        public void StartPreparation()
        {
            if (State != RaceStateEnum.Preparing)
                return;

            ChangeState(RaceStateEnum.CountDown);
            _countdownTimer.enabled = true;

            PreparationStartedEvent?.Invoke();
        }

        public void Construct(TrackPointCircuit trackPointCircuit)
        {
            _trackpointCircuit = trackPointCircuit;
        }
        #endregion

        private void OnCountdownFinished()
        {
            StartRace();
        }

        private void OnLapCompleted(int lapsCompleted)
        {
            if (_trackpointCircuit.Type == TrackTypeEnum.Sprint)
            {
                CompleteRace();
            }

            if (_trackpointCircuit.Type == TrackTypeEnum.Circular)
            {
                if (lapsCompleted == _lapsToComplete)
                    CompleteRace();
                else
                    CompleteLap(lapsCompleted);
            }
        }

        private void OnTrackPointTriggered(TrackPoint trackPoint)
        {
            TrackPointTriggeredEvent?.Invoke(trackPoint);
        }

        private void ChangeState(RaceStateEnum state)
        {
            _state = state;

            StateChangedEvent?.Invoke(_state);
        }

        private void CompleteRace()
        {
            if (State != RaceStateEnum.Running)
                return;

            ChangeState(RaceStateEnum.Finished);

            CompletedEvent?.Invoke();
        }

        private void StartRace()
        {
            if (State != RaceStateEnum.CountDown)
                return;

            ChangeState(RaceStateEnum.Running);

            StartedEvent?.Invoke();
        }

        private void CompleteLap(int lapsCompleted)
        {
            LapCompletedEvent?.Invoke(lapsCompleted);
        }
    }
}
