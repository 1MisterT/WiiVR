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
        private Rigidbody _golfBallRigidBody;
        private Renderer _golfBallRenderer;
     
        private GolfGameManager _golfGameManager;

        private GolfHole _activeGolfHole;

        public Material moveMaterial; // reference wanted material for the golf ball in the inspector
        public Material stopMaterial;

        private ChangeLayerName _changeLayerName;
        private Ball _ball;
        private RegisterBallHit _registerHit;

        [SerializeField] private AudioClip winSound;

        private int _counter;

        void Start()
        {
            _golfBallRoot = GameObject.FindWithTag("GolfBallRoot");
            _golfBallRigidBody = _golfBallRoot.GetComponent<Rigidbody>();
            _golfBallMesh = _golfBallRoot.GetNamedChild("GolfBallMesh");
            _golfBallRenderer = _golfBallMesh.GetComponent<Renderer>();
            
            _ball = _golfBallRoot.GetComponent<Ball>();
            
            _golfGameManager = GameObject.Find("GolfGameManager").GetComponent<GolfGameManager>();
            
            _changeLayerName = _golfBallRoot.GetComponent<ChangeLayerName>();
            _registerHit = gameObject.GetComponent<RegisterBallHit>();
        }

        
        // register the hit and count the hits
        private void OnCollisionEnter(Collision other)
        {
           if (other.gameObject.CompareTag("GolfClubRoot") && _ball.isHitable) 
           {
               _registerHit.RegisterHitLog();
           }

           if (other.gameObject.CompareTag("GolfHole"))
           {
                _ball.isInhole = true;
                SoundFXManager.Instance.PlaySoundFX(winSound, gameObject.transform);
               _golfGameManager.RestartGame();
           }
        }

        void FixedUpdate()
        {
            MakeTouchable();
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
            _changeLayerName.targetLayerName = "IgnoreHitLayer"; 
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
