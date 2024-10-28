using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Monobehavior that sets up an object ready for use (e.g. golf club)
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

        // reference the ChangeLayerName script
        private ChangeLayerName _changeLayerName;

        // needed to commpare with targetLayerName
        private string _currentLayerName;

        // provided in the inspector of ChangeLayerName
        private string _targetLayerName;

        void Start()
        {
            if (gameObject.name == "GolfClub")
            {
                Debug.Log("Initializing Golf Club Setup");
            }

            _rb = GetComponent<Rigidbody>();
            _grabInteract = gameObject.GetComponent<XRGrabInteractable>(); // get the XR Grab Interactable component
            _changeLayerName =
                gameObject.GetComponent<ChangeLayerName>(); // get the ChangeLayerName component to use its methods
            EnableKinematic(); // Enable isKinematic so the objects floats before you grab it for the first time

            // create event listener when interactable is grabbed
            _grabInteract.selectEntered.AddListener(OnGrab);
            _grabInteract.selectExited.AddListener(OnRelease);

            // get the layer of the game object
            _currentLayerName = LayerMask.LayerToName(gameObject.layer);
            _targetLayerName = _changeLayerName.targetLayerName;
        }

        void OnGrab(SelectEnterEventArgs args)
        {
            Debug.Log(gameObject.name + " is grabbed, changed parent and child layer names to " + _targetLayerName);
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

        void OnRelease(SelectExitEventArgs args)
        {
            Debug.Log(gameObject.name + " is released, changed parent and child layer name back to: " +
                      _currentLayerName);
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
