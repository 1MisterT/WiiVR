using Golf;
using Unity.XR.CoreUtils;
using UnityEngine;

public class RespawnObjects : MonoBehaviour
{
    public GameObject respawnPoint;
    
    private GameObject _golfBall;
    private Transform _golfBallRoot; // reference the root object of golf ball to adress all its child objects when teleporting it later
    private ChangeLayerName _golfBallChangeLayer;
    
    private GameObject _golfClub;
    private Transform _golfClubRoot; // the same process as _golfBallRoot
    private ChangeLayerName _golfClubChangeLayer;

    private GrabInteractableSetup _grabInteractableSetup;

    private void Start()
    {
        _golfBall = GameObject.Find("GolfBall");
        _golfBallChangeLayer = _golfBall.GetComponent<ChangeLayerName>();
        
        _golfClub = GameObject.Find("GolfClub");
        _golfClubChangeLayer = _golfClub.GetComponent<ChangeLayerName>();
       
        _grabInteractableSetup = _golfClub.GetComponent<GrabInteractableSetup>();
        
        // specifically get the parent/root objects of the game objects to transform them later on
        _golfClubRoot = _golfClub.GetComponent<Transform>();
        _golfBallRoot = _golfBall.GetComponent<Transform>();
        
        
        if (respawnPoint == null)
        {
            respawnPoint = GameObject.Find("RespawnArea").GetNamedChild("RespawnPoint");

            if (respawnPoint == null)
            {
                Debug.LogError("No respawn point for respawning objects set in inspector nor could find one in the scene");
            }
        }
    }

    // When an object collides with a collider with which this script is assigned to
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GolfClubPartxxx") || other.CompareTag("GolfClubRootxxx")) // Adjust the first one to the tag 'GolfClubPart' of any child objects of the golf club that might collide. 'GolfClubRoot' is meant for the parent object
        {
           RespawnGolfCLub();
        } else if (other.CompareTag("GolfBallPartxxx") || other.CompareTag("GolfBallRootxxx"))
        {
           RespawnGolfBall();
        }
        else
        {
            GameObject collidingObject = other.gameObject;
            RespawnObject(collidingObject);
        }
    }

    public void RespawnGolfCLub()
    {
        // Check if golfClubRoot is specifically the golf club (or adjust to use GolfClubRoot directly)
        if (_golfClubRoot.CompareTag("GolfClubRoot"))
        {
            _golfClubRoot.position = respawnPoint.transform.position; // Move the entire golf club to the respawn point
            // make object kinematic and set rotation to 0 so you can grab it properly
            _grabInteractableSetup.EnableKinematic();
            _golfClubRoot.rotation = Quaternion.Euler(0, 0, 0);
            Debug.Log("Golf Club respawned at " + respawnPoint.name);
        }
    }

    public void RespawnGolfBall()
    {
        if (_golfBallRoot.CompareTag("GolfBallRoot"))
        {
            _golfBallChangeLayer.targetLayerName = "Default";
            _golfBallChangeLayer.SetLayerRecursively(_golfBall);
            _golfBallRoot.position = respawnPoint.transform.position;
            Debug.Log("GolfBall respawned at " + respawnPoint.name);
        }
    }

    public void RespawnObject(GameObject teleportThisGameObject)
    {
        Transform teleportTransform = teleportThisGameObject.transform;
        teleportTransform.position = respawnPoint.transform.position;
        Debug.Log(teleportThisGameObject.name + " respawned at " + teleportTransform.name);
    }
}
