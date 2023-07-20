using UnityEngine;
using UnityEngine.Events;

namespace SF3DRacing
{
    public class Timer : MonoBehaviour
    {
        public event UnityAction FinishedEvent;
        [SerializeField] private float _time;
        private float _currentTime;
        public float CurrentTime => _currentTime;

        protected void Start()
        {
            _currentTime = _time;
        }

        protected void Update()
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime <= 0)
            {
                enabled = false;
                FinishedEvent?.Invoke();
            }
        }
    }
}
