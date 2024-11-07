using Golf;
using Unity.XR.CoreUtils;
using UnityEngine;

public class RespawnObjects : MonoBehaviour
{
    public GameObject respawnPoint;
    
    private GameObject _golfBall;
    private Transform _golfBallRoot; // reference the root object of golf ball to adress all its child objects when teleporting it later
    private ChangeLayerName _golfBallChangeLayer;
    private GameObject _golfBallRespawnPoint;

    public GameObject setGameObject;
    public GameObject setRespawnPoint;
    
    private GameObject _golfClub;
    private Transform _golfClubRoot; // the same process as _golfBallRoot
    private GameObject _golfClubRespawnPoint;
    
    private Transform _collidingGameObjectParent;
    private GameObject _collidingGameObject;

    private GrabInteractableSetup _grabInteractableSetup;
    
    private SoundFXManager _soundFXManager;

    private void Start()
    {
        _soundFXManager = SoundFXManager.Instance;
        _golfBall = GameObject.Find("GolfBall");
        _golfBallChangeLayer = _golfBall.GetComponent<ChangeLayerName>();
        
        _golfClub = GameObject.Find("GolfClub");
        if (_golfClub == null)
        {
            _golfClub = GameObject.Find("SimpleGolfClub");
        }
       
        _grabInteractableSetup = _golfClub.GetComponent<GrabInteractableSetup>();
        
        // specifically get the parent/root objects of the game objects to transform them later on
        _golfClubRoot = _golfClub.GetComponent<Transform>();
        _golfBallRoot = _golfBall.GetComponent<Transform>();
        
        
        if (_golfBallRespawnPoint == null)
        {
            _golfBallRespawnPoint = GameObject.Find("GolfBallRespawnArea").GetNamedChild("GolfBallRespawnPoint");

            if (respawnPoint == null)
            {
                Debug.LogError("No respawn point for respawning objects set in inspector nor could find one in the scene");
            }
        }
        
        
        if (_golfClubRespawnPoint == null)
        {
            _golfClubRespawnPoint = GameObject.Find("GolfClubRespawnArea").GetNamedChild("GolfClubSpawnPoint");

            if (_golfClubRespawnPoint == null)
            {
                Debug.LogError("No respawn point for respawning objects set in inspector nor could find one in the scene");
            }
        }
    }

    // When an object collides with a collider with which this script is assigned to
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GolfClubPart") || other.CompareTag("GolfClubRoot")) // Adjust the first one to the tag 'GolfClubPart' of any child objects of the golf club that might collide. 'GolfClubRoot' is meant for the parent object
        {
           RespawnGolfCLub();
        } 
        else if (other.CompareTag("GolfBallPart") || other.CompareTag("GolfBallRoot"))
        {
           RespawnGolfBall();
        } 
        else
        {
            Transform parentTransform = other.transform.parent;
            
            if (parentTransform != null)
            {
                _collidingGameObjectParent = other.transform.parent;
                RespawnGameObjectParent(_collidingGameObjectParent, respawnPoint);
            } 
            else if (other.gameObject != null)
            {
                _collidingGameObject = other.gameObject;
                //RespawnGameObject(_collidingGameObject);
            } 
            else
            {
                Debug.LogWarning("No object to respawn");
            }
        }
    }

    public void RespawnGolfCLub()
    {
        _soundFXManager.PlaySoundFX(_soundFXManager.uiSound, gameObject.transform);
        // Check if golfClubRoot is specifically the golf club (or adjust to use GolfClubRoot directly)
        if (_golfClubRoot.CompareTag("GolfClubRoot"))
        {
            _golfClubRoot.position = _golfClubRespawnPoint.transform.position; // Move the entire golf club to the respawn point
            // make object kinematic and set rotation to 0 so you can grab it properly
            _grabInteractableSetup.EnableKinematic();
            if (_golfClubRoot.name == "GolfClub")
            {
                _golfClubRoot.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                _golfClubRoot.rotation = Quaternion.Euler(-90, 180, 0);
            }
            
            Debug.Log("Golf Club respawned at " + _golfClubRespawnPoint.name);
        }
    }

    public void RespawnGolfBall()
    {
        _soundFXManager.PlaySoundFX(_soundFXManager.uiSound, gameObject.transform);
        if (_golfBallRoot.CompareTag("GolfBallRoot"))
        {
            _golfBallChangeLayer.targetLayerName = "Default";
            _golfBallChangeLayer.SetLayerRecursively(_golfBall);
            _golfBallRoot.position = _golfBallRespawnPoint.transform.position;
            Debug.Log("GolfBall respawned at " + _golfBallRespawnPoint.name);
        }
    }

    // Respawn any object or its parent
    // in order to determine if it is just a simple Game Object or a Parent object we need to check them before teleporting them to a set respawn point
    public void RespawnGameObject(GameObject teleportThisGameObject, GameObject customRespawnPoint)
    {
        teleportThisGameObject.transform.position = customRespawnPoint.transform.position;
        Debug.Log(teleportThisGameObject.name + " respawned at " + customRespawnPoint.name);
    }

    public void RespawnGameObjectParent(Transform teleportThisGameObjectTransform, GameObject customRespawnPoint)
    {
        teleportThisGameObjectTransform.position = customRespawnPoint.transform.position;
        Debug.Log(teleportThisGameObjectTransform.name + " respawned at " + customRespawnPoint.name);
    }
    
    
    // testing a method that can be invoced by interacting with a button
    // Problem: Cannot be invoced if it has parameters, so we have to set public Game Objects of the class in the inspector of the button
    public void RespawnGameObjectTest()
    {
        setGameObject.transform.position = setGameObject.transform.position;
        Debug.Log(setGameObject.name + " respawned at " + setGameObject.name);
    }
}
