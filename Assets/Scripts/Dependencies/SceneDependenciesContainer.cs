using UnityEngine;

namespace SF3DRacing
{

    public class SceneDependenciesContainer : Dependency
    {
        [SerializeField] private TrackPointCircuit _trackPointCircuit;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private RaceStateTracker _raceStateTracker;
        [SerializeField] private Car _playerCar;
        [SerializeField] private CarInputControl _playerCarInputControl;
        [SerializeField] private RaceTimeTracker _raceTimeTracker;
        [SerializeField] private RaceResultTime _raceResultTime;

        protected void Awake()
        {
            FindObjectsToBind();
        }

        protected override void BindAll(MonoBehaviour monoBehInScene)
        {
            Bind<TrackPointCircuit>(_trackPointCircuit, monoBehInScene);
            Bind<CameraController>(_cameraController, monoBehInScene);
            Bind<RaceStateTracker>(_raceStateTracker, monoBehInScene);
            Bind<Car>(_playerCar, monoBehInScene);
            Bind<CarInputControl>(_playerCarInputControl, monoBehInScene);
            Bind<RaceTimeTracker>(_raceTimeTracker, monoBehInScene);
            Bind<RaceResultTime>(_raceResultTime, monoBehInScene);

            // if (monoBehInScene is IDependency<TrackPointCircuit>)
            //     (monoBehInScene as IDependency<TrackPointCircuit>).Construct(_trackPointCircuit);

            // if (monoBehInScene is IDependency<CameraController>)
            //     (monoBehInScene as IDependency<CameraController>).Construct(_cameraController);

            // if (monoBehInScene is IDependency<RaceStateTracker>)
            //     (monoBehInScene as IDependency<RaceStateTracker>).Construct(_raceStateTracker);

            // if (monoBehInScene is IDependency<Car>)
            //     (monoBehInScene as IDependency<Car>).Construct(_playerCar);

            // if (monoBehInScene is IDependency<CarInputControl>)
            //     (monoBehInScene as IDependency<CarInputControl>).Construct(_playerCarInputControl);

            // if (monoBehInScene is IDependency<RaceTimeTracker>)
            //     (monoBehInScene as IDependency<RaceTimeTracker>).Construct(_raceTimeTracker);

            // if (monoBehInScene is IDependency<RaceResultTime>)
            //     (monoBehInScene as IDependency<RaceResultTime>).Construct(_raceResultTime);
        }
    }
}
