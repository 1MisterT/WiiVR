
using System.Collections;
using UnityEngine;

namespace Golf
{
    public class Ball : MonoBehaviour
    {
        // public property will be set by other scripts and handles the logic of disabling the collision collider when the ball is in the hole
        private bool _isInHole;
        public bool isInhole {get => _isInHole; set { _isInHole = value; }
        }

        private bool _hitRegisteredThisFrame;
        public bool hitRegisteredThisFrame {get => _hitRegisteredThisFrame; set => _hitRegisteredThisFrame = value; }
        
        private bool _isHitable; 
        public bool isHitable { get => _isHitable; set { _isHitable = value; } }
        

        public bool isIdle { get; private set; }
        public bool isMoving { get; private set; }
        public bool wasMoving { get; private set; }
        

        private Rigidbody _rigidbody;
        private Collider[] _colliderList;
        private Collider _collisionCollider;
        private Coroutine _movementCheckCoroutine;

        private float _hitCooldown = 0.1f; // 0.1-second cooldown before ball can be hit again
        private float _lastHitTime;

        private int _hitCount;
        public int hitCount { get => _hitCount; set { _hitCount = value; } }

        private void Start()
        {
            _movementCheckCoroutine = StartCoroutine(CheckMovementCoroutine());
            _rigidbody = GetComponent<Rigidbody>();
            if (gameObject.GetComponent<Collider>() != null && !gameObject.GetComponent<Collider>().isTrigger)
            {
                _collisionCollider = gameObject.GetComponent<Collider>();
            }
            else
            {
                _colliderList = GetComponentsInChildren<Collider>();
                FindCollisionCollider(_colliderList);
            }
            
            _hitCount = 0;

        }
        
        public bool canHit => !isMoving && Time.time - _lastHitTime > _hitCooldown;
        private void FixedUpdate()
        {
            // Reset the flag at the beginning of each frame
            _hitRegisteredThisFrame = false;
            
            // Ball can be hit only if it's not moving and cooldown has passed
            isHitable = !isMoving && canHit;
        }

        // checking movement at fixed intervals instead of every single frame
        // event driven updates
        // preventing overlap and false hits
        // This kinda fixes the issue of the ball-hit-registration being very inconsisent and makes the hit detection more reliable (still not 100% accurate registration)
        // need to be careful with this
        private IEnumerator CheckMovementCoroutine()
        {
            // in this context it is okay to use while (true) because we are using a Coroutine in Unity in combination with yield statements
            // We just have to make sure that the Coroutine lifecycle no longer exists when it is not needed
            while (true)
            {
                // Wait for a specified interval
                yield return new WaitForSeconds(0.5f); // Check every half second

                TrackMovement(); // Call your movement check logic here
            }
        }
        
        private void TrackMovement()
        {
            // Check if the ball is moving based on its velocity magnitude
            isMoving = _rigidbody.velocity.magnitude >= 0.02f;

            // Check if the movement state has changed
            if (isMoving != wasMoving)
            {
                isIdle = !isMoving;
                Debug.Log($@"{name} {(isMoving ? "is moving" : "stopped moving")}");
            }

            // Update the previous state for the next frame
            wasMoving = isMoving;
        }
        
        private void StopTrackMovement()
        {
            if (_movementCheckCoroutine != null)
            {
                StopCoroutine(_movementCheckCoroutine);
                _movementCheckCoroutine = null;
            }
        }
        
        private void FindCollisionCollider(Collider[] colliderList)
        {
            foreach (Collider col in colliderList)
            {
                if (!col.isTrigger)
                {
                    _collisionCollider = col;
                    break;
                }
            }
        }

        private void DisableCollider(Collider collider)
        {
            collider.enabled = false;
        }

        private void EnableCollider(Collider collider)
        {
            collider.enabled = true;
        }

        private void OnDestroy()
        {
            StopTrackMovement();
        }
    }
}