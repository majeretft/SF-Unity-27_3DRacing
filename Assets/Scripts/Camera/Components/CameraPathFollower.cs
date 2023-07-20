using UnityEngine;

namespace SF3DRacing
{
    public class CameraPathFollower : CameraComponent
    {
        [SerializeField] private Transform _path;
        [SerializeField] private Transform _lookTarget;
        [SerializeField] private float _moveSpeed = 1.0f;

        private Vector3[] _points;
        private int _index;

        protected void Start()
        {
            _points = new Vector3[_path.childCount];
            for (int i = 0; i < _path.childCount; i++)
            {
                _points[i] = _path.GetChild(i).position;
            }
            _index = 0;

            transform.position = _points[_index];
            transform.LookAt(_lookTarget);
        }

        protected void Update()
        {
            if (_index < _points.Length)
            {
                transform.position = Vector3.MoveTowards(transform.position, _points[_index], _moveSpeed * Time.deltaTime);
                if (transform.position == _points[_index])
                {
                    _index++;
                }
            }
            else
            {
                _index = 0;
            }

            transform.LookAt(_lookTarget);
        }

        public void SetLookTarget(Transform target)
        {
            _lookTarget = target;
        }

        public void StartMoveToNearestPoint()
        {
            var shortestDistance = Mathf.Infinity;
            for (var i = 0; i < _points.Length; i++)
            {
                var distance = Vector3.Distance(transform.position, _points[i]);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    _index = i;
                }
            }
        }
    }
}
