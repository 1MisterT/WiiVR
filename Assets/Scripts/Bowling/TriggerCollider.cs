using UnityEngine;

namespace Bowling
{
    public class TriggerCollider : MonoBehaviour
    {
        // Start is called before the first frame update
        private BowlingController _bowlingController;
        public bool triggerTurn = false;
        private Rigidbody _activeBall;
        void Start()
        {
            _bowlingController = BowlingController.instance;
            _bowlingController.softResetEvent += (sender, args) =>
            {
                _activeBall = null;
            };
            _bowlingController.hardResetEvent += (sender, args) =>
            {
                _activeBall = null;
            };
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_activeBall && other.attachedRigidbody && other.attachedRigidbody != _activeBall)
            {
                other.attachedRigidbody.velocity = Vector3.zero;
                other.attachedRigidbody.angularVelocity = Vector3.zero;
                return;
            }
            _activeBall = other.gameObject.GetComponent<Rigidbody>();
            if (triggerTurn) _bowlingController.BallRolled(other.gameObject);
        }
    }
}
