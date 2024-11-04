using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class SubMenu
    {
        private readonly GameObject _menuObject;
        private readonly GameObject _buttonObject;
        internal Button Button => _buttonObject.GetComponent<Button>();
        private readonly MainMenuState _backButtonTarget;
        public MainMenuState State { get; }
        
        public SubMenu(MainMenuState state, GameObject menu, GameObject button, MainMenuState? backButtonTarget = null)
        {
            State = state;
            _menuObject = menu;
            _buttonObject = button;
            _backButtonTarget = backButtonTarget ?? MainMenuState.Main;
        }

        public void Show()
        {
            SetVisible(true);
        }

        public void Hide()
        {
            SetVisible(false);
        }

        private void SetVisible(bool visible)
        {
            _menuObject.SetActive(visible);
        }

        public MainMenuState GetBackButtonTarget() => _backButtonTarget;
    }
}