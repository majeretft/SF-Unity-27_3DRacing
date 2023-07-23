using UnityEngine;
using UnityEngine.UI;

namespace SF3DRacing
{
    public class ResultsUI : MonoBehaviour, IDependency<RaceResultTime>
    {
        [SerializeField] private GameObject _resultsGroup;
        [SerializeField] private Text _bestTime;
        [SerializeField] private Text _recoedTime;
        [SerializeField] private Text _currentTime;

        private RaceResultTime _resultTime;

        protected void Start()
        {
            _resultsGroup.SetActive(false);

            _resultTime.BestTimeUpdatedEvent += OnBestTimeUpdated;
        }

        protected void OnDestroy()
        {
            _resultTime.BestTimeUpdatedEvent -= OnBestTimeUpdated;
        }

        public void Construct(RaceResultTime dependency)
        {
            _resultTime = dependency;
        }

        private void OnBestTimeUpdated()
        {
            _resultsGroup.SetActive(true);

            _bestTime.text = StringTime.SecondsToString(_resultTime.BestTime);
            _recoedTime.text = _resultTime.HasRecord
                ? StringTime.SecondsToString(_resultTime.PlayerRecordTime)
                : "-";
            _currentTime.text = StringTime.SecondsToString(_resultTime.CurrentTime);
        }
    }
}
