using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MenuHandler
    {
        private readonly Dictionary<MainMenuState, SubMenu> _map;
        private MainMenuState _state = MainMenuState.Main;
        private readonly GameObject _backButton;
        
        public MenuHandler(SubMenu[] menus, GameObject backButton)
        {
            _map = menus.ToDictionary(x => x.State);
            _backButton = backButton;
            
            InitializeButtons();
            SetActive(_state);
        }

        private void InitializeButtons()
        {
            foreach (SubMenu menu in _map.Values)
            {
                menu.Button.onClick.AddListener(() => SetActive(menu.State));
            }
        }

        private void SetActive(MainMenuState state)
        {
            foreach (var menu in _map.Values)
            {
                if (menu.State == state) continue;
                menu.Hide();
            }
            
            _map[state].Show();
            _state = state;
            
            SetBackButton();
        }

        private void SetBackButton()
        {
            _backButton.SetActive(_state != MainMenuState.Main);
            var button = _backButton.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => SetActive(_map[_state].GetBackButtonTarget()));
        }
    }
}