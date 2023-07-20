using System;
using UnityEngine;

namespace SF3DRacing
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Car _car;
        [SerializeField] private Camera _camera;
        [SerializeField] private CameraFollow _follower;
        [SerializeField] private CameraShaker _shaker;
        [SerializeField] private CameraFovCorrector _fovCorrector;
        [SerializeField] private CameraPathFollower _pathFollower;
        [SerializeField] private RaceStateTracker _tracker;

        protected void Awake()
        {
            _follower.SetProperties(_car, _camera);
            _shaker.SetProperties(_car, _camera);
            _fovCorrector.SetProperties(_car, _camera);
            _pathFollower.SetProperties(_car, _camera);
        }

        protected void Start()
        {
            _tracker.PreparationStartedEvent += OnPreparationStarted;
            _tracker.CompletedEvent += OnRaceCompleted;

            _pathFollower.enabled = true;
            _follower.enabled = false;
        }

        protected void OnDestroy()
        {
            _tracker.PreparationStartedEvent -= OnPreparationStarted;
            _tracker.CompletedEvent -= OnRaceCompleted;
        }

        private void OnRaceCompleted()
        {
            _pathFollower.enabled = true;
            _follower.enabled = false;

            _pathFollower.StartMoveToNearestPoint();
            _pathFollower.SetLookTarget(_car.transform);
        }

        private void OnPreparationStarted()
        {
            _pathFollower.enabled = false;
            _follower.enabled = true;
        }
    }
}
