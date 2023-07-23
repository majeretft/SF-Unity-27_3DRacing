using UnityEngine;

namespace SF3DRacing
{
    public class TrackPointActivator : TrackPoint
    {
        [SerializeField] private GameObject _hint;

        protected void Start()
        {
            _hint.SetActive(IsTarget);
        }

        protected override void OnMarkedAsTarget()
        {
            base.OnMarkedAsTarget();

            _hint.SetActive(true);
        }

        protected override void OnMarkedPassed()
        {
            base.OnMarkedPassed();

            _hint.SetActive(false);
        }
    }
}
