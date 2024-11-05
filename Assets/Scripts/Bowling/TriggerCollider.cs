using UnityEngine;

/* Copyright (C) Tom Troeger */

namespace Bowling
{
    public class TriggerCollider : MonoBehaviour
    {
        // Start is called before the first frame update
        private BowlingController _bowlingController;
        public bool triggerTurn;
        private Rigidbody _activeBall;
        void Start()
        {
            _bowlingController = BowlingController.Instance;
            _bowlingController.SoftResetEvent += (_, _) =>
            {
                _activeBall = null;
            };
            _bowlingController.HardResetEvent += (_, _) =>
            {
                _activeBall = null;
            };
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
            _bowlingController.BallRolled(other.gameObject, triggerTurn);
        }
    }
}
