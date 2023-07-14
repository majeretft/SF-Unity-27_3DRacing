#if UNITY_EDITOR
using UnityEngine;

namespace SF3DRacing
{
    public class EditorSettings : MonoBehaviour
    {
        protected void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
    }
}

#endif

