using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Monobehavior that sets up an object ready for use (e.g. golf club)
// contains event listener to check if the assigned Game Object is grabbed by the player
// toggle the isKinematic property then the object is grabbed for the first time
// references ChangeLayerName script
// while grabbing the object it clips through a targeted layer of objects to make it easier to use
// on release everthing is changed back to normal and the objects behaves like an usual Rigidbody
namespace Golf
{
    public class GrabInteractableSetup : MonoBehaviour
    {
        private Rigidbody _rb;

        private XRGrabInteractable _grabInteract;
        
        // reference the GolfGameManager to start the game efficiently
        private GolfGameManager _golfGameManager;
        
        // reference the DisplayHitScore to enable and disable the GolfClub UI
        private DisplayHitScore _displayHitScore;

        // reference the ChangeLayerName script
        private ChangeLayerName _changeLayerName;

        // needed to commpare with targetLayerName
        private string _currentLayerName;

        // provided in the inspector of ChangeLayerName
        private string _targetLayerName;
        

        // public get but private set in order to provide the state of the interactable to trigger events when it is grabbed
        // initially false until it is grabbed
        public bool isGrabbed { get; private set; }
        

        void Start()
        {
            _golfGameManager = FindObjectOfType<GolfGameManager>();
            _displayHitScore = GameObject.Find("GolfClubCanvas").GetComponent<DisplayHitScore>();
            
            _rb = GetComponent<Rigidbody>();
            
            _grabInteract = gameObject.GetComponent<XRGrabInteractable>(); // get the XR Grab Interactable component
            
            _changeLayerName = gameObject.GetComponent<ChangeLayerName>(); // get the ChangeLayerName component to use its methods
            
            EnableKinematic(); // Enable isKinematic so the objects floats before you grab it for the first time
            
            isGrabbed = false;

            // create event listener when interactable is grabbed
            _grabInteract.selectEntered.AddListener(OnGrab);
            _grabInteract.selectExited.AddListener(OnRelease);

            // get the layer of the game object
            _currentLayerName = LayerMask.LayerToName(gameObject.layer);
            _targetLayerName = _changeLayerName.targetLayerName;
        }

        // change layer names of parent and child objects
        // set isGrabbed to true
        // the golf club also starts the golf game so we need to access the GolfGameManager to change the gameStarted property
        void OnGrab(SelectEnterEventArgs args)
        {
            Debug.Log(gameObject.name + " is grabbed, changed parent and child layer names to " + _targetLayerName);
            isGrabbed = true;
            _displayHitScore.EnableUI();
            _displayHitScore.DisplayHitCount();
            
            if (!_golfGameManager.gameStarted)
            {
                _golfGameManager.StartGame();
            }
            
            if (_rb.isKinematic)
            {
                DisableKinematic();
            }

            if (_currentLayerName != _targetLayerName)
            {
                _changeLayerName.StoreOriginalLayers(transform);

                // set layer names of parents and all its child objects
                _changeLayerName.SetLayerRecursively(gameObject);
            }
        }

        // change back the original layer names 
        // set isGrabbed ro false
        void OnRelease(SelectExitEventArgs args)
        {
            Debug.Log(gameObject.name + " is released, changed parent and child layer name back to: " + _currentLayerName);
            isGrabbed = false;
            _displayHitScore.DisableUI();
            _displayHitScore.DisplayHitCount();
            
            if (_currentLayerName != _targetLayerName)
            {
                // restore the previous layer names when grab interactable is released
                _changeLayerName.RestoreOriginalLayers();
            }

            if (_rb.isKinematic)
            {
                DisableKinematic();
            }
        }

        void OnDestroy()
        {
            _grabInteract.selectEntered.RemoveListener(OnGrab);
            _grabInteract.selectExited.RemoveListener(OnRelease);
        }

        public void DisableKinematic()
        {
            _rb.isKinematic = false;
        }

        public void EnableKinematic()
        {
            _rb.isKinematic = true;
        }
    }
}
