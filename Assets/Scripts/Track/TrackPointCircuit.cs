using UnityEngine;
using UnityEngine.Events;

namespace SF3DRacing
{
    public enum TrackTypeEnum
    {
        Circular,
        Sprint,
    }

    public class TrackPointCircuit : MonoBehaviour
    {
        [SerializeField] private TrackTypeEnum _type;
        public TrackTypeEnum Type => _type;
        [SerializeField] private bool _isPrintLog;

        private TrackPoint[] _points;
        public event UnityAction<int> LapCompletedEvent;
        public event UnityAction<TrackPoint> TrackPointTriggeredEvent;
        private int _lapsCompleted = -1;

        private UnityAction<int> LapCompletedDebugLog = (x) => Debug.Log($"Laps Completed = {x}");
        private UnityAction<TrackPoint> TrackPointTriggereDebugLog = (x) => Debug.Log($"Laps Completed = {x.name}");

        protected void Start()
        {
            BuildCircuit();

            foreach (var point in _points)
            {
                point.TriggeredEvent += OnTrackPointTriggered;
            }

            _points[0].MarkAsTarget();

            if (_isPrintLog)
            {
                LapCompletedEvent += LapCompletedDebugLog;
                TrackPointTriggeredEvent += TrackPointTriggereDebugLog;
            }
        }

        protected void OnDestroy()
        {
            foreach (var point in _points)
            {
                point.TriggeredEvent -= OnTrackPointTriggered;
            }

            if (_isPrintLog)
            {
                LapCompletedEvent -= LapCompletedDebugLog;
                TrackPointTriggeredEvent -= TrackPointTriggereDebugLog;
            }
        }

        private void OnTrackPointTriggered(TrackPoint point)
        {
            if (!point.IsTarget)
                return;

            point.MarkPassed();
            point.Next?.MarkAsTarget();

            TrackPointTriggeredEvent?.Invoke(point);

            if (point.IsLast)
            {
                _lapsCompleted++;

                if (_type == TrackTypeEnum.Sprint)
                    LapCompletedEvent?.Invoke(_lapsCompleted);

                if (_type == TrackTypeEnum.Circular)
                    if (_lapsCompleted > 0)
                        LapCompletedEvent?.Invoke(_lapsCompleted);
            }
        }

        [ContextMenu(nameof(BuildCircuit))]
        private void BuildCircuit()
        {
            _points = TrackCircuitBuilder.BuildCircuit(transform, _type);
        }
    }
}
