using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Menu
{
    public class MenuController : MonoBehaviour
    {
        private MenuHandler _handler;

        [SerializeField] private SubMenuStruct[] subMenus;
        [SerializeField] private GameObject backButton;
        [Serializable]
        public struct SubMenuStruct
        {
            public MainMenuState state;
            public GameObject menu;
            public GameObject button;
            public MainMenuState backButtonTarget;
        }
        

        // Start is called before the first frame update
        void Start()
        {
            _handler = new(subMenus.Select(x => new SubMenu(x.state, x.menu, x.button, x.backButtonTarget)).ToArray(),
                backButton);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void QuitGame() => Application.Quit();
    }
}