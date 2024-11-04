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
        
        private void Start()
        {
            var lController = GameObject.Find(ControllerName);
            var pMenu = GameObject.Find("PauseMenu");
            pMenu.transform.SetParent(lController.transform, new Vector3(0.25f, 0, 0.5f));
            pMenu.SetActive(false);

            toggleButton.action.performed += context =>
            {
                pMenu.SetActive(!pMenu.activeSelf);
            };

            TeleportAction.OnTeleported += () =>
            {
                if (pMenu.activeSelf) pMenu.SetActive(false);
            };
        }
    }
}