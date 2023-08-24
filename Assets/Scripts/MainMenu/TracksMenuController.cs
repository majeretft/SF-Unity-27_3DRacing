using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace SF3DRacing
{
    [Serializable]
    public class SceneMap {
        public string TrackName;
        public string SceneName;
    }

    public class TracksMenuController : MonoBehaviour, IDependency<MenuSfx>
    {
        [SerializeField]
        private UIDocument _uIDocumentRoot;

        [SerializeField]
        private MenuSfx _sfx;

        [SerializeField]
        private List<SceneMap> _sceneMap;

        private void Start()
        {
            var menuSection = _uIDocumentRoot.rootVisualElement.Q<VisualElement>("TracksMenu");

            var menuCards = menuSection
                .Q<VisualElement>("track-wrapper")
                .Query()
                .Children<VisualElement>("track-container")
                .ToList();

            foreach (var obj in menuCards)
                obj.RegisterCallback<MouseEnterEvent>(OnMouseEnterEvent);

            var menuButtons = menuSection
                .Q<VisualElement>("track-wrapper")
                .Query("track-button")
                .ToList();

            foreach (var obj in menuButtons)
                obj.RegisterCallback<ClickEvent>(OnMouseClickEvent);
        }

        public void Construct(MenuSfx dependency)
        {
            _sfx = dependency;
        }

        private void OnMouseClickEvent(ClickEvent e)
        {
            if (e.target is not Button button)
                return;

            _sfx.PlaySound(MenuActions.ButtonClick);

            var prop = button.GetClasses().Where(x => x != "track-button" && x.StartsWith("track-")).FirstOrDefault();
            if (string.IsNullOrEmpty(prop))
                return;

            var scene = _sceneMap.FirstOrDefault(x => x.TrackName == prop);
            if (scene == null)
                return;

            SceneManager.LoadScene(scene.SceneName);
        }

        private void OnMouseEnterEvent(MouseEnterEvent e)
        {
            _sfx.PlaySound(MenuActions.Hover);
        }
    }
}
