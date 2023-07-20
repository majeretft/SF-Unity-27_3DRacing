using UnityEngine;

namespace SF3DRacing
{
    [RequireComponent(typeof(CameraController))]
    public abstract class CameraComponent : MonoBehaviour
    {
        protected Car _car;
        protected Camera _camera;

        public virtual void SetProperties(Car car, Camera camera)
        {
            _car = car;
            _camera = camera;
        }
    }
}
