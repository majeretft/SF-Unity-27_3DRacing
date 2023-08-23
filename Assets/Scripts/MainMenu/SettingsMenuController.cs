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

        [SerializeField]
        private List<SettingBase> _settingItems;

        private void Start()
        {
            var menuSection = _uIDocumentRoot.rootVisualElement.Q<VisualElement>("SettingsMenu");
            var wrapper = menuSection.Q<VisualElement>("settings-wrapper");

            // var menuRows = wrapper
            //     .Query()
            //     .Children<VisualElement>("setting-row")
            //     .ToList();

            // foreach (var obj in menuRows)
            //     obj.RegisterCallback<MouseEnterEvent>(OnMouseEnterEvent);

            // var menuButtons = wrapper
            //     .Query()
            //     .Children<VisualElement>("track-image")
            //     .ToList();

            // foreach (var obj in menuButtons)
            //     obj.RegisterCallback<ClickEvent>(OnMouseClickEvent);

            var resetButton = wrapper.Q("reset-saves-button");
            resetButton.RegisterCallback<ClickEvent>(OnMouseClickEvent);
            resetButton.RegisterCallback<MouseEnterEvent>(OnMouseEnterEvent);

            foreach (var item in _settingItems)
            {
                item.Load();
                var template = item.GuiTemplate;
                var templateContainer = template.Instantiate();
                var settingSwitch = templateContainer.Q<SettingSwitch>();
                wrapper.Insert(wrapper.childCount - 1, settingSwitch);
                settingSwitch.Init(item, _sfx);
            }
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
