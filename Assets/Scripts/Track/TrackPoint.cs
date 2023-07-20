using UnityEngine;
using UnityEngine.Events;

namespace SF3DRacing
{
    public class TrackPoint : MonoBehaviour
    {
        public event UnityAction<TrackPoint> TriggeredEvent;

        public TrackPoint Next;
        public bool IsFirst;
        public bool IsLast;

        protected bool _isTarget;
        public bool IsTarget => _isTarget;

        protected void OnTriggerEnter(Collider other)
        {
            if (!other.transform.root.GetComponent<Car>())
                return;

            TriggeredEvent?.Invoke(this);
        }

        protected virtual void OnMarkedPassed() { }
        protected virtual void OnMarkedAsTarget() { }

        public void MarkPassed()
        {
            _isTarget = false;
            OnMarkedPassed();
        }

        public void MarkAsTarget()
        {
            _isTarget = true;
            OnMarkedAsTarget();
        }

        public void Reset()
        {
            Next = null;
            IsFirst = false;
            IsLast = false;
        }
    }
}
