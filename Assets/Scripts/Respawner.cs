using UnityEngine;

namespace SF3DRacing
{
    public class Respawner : MonoBehaviour, IDependency<RaceStateTracker>, IDependency<Car>, IDependency<CarInputControl>
    {
        [SerializeField] private float _respawnHeight;

        private RaceStateTracker _tracker;
        private Car _car;
        private CarInputControl _inputControl;
        private TrackPoint _respawnTrackPoint;

        protected void Start()
        {
            _tracker.TrackPointTriggeredEvent += OnTrackPointTriggered;
        }

        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Respawn();
            }
        }

        public void Respawn()
        {
            if (!_respawnTrackPoint)
                return;

            if (_tracker.State != RaceStateEnum.Running)
                return;

            _car.Respawn(
                _respawnTrackPoint.transform.position + _respawnTrackPoint.transform.up * _respawnHeight,
                _respawnTrackPoint.transform.rotation
            );

            _inputControl.Reset();
        }

        public void Construct(RaceStateTracker dependency)
        {
            _tracker = dependency;
        }

        public void Construct(Car dependency)
        {
            _car = dependency;
        }

        public void Construct(CarInputControl dependency)
        {
            _inputControl = dependency;
        }

        private void OnTrackPointTriggered(TrackPoint point)
        {
            _respawnTrackPoint = point;
        }
    }
}
