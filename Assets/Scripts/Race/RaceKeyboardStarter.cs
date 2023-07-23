using UnityEngine;

namespace SF3DRacing
{
    public class RaceKeyboardStarter : MonoBehaviour, IDependency<RaceStateTracker>
    {
        private RaceStateTracker _tracker;

        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
                _tracker.StartPreparation();
        }

        public void Construct(RaceStateTracker dependency)
        {
            _tracker = dependency;
        }
    }
}
