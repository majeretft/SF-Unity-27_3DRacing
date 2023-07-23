using UnityEngine;
using UnityEngine.UI;

namespace SF3DRacing
{
    public class RaceRecordTimeUI : MonoBehaviour, IDependency<RaceStateTracker>, IDependency<RaceResultTime>
    {
        [SerializeField] private GameObject _bestRecordObj;
        [SerializeField] private GameObject _playerRecordObj;
        [SerializeField] private Text _bestRecordText;
        [SerializeField] private Text _playerRecordText;

        private RaceResultTime _resultTime;
        private RaceStateTracker _raceStateTracker;

        protected void Start()
        {
            _raceStateTracker.StartedEvent += OnRaceStarted;
            _raceStateTracker.CompletedEvent += OnRaceCompleted;

            _bestRecordObj.SetActive(false);
            _playerRecordObj.SetActive(false);
        }

        protected void OnDestroy()
        {
            _raceStateTracker.StartedEvent -= OnRaceStarted;
            _raceStateTracker.CompletedEvent -= OnRaceCompleted;
        }

        public void Construct(RaceStateTracker dependency)
        {
            _raceStateTracker = dependency;
        }

        public void Construct(RaceResultTime dependency)
        {
            _resultTime = dependency;
        }

        private void OnRaceStarted()
        {
            if (_resultTime.PlayerRecordTime > _resultTime.BestTime || _resultTime.HasRecord == false)
            {
                _bestRecordObj.SetActive(true);
                _bestRecordText.text = StringTime.SecondsToString(_resultTime.BestTime);
            }

            if (_resultTime.HasRecord == true)
            {
                _playerRecordObj.SetActive(true);
                _playerRecordText.text = StringTime.SecondsToString(_resultTime.PlayerRecordTime);
            }
        }

        private void OnRaceCompleted()
        {
            _bestRecordObj.SetActive(false);
            _playerRecordObj.SetActive(false);
        }
    }
}
