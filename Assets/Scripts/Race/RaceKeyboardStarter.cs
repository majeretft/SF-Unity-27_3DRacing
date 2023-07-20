using UnityEngine;

namespace SF3DRacing
{
    public class RaceKeyboardStarter : MonoBehaviour
    {
        [SerializeField] private RaceStateTracker _tracker;

        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
                _tracker.StartPreparation();
        }
    }
}
