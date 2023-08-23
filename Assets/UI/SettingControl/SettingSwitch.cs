using UnityEngine;
using UnityEngine.UIElements;

namespace SF3DRacing
{
    public class SettingSwitch : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<SettingSwitch> { }

        private Label _settingName => this.Q<Label>("setting-name");
        private Label _settingValue => this.Q<Label>("setting-value");
        private VisualElement _prev => this.Q<VisualElement>("prev");
        private VisualElement _next => this.Q<VisualElement>("next");
        private VisualElement _row => this.Q<VisualElement>("setting-row");

        private SettingBase _setting;
        private MenuSfx _sfx;

        public SettingSwitch() { }

        public void Init(SettingBase setting, MenuSfx sfx)
        {
            _setting = setting;
            _sfx = sfx;

            _settingName.text = setting.Title;
            _settingValue.text = setting.GetStringValue();

            _prev.RegisterCallback<ClickEvent>(OnPrev);
            _next.RegisterCallback<ClickEvent>(OnNext);

            _row.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
        }

        private void OnMouseEnter(MouseEnterEvent e)
        {
            _sfx.PlaySound(MenuActions.Hover);
        }

        private void OnPrev(ClickEvent e)
        {
            _sfx.PlaySound(MenuActions.SettingChange);

            _setting.SetPrevValue();
            _setting.Apply();

            _settingValue.text = _setting.GetStringValue();
        }

        private void OnNext(ClickEvent e)
        {
            _sfx.PlaySound(MenuActions.SettingChange);

            _setting.SetNextValue();
            _setting.Apply();

            _settingValue.text = _setting.GetStringValue();
        }
    }
}
