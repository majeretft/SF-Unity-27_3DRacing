using UnityEngine;
using UnityEngine.UI;

namespace SF3DRacing
{
    public class HintsUI : MonoBehaviour, IDependency<RaceStateTracker>
    {
        [SerializeField] private GameObject _raceGroup;
        [SerializeField] private GameObject _prepareGroup;

        private RaceStateTracker _tracker;

        protected void Start()
        {
            _raceGroup.SetActive(false);
            _prepareGroup.SetActive(false);

            _tracker.StateChangedEvent += OnStateChanged;
        }

        protected void OnDestroy()
        {
            _tracker.StateChangedEvent -= OnStateChanged;
        }

        public void Construct(RaceStateTracker dependency)
        {
            _tracker = dependency;
        }

        private void OnStateChanged(RaceStateEnum state)
        {
            switch (state)
            {
                case RaceStateEnum.Preparing:
                    _prepareGroup.SetActive(true);
                    _raceGroup.SetActive(false);
                    break;
                case RaceStateEnum.Running:
                    _prepareGroup.SetActive(false);
                    _raceGroup.SetActive(true);
                    break;
                case RaceStateEnum.Finished:
                    _prepareGroup.SetActive(false);
                    _raceGroup.SetActive(true);
                    break;
            }
        }
    }
}
