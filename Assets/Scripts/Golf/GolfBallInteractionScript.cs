using System;
using Unity.Collections;
using Unity.XR.CoreUtils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Golf
{ 
    public class GolfBallInteraction : MonoBehaviour
    {
        private GameObject _golfBall;
        private GameObject _golfBallMesh;
        private Renderer _golfBallRenderer;

        public Material moveMaterial; // reference wanted material for the golf ball in the inspector
        public Material stopMaterial;

        private ChangeLayerName _changeLayerName;
        private Ball _ball;
        private Rigidbody _golfBallCollider;
        private RegisterBallHit _registerHit;

        private int _counter;

        void Start()
        {
            _golfBall = GameObject.FindWithTag("GolfBall");
            _golfBallMesh = _golfBall.GetNamedChild("GolfBallMesh");
            
            _golfBallCollider = _golfBall.GetComponent<Rigidbody>();
            _golfBallRenderer = _golfBallMesh.GetComponent<Renderer>();
            _changeLayerName = gameObject.GetComponent<ChangeLayerName>();
            _ball = _golfBall.GetComponent<Ball>();
            
            _registerHit = gameObject.GetComponent<RegisterBallHit>();
        }

        void FixedUpdate()
        {
            // check if golf ball is moving
            if (!_ball.isMoving)
            {
                OnStopMoving();
            }
            else
            {
                OnMoving();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("GolfHole"))
            {
                _golfBallCollider.detectCollisions = false;
                _ball.isInhole = true;
                Debug.Log("GolfBall reached the hole. Needed hits: " +  _registerHit.hitCounter);
            }

            if (other.CompareTag("GolfClub") && _ball.isMoving || _ball.isIdle)
            {
                _ball.isHitable = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("GolfClub") && _ball.isHitable) // ToDo: need to make a check which automatically takes the game object and its child objects and read their tags
            {
                _registerHit.RegisterHitLog();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _changeLayerName.StoreOriginalLayers(transform);
            _changeLayerName.SetLayerRecursively(gameObject);
        }

        // Method that makes the golf ball untouchable while it is moving
        void OnMoving()
        {
            _changeLayerName.StoreOriginalLayers(transform);
            _changeLayerName.SetLayerRecursively(gameObject);
            _golfBallRenderer.material = moveMaterial;
        }

        void OnStopMoving()
        {
            _changeLayerName.RestoreOriginalLayers();
            _golfBallRenderer.material = stopMaterial;
        }
    }
}
