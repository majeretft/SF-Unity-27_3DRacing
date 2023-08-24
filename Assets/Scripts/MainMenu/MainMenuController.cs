using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SF3DRacing
{
    public class MainMenuController : MonoBehaviour, IDependency<MenuSfx>
    {
        [SerializeField]
        private UIDocument _uIDocument;

        [SerializeField]
        private MenuSfx _sfx;

        private List<VisualElement> _innerMenus = new();
        private List<Button> _mainButtons = new();

        private void Start()
        {
            var tracksMenuBtn = _uIDocument.rootVisualElement.Q<Button>("tracks-button", "menu-button");
            var settingsMenuBtn = _uIDocument.rootVisualElement.Q<Button>("settings-button", "menu-button");
            var aboutMenuBtn = _uIDocument.rootVisualElement.Q<Button>("about-button", "menu-button");
            var quitMenuBtn = _uIDocument.rootVisualElement.Q<Button>("quit-button", "menu-button");

            tracksMenuBtn.RegisterCallback<ClickEvent>(OnMainMenuButtonClicked);
            settingsMenuBtn.RegisterCallback<ClickEvent>(OnMainMenuButtonClicked);
            aboutMenuBtn.RegisterCallback<ClickEvent>(OnMainMenuButtonClicked);
            quitMenuBtn.RegisterCallback<ClickEvent>(OnMainMenuButtonClicked);

            tracksMenuBtn.RegisterCallback<MouseEnterEvent>(OnMouseEnterEvent);
            settingsMenuBtn.RegisterCallback<MouseEnterEvent>(OnMouseEnterEvent);
            aboutMenuBtn.RegisterCallback<MouseEnterEvent>(OnMouseEnterEvent);
            quitMenuBtn.RegisterCallback<MouseEnterEvent>(OnMouseEnterEvent);

            _mainButtons.Add(tracksMenuBtn);
            _mainButtons.Add(settingsMenuBtn);
            _mainButtons.Add(aboutMenuBtn);
            _mainButtons.Add(quitMenuBtn);

            _innerMenus = _uIDocument.rootVisualElement
                .Q<VisualElement>("inner-menu-container")
                .Query()
                .Children<VisualElement>()
                .ToList();
        }

        public void Construct(MenuSfx dependency)
        {
            _sfx = dependency;
        }

        private void OnMouseEnterEvent(MouseEnterEvent e)
        {
            _sfx.PlaySound(MenuActions.Hover);
        }

        private void OnMainMenuButtonClicked(ClickEvent e)
        {
            if (e.target is not Button el)
                return;

            foreach (var menu in _innerMenus)
                menu.RemoveFromClassList("in");

            foreach (var menu in _mainButtons)
                menu.RemoveFromClassList("active");

            _sfx.PlaySound(MenuActions.ButtonClick);
            
            switch (el.name)
            {
                case "tracks-button":
                    el.AddToClassList("active");
                    _innerMenus.Find(x => x.name == "TracksMenu").AddToClassList("in");
                    break;
                case "settings-button":
                    el.AddToClassList("active");
                    _innerMenus.Find(x => x.name == "SettingsMenu").AddToClassList("in");
                    break;
                case "about-button":
                    el.AddToClassList("active");
                    _innerMenus.Find(x => x.name == "AboutMenu").AddToClassList("in");
                    break;
                case "quit-button":
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                    break;
                default:
                    throw new System.Exception("Unknown button name");
            }
        }
    }
}
