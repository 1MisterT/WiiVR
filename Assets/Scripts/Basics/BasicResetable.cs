using UnityEngine;

namespace Basics
{
    public class BasicResettable : MonoBehaviour
    {
        private  Vector3 _originalPosition;
        private Quaternion _originalRotation;
        protected Rigidbody Rigidbody;
        
        protected virtual void Start()
        {
            _originalPosition = gameObject.transform.position;
            _originalRotation = gameObject.transform.rotation;
            Rigidbody = GetComponent<Rigidbody>();
        }
        
        public virtual void Reset()
        {   
            gameObject.transform.position = _originalPosition;
            gameObject.transform.rotation = _originalRotation;
            if (Rigidbody)
            {
                Rigidbody.velocity = Vector3.zero;
                Rigidbody.angularVelocity = Vector3.zero;
            }
            gameObject.SetActive(true);
        }

    }
}