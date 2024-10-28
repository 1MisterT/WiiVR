using Unity.XR.CoreUtils;
using UnityEngine;


// script assigned to ball and handles interaction with its surroundings
// the GolfBall game object and its Ball component are absolutely necessary
namespace Golf
{ 
    public class GolfBallInteraction : MonoBehaviour
    {
        private GameObject _golfBallRoot;
        private GameObject _golfBallMesh;
        private Renderer _golfBallRenderer;
     

        public Material moveMaterial; // reference wanted material for the golf ball in the inspector
        public Material stopMaterial;

        private ChangeLayerName _changeLayerName;
        private Ball _ball;
        private Rigidbody _golfBallRigidbody;
        private RegisterBallHit _registerHit;

        private int _counter;

        void Start()
        {
            _golfBallRoot = GameObject.FindWithTag("GolfBallRoot");
            _golfBallMesh = _golfBallRoot.GetNamedChild("GolfBallMesh");
            
            _golfBallRigidbody = _golfBallRoot.GetComponent<Rigidbody>();
            _golfBallRenderer = _golfBallMesh.GetComponent<Renderer>();
            _changeLayerName = _golfBallRoot.GetComponent<ChangeLayerName>();
            _ball = _golfBallRoot.GetComponent<Ball>();
            
            _registerHit = gameObject.GetComponent<RegisterBallHit>();
        }

        void FixedUpdate()
        {
          if (!_ball.isMoving || _ball.isHitable)
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
                //_changeLayerName.targetLayerName = "PassThroughPlane";
                //_changeLayerName.SetLayerRecursively(gameObject);
                _ball.isInhole = true;
                Debug.Log("GolfBall reached the hole. Needed hits: " +  _ball.hitCount);
            }

            if (other.CompareTag("GolfClubPart") && _ball.isMoving)
            {
                _ball.isHitable = false;
            }
        }

        // register the hit and count the hits
        private void OnCollisionEnter(Collision other)
        {
           if (other.gameObject.CompareTag("GolfClubRoot") && _ball.isHitable) 
           {
               _registerHit.RegisterHitLog();
           }
        }

        // Method that makes the golf ball untouchable while it is moving
        void OnMoving()
        {
            MakeUntouchable();
        }

        void OnStopMoving()
        {
           MakeTouchable();
        }

        private void MakeUntouchable()
        {
            _changeLayerName.targetLayerName = "IgnoreHitLayer"; // Problem: Conflict with layer name change when ball reaches the golf hole (fixed when using real 3D golf hole)
            _changeLayerName.StoreOriginalLayers(transform);
            _changeLayerName.SetLayerRecursively(gameObject);
            _golfBallRenderer.material = moveMaterial;
        }

        private void MakeTouchable()
        {
            _changeLayerName.RestoreOriginalLayers();
            _golfBallRenderer.material = stopMaterial;
        }
    }
}
