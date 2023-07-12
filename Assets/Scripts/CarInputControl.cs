using UnityEngine;

namespace SF3DRacing
{
    public class CarInputControl : MonoBehaviour
    {
        [SerializeField] private Car _car;

        protected void Update()
        {
            _car.BrakeControl = Input.GetAxis("Jump");
            _car.SteerControl = Input.GetAxis("Horizontal");
            _car.ThrottleControl = Input.GetAxis("Vertical");
        }
    }
}
