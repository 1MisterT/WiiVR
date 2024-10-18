using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Golf
{
    public class HoleVortexScript : MonoBehaviour
    {
        private GameObject _golfball;
        private GameObject _golfhole;
        private Rigidbody _golfrb;

        void Start()
        {
            _golfball = GameObject.FindGameObjectWithTag("GolfBall");
            _golfhole = GameObject.FindGameObjectWithTag("GolfHole");
            _golfrb = _golfball.GetComponent<Rigidbody>();

        }

        void OnTriggerEnter(Collider other)
        {
            Vector3 currentVelocity = _golfrb.velocity;
            if (other.gameObject.tag == "GolfBall" && currentVelocity != Vector3.zero)
            {
                Debug.Log("Golfball entered VortexTrigger");
                Vector3 directionToTargetCenter = (_golfhole.transform.position - other.transform.position).normalized;
                float dotProduct = Vector3.Dot(transform.forward, directionToTargetCenter);

                if (dotProduct < 0.9f)
                {
                    float angle = 45f;
                    float force = _golfrb.velocity.x;
                    float forceAmount = 1f;

                    Quaternion rotation = Quaternion.Euler(0, angle, 0);
                    Vector3 angleDirection = rotation * currentVelocity.normalized;
                    Vector3 forceVector = angleDirection * forceAmount;

                    _golfrb.AddForce(forceVector, ForceMode.Impulse);

                    Debug.Log("Moving Golfball");
                }

            }
        }
    }
}
