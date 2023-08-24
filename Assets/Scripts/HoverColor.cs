using UnityEngine;
using UnityEngine.UI;

namespace SF3DRacing
{
    public class HoverColor : MonoBehaviour, IDependency<MenuSfx>
    {
        public Button button;
        public Color hoverColor;

        public Color _defaultColor;

        private MenuSfx _menuSfx;

        private void Start()
        {
            OnLeave();
        }

        public void OnEnter()
        {
            button.image.color = hoverColor;
            _menuSfx.PlayHoverSound();
        }

        public void OnLeave()
        {
            button.image.color = _defaultColor;
        }

        public void Construct(MenuSfx dependency)
        {
            _menuSfx = dependency;
        }
    }
}
