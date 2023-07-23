using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SF3DRacing
{
    public class RaceResultTime : MonoBehaviour, IDependency<RaceTimeTracker>, IDependency<RaceStateTracker>
    {
        public const string PrefMark = "_PlayerBestTime";

        public event UnityAction BestTimeUpdatedEvent;

        [SerializeField] private float _bestTime;

        private float _playerRecordTime;
        private float _currentTime;

        private RaceTimeTracker _timeTracker;
        private RaceStateTracker _raceStateTracker;

        public float PlayerRecordTime => _playerRecordTime;
        public float CurrentTime => _currentTime;
        public float BestTime => _bestTime;
        public bool HasRecord => _playerRecordTime > 0;

        protected void Awake()
        {
            Load();
        }

        protected void Start()
        {
            _raceStateTracker.CompletedEvent += OnRaceCompleted;
        }

        protected void OnDestroy()
        {
            _raceStateTracker.CompletedEvent -= OnRaceCompleted;
        }

        public void Construct(RaceTimeTracker dependency)
        {
            _timeTracker = dependency;
        }

        public void Construct(RaceStateTracker dependency)
        {
            _raceStateTracker = dependency;
        }

        public float GetAbsoluteRecord()
        {
            if (_playerRecordTime < _bestTime && _playerRecordTime > 0)
                return _playerRecordTime;
            else
                return _bestTime;
        }

        private void OnRaceCompleted()
        {
            var record = GetAbsoluteRecord();

            if (_timeTracker.RaceTime < record || _playerRecordTime <= 0)
            {
                _playerRecordTime = _timeTracker.RaceTime;

                Save();
            }

            _currentTime = _timeTracker.RaceTime;

            BestTimeUpdatedEvent?.Invoke();
        }

        private void Load()
        {
            var time = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + PrefMark, 0);
            if (time > 0)
                _playerRecordTime = time;
        }

        private void Save()
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + PrefMark, _playerRecordTime);
            PlayerPrefs.Save();
        }
    }
}
