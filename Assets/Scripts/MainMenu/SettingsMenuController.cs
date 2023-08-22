using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SF3DRacing
{
    public class SettingsMenuController : MonoBehaviour
    {
        [SerializeField]
        private UIDocument _uIDocumentRoot;

        [SerializeField]
        private MenuSfx _sfx;

        private void Start()
        {
            var menuSection = _uIDocumentRoot.rootVisualElement.Q<VisualElement>("SettingsMenu");

            var menuRows = menuSection
                .Q<VisualElement>("settings-wrapper")
                .Query()
                .Children<VisualElement>("setting-row")
                .ToList();

            foreach (var obj in menuRows)
                obj.RegisterCallback<MouseEnterEvent>(OnMouseEnterEvent);

            var menuButtons = menuSection
                .Q<VisualElement>("settings-wrapper")
                .Query()
                .Children<VisualElement>("track-image")
                .ToList();

            foreach (var obj in menuButtons)
                obj.RegisterCallback<ClickEvent>(OnMouseClickEvent);

            menuSection
                .Q<VisualElement>("settings-wrapper")
                .Q("reset-saves-button")
                .RegisterCallback<ClickEvent>(OnMouseClickEvent);
        }

        private void OnMouseClickEvent(ClickEvent e)
        {
            _sfx.PlaySound(MenuActions.SettingChange);
        }

        private void OnMouseEnterEvent(MouseEnterEvent e)
        {
            _sfx.PlaySound(MenuActions.Hover);
        }
    }
}
