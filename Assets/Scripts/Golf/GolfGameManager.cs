using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Golf
{
    public class GolfGameManager : MonoBehaviour
    {
        // get every necessary object of the golf scene
        // we need to make sure, that this script runs before any other script that accesses the objects within this manager
        // in order to do this, we need to configure the Script Execution Order
        // Edit > Project Settings > Scrip Execution Order
        private List <GameObject> _golfObjectList;
        private List<GameObject> _golfHoleList;
        private List <GameObject> _golfPlayerUI;
        
        private GameObject _startingGolfHole;
        private GameObject _golfHole;
        public GolfHole activeGolfHole { get; private set; }
        
        public GameObject golfClub { get; private set; }
        public GameObject golfPlayer { get; private set; }
        public GameObject golfBall { get; private set; }
        private Ball _ball;
        
        private List <XRGrabInteractable> _grabInteractableList;
        private XRGrabInteractable _grabInteractableGolfClub;
        private GrabInteractableSetup _grabInteractableSetup;
        
        private RespawnObjects _respawnObjects;

        public bool gameStarted { get; private set; }

        void Start()
        {
            _golfObjectList = FindObjectsWithTagContaining("Golf");
            
            _grabInteractableSetup = FindObjectOfType<GrabInteractableSetup>();
            
            _golfPlayerUI = FindObjectsWithTagContaining("PlayerUI");
            
            golfBall = GameObject.Find("GolfBall");
            golfClub = GameObject.Find("RealisticGolfClub");
            
            // find the golf holes and store them in a list
            _golfHoleList = _golfObjectList.Where(golfHole => golfHole.name.Contains("GolfHole")).ToList();
            
            _respawnObjects = FindObjectOfType<RespawnObjects>();
            
            // Set the first golf hole
            InitializeStartingGolfHole();
            
            DisableGolfObject(golfBall);
            DisablePlayerUIList(_golfPlayerUI);
        }

        // Game can be started by grabbing the GolfClub
        // for reference: GrabInteractableSetup
        public void StartGame()
        {
            gameStarted = true;
            EnablePlayerUIList(_golfPlayerUI);
            EnableGolfObject(golfBall);
            Debug.Log("Welcome to the Golf Game!");
        }

        // Ends the game
        // implement on a UI button or when the GolfBall GameObject is in the GolfHole
        public void EndGame()
        {
            gameStarted = false;
            DisablePlayerUIList(_golfPlayerUI);
            _ball.hitCount = 0;
            DisableGolfObject(golfBall);
            
            Debug.Log("Game Over!");
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

        public void EnablePlayerUIList(List<GameObject> playerUI)
        {
            foreach (var obj in playerUI) 
            {
                obj.SetActive(true);
            }
        }

        public void DisablePlayerUIList(List<GameObject> playerUI)
        {
            foreach (var obj in playerUI)
            {
                obj.SetActive(false);
            }
        }
        
        public void EnablePlayerUIGameObject(GameObject playerUI)
        {
            playerUI.SetActive(true);
        }
        
        public void DisablePlayerUIGameObject(GameObject playerUI)
        {
            playerUI.SetActive(false);
        }

        public void EnableGolfObject(GameObject golfgameObject)
        {
            if (golfgameObject.name == "GolfBall")
            {
                _respawnObjects.RespawnGolfBall();
            }
            golfgameObject.SetActive(true);
        }

        public void DisableGolfObject(GameObject golfGameObject)
        {
            golfGameObject.SetActive(false);
        }


        private void InitializeStartingGolfHole()
        {
            // start with the first golf hole
            _startingGolfHole = _golfHoleList.SingleOrDefault(golfHole => golfHole.name ==  "GolfHole1");
        }
        private  void SetActiveGolfHole(GameObject golfHole)
        {
           
        }

        private void FindActiveGolfHole()
        {
            
        }

        public void NextField()
        {
            
        }
    }
}
