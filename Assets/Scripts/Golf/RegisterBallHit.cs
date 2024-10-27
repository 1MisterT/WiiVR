using UnityEngine;

namespace Golf
{
    public class RegisterBallHit : MonoBehaviour
    {
        private float _hitCooldown = 1f; // 1 second cooldown
        private float _lastHitTime;
        
        private Ball _ball;

        private void Start()
        {
            _ball = gameObject.GetComponent<Ball>();
        }
        
        // Method that sets cooldown after the ball was hit
        private bool CanRegisterHit()
        {
            _ball.isHitable = true;
            return Time.time - _lastHitTime > _hitCooldown;
        }

        // Method that is called when the ball experiences a collision
        // Logging the registered hit
        public void RegisterHitLog()
        {
            if (CanRegisterHit() && _ball.isHitable && !_ball.hitRegisteredThisFrame)
            {
               // _ball.isHitable = false;
                _ball.hitRegisteredThisFrame = true;
                _lastHitTime = Time.time; // start cooldown
                _ball.hitCount++; // Increment hit counter
                Debug.Log("Golf ball hit!");
                Debug.Log("Current amount of hits: " + _ball.hitCount);
            }
        }
    }
}
