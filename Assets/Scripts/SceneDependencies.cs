using UnityEngine;

namespace SF3DRacing
{
    public interface IDependency<T>
    {
        void Construct(T dependency);
    }

    public class SceneDependencies : MonoBehaviour
    {
        [SerializeField] private TrackPointCircuit _trackPointCircuit;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private RaceStateTracker _raceStateTracker;
        [SerializeField] private Car _playerCar;
        [SerializeField] private CarInputControl _playerCarInputControl;


        protected void Awake()
        {
            var monoBehs = FindObjectsOfType<MonoBehaviour>();

            foreach (var monoBeh in monoBehs)
            {
                Bind(monoBeh);
            }
        }

        private void Bind(MonoBehaviour monoBeh)
        {
            if (monoBeh is IDependency<TrackPointCircuit>)
                (monoBeh as IDependency<TrackPointCircuit>).Construct(_trackPointCircuit);

            if (monoBeh is IDependency<CameraController>)
                (monoBeh as IDependency<CameraController>).Construct(_cameraController);

            if (monoBeh is IDependency<RaceStateTracker>)
                (monoBeh as IDependency<RaceStateTracker>).Construct(_raceStateTracker);

            if (monoBeh is IDependency<Car>)
                (monoBeh as IDependency<Car>).Construct(_playerCar);

            if (monoBeh is IDependency<CarInputControl>)
                (monoBeh as IDependency<CarInputControl>).Construct(_playerCarInputControl);
        }
    }
}
