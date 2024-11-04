using Menu.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Menu
{
    public class PauseMenuController : MonoBehaviour
    {
        private const string ControllerName = "LeftHandController";
        [SerializeField] private InputActionReference toggleButton;
        
        private XRController _controller;
        private GameObject _pMenu;
        
        private void Start()
        {
            var lController = GameObject.Find(ControllerName);
            _pMenu = GameObject.Find("PauseMenu");
            _pMenu.transform.SetParent(lController.transform, new Vector3(0.25f, 0, 0.5f));
            _pMenu.SetActive(false);

            toggleButton.action.performed += _ =>
            {
                Toggle(!_pMenu.activeSelf);
            };

            TeleportAction.OnTeleported += () =>
            {
                if (_pMenu.activeSelf) Toggle(false);
            };
        }

        private void Toggle(bool state)
        {
            _pMenu.SetActive(state);
            OnPauseMenuToggled?.Invoke(state);
        }

        public delegate void PauseMenuEventArgs(bool opened);

        public static event PauseMenuEventArgs OnPauseMenuToggled;
    }
}