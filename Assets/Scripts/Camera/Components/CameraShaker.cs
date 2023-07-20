using UnityEngine;

namespace SF3DRacing
{
    public class CameraShaker : CameraComponent
    {
        [SerializeField, Range(0, 1)] private float _normalizedSpeedShake;
        [SerializeField] private float _shakeAmount;

        protected void Update()
        {
            if (_car.LinearVelocityNormalized >= _normalizedSpeedShake)
                transform.localPosition += Random.insideUnitSphere * _shakeAmount * Time.deltaTime;
        }
    }
}
