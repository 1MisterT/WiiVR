using UnityEngine;
namespace Golf
{
    public class RegisterBallHit2 : MonoBehaviour
    {
        public int hitCount = 0;
        public float cooldownTime = 0.2f; // cooldown in seconds
        private float _lastHitTime = -Mathf.Infinity;
        private bool _isHittable = true;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            // Update hittable status based on whether the ball is moving
            _isHittable = _rigidbody.velocity.magnitude < 0.2f;
        }

        private void OnCollisionEnter(Collision other)
        {
            // Check if hit is from golf club, cooldown has passed, and ball is hittable
            if (other.gameObject.CompareTag("GolfClubRoot") && _isHittable && Time.time > _lastHitTime + cooldownTime)
            {
                hitCount++;
                _lastHitTime = Time.time;
                _isHittable = false;  // Temporarily disable hit detection until the ball is stationary again
                Debug.Log("Golf ball hit! Hit count: " + hitCount);
            }
        }
    }
}

