using UnityEngine;

namespace SF3DRacing
{
    [RequireComponent(typeof(AudioSource))]
    public class WheelEffects : MonoBehaviour
    {
        [SerializeField] private WheelCollider[] _wheels;
        [SerializeField] private ParticleSystem[] _wheelSmokes;

        [SerializeField] private float _forwardSlipLimit;
        [SerializeField] private float _sideSlipLimit;

        [SerializeField] private GameObject _skidPrefab;


        private AudioSource _skidSound;
        private WheelHit hit;
        private Transform[] _skidTrails;

        protected void Start()
        {
            _skidTrails = new Transform[_wheels.Length];
            _skidSound = GetComponent<AudioSource>();
        }

        protected void Update()
        {
            bool isSkid = false;

            for (var i = 0; i < _wheels.Length; i++)
            {
                _wheels[i].GetGroundHit(out hit);

                if (!_wheels[i].isGrounded || (Mathf.Abs(hit.forwardSlip) <= _forwardSlipLimit && Mathf.Abs(hit.sidewaysSlip) <= _sideSlipLimit))
                {
                    _skidTrails[i] = null;
                    _wheelSmokes[i].Stop();
                    continue;
                }

                if (!_skidSound.isPlaying)
                    _skidSound.Play();

                if (!_skidTrails[i])
                    _skidTrails[i] = Instantiate(_skidPrefab).transform;

                if (_skidTrails[i])
                {
                    _skidTrails[i].position = hit.point + new Vector3(0, 0.01f, 0); // _wheels[i].transform.position - hit.normal * _wheels[i].radius;
                    _skidTrails[i].forward = -hit.normal;

                    _wheelSmokes[i].transform.position = _skidTrails[i].position;
                    _wheelSmokes[i].Emit(1);

                    isSkid = true;
                }
            }

            if (!isSkid)
                _skidSound.Stop();

        }
    }
}
