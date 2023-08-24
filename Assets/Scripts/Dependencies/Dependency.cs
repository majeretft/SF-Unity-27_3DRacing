using UnityEngine;

namespace SF3DRacing
{
    public abstract class Dependency : MonoBehaviour
    {
        protected virtual void BindAll(MonoBehaviour monoBehInScene) { }

        protected virtual void Bind<T>(MonoBehaviour bindObject, MonoBehaviour target) where T : class
        {
            if (target is IDependency<T>)
                (target as IDependency<T>).Construct(bindObject as T);
        }

        protected void FindObjectsToBind()
        {
            var monoBehs = FindObjectsOfType<MonoBehaviour>();

            foreach (var monoBeh in monoBehs)
            {
                BindAll(monoBeh);
            }
        }
    }
}
