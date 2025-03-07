using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Golf
{
    public class HybridInteractable : MonoBehaviour
    {
        private XRGrabInteractable _interactable;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("GolfBallPart"))
            {
                SetVelocityTracking();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("GolfBallPart"))
            {
                SetKinematic();
            }
        }
    
        // Function to change the movement type to Instantaneous
        public void SetInstantaneous()
        {
            _interactable.movementType = XRBaseInteractable.MovementType.Instantaneous;
        }

        // Function to change the movement type to Kinematic
        public void SetKinematic()
        {
            _interactable.movementType = XRBaseInteractable.MovementType.Kinematic;
        }

        // Function to change the movement type to Velocity Tracking
        public void SetVelocityTracking()
        {
            _interactable.movementType = XRBaseInteractable.MovementType.VelocityTracking;
        }

        // Example of toggling movement type based on a condition or input
        public void ToggleMovementType()
        {
            if (_interactable.movementType == XRBaseInteractable.MovementType.Kinematic)
            {
                SetVelocityTracking();
            }
            else
            {
                SetKinematic();
            }
        }
        private void Start()
        {
            _interactable = GetComponent<XRGrabInteractable>();
        }
    }
}
