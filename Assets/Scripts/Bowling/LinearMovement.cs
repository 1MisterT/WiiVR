using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Bowling
{
    public class LinearMovement : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        
        public Vector3 direction;
        
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.attachedRigidbody is not null)
            {
                _rigidbody = collider.attachedRigidbody;
                _rigidbody.angularVelocity = Vector3.zero;
                _rigidbody.velocity = direction;
            }
        }

        // private void OnTriggerStay(Collider collider)
        // {
        //     if (collider.attachedRigidbody is not null)
        //     {
        //         _rigidbody = collider.attachedRigidbody;
        //         _rigidbody.velocity = direction;
        //         
        //     }
        // }
    }
}