using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Golf
{
    public class RegisterBallHit : MonoBehaviour
    {
        private float _hitCooldown = 0.1f; // 0.1 seconds cooldown
        private float _lastHitTime;
        public int hitCounter{ get; private set; }

    private Ball _ball;


        // Method that sets cooldown after the ball was hit
        private bool CanRegisterHit()
        {
            _ball = GameObject.FindGameObjectWithTag("GolfBall").GetComponent<Ball>();
            _ball.isHitable = true;
            return Time.time - _lastHitTime > _hitCooldown;
        }

        // Method that is called when the ball experiences a collision
        // Logging the registered hit
        public void RegisterHitLog()
        {
            if (CanRegisterHit() && _ball.isHitable && !_ball.hitRegisteredThisFrame)
            {
                _ball.isHitable = false;
                _ball.hitRegisteredThisFrame = true;
                _lastHitTime = Time.time; // start cooldown
                hitCounter++; // Increment hit counter
                Debug.Log("Golf ball hit!");
                Debug.Log("Current amount of hits: " + hitCounter);
            }
        }
    }
}
