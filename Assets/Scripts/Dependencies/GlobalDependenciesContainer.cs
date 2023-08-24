using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SF3DRacing
{
    public class GlobalDependenciesContainer : Dependency
    {
        [SerializeField]
        private Pauser _pauser;

        [SerializeField]
        private MenuSfx _menuSfx;

        private static GlobalDependenciesContainer _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        protected override void BindAll(MonoBehaviour monoBehInScene)
        {
            Bind<Pauser>(_pauser, monoBehInScene);
            Bind<MenuSfx>(_menuSfx, monoBehInScene);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            FindObjectsToBind();
        }
    }
}
