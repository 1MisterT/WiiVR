using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Golf
{
    public class GolfGameManager : MonoBehaviour
    {
        // get every necessary object of the golf scene
        private List <GameObject> _golfObjectList;
        private List <GameObject> _golfHoleList;
        private List <GameObject> _golfPlayerUI;
        
        private GameObject _golfHole;
        private GameObject _activeGolfHole;
        
        private GameObject _golfClub;
        private GameObject _golfPlayer;
        private GameObject _golfBall;
        
        private List <XRGrabInteractable> _grabInteractableList;
        private XRGrabInteractable _grabInteractableGolfClub;
        
        private GrabInteractableSetup _grabInteractableSetup;

        public bool gameStarted {get; private set;} = false;

        void Start()
        {
            _golfObjectList = FindObjectsWithTagContaining("Golf");
            foreach (var obj in _golfObjectList)
            {
                Debug.Log(obj.name);
            }
            _grabInteractableSetup = FindObjectOfType<GrabInteractableSetup>();
            
            _golfPlayerUI = FindObjectsWithTagContaining("PlayerUI");
            foreach (var obj in _golfPlayerUI)
            {
                obj.SetActive(false);
            }
        }

        private void FixedUpdate() // just for testing --> need to make event listeners for more efficiency
        {
            if (_grabInteractableSetup.isGrabbed && !gameStarted)
            {
                StartGame();
            }
        }
        
        // ToDo: Trying to link an Interface to RespawnObjects to create a respawn manager
        public interface IRespawn
        {
            void RespawnGameObject();
            void RespawnGameObjectParent();
        }

        private void StartGame()
        {
            gameStarted = true;
            EnablePlayerUI(_golfPlayerUI);
            Debug.Log("Welcome to the Golf Game!");
        }

        private void EndGame()
        {
            gameStarted = false;
        }
        
        // method to get every objects that returns all active Game Objects in the scene and filters them by their Tag if they contain a certain fragment of the Tag name
        List<GameObject> FindObjectsWithTagContaining(string keyword)
        {
            List<GameObject> matchingObjects = new List<GameObject>();

            foreach (GameObject obj in FindObjectsOfType<GameObject>())
            {
                if (obj.CompareTag(keyword) || obj.tag.Contains(keyword))
                {
                    matchingObjects.Add(obj);
                }
            }
            return matchingObjects;
        }

        private void EnablePlayerUI(List<GameObject> playerUI)
        {
            foreach (var obj in playerUI) 
            {
                obj.SetActive(true);
            }
        }

        private void DisablePlayerUI(List<GameObject> playerUI)
        {
            foreach (var obj in playerUI)
            {
                obj.SetActive(false);
            }
        }
        
        private void EnablePlayerUISpecifically(GameObject playerUI)
        {
            playerUI.SetActive(true);
        }
        
        private void DisablePlayerUiSpecifically(GameObject playerUI)
        {
            playerUI.SetActive(false);
        }
       
    }
}
