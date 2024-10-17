using Unity.Collections;
using Unity.XR.CoreUtils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GolfBallInteraction : MonoBehaviour
{
    private GameObject _golfBall;
    private GameObject _golfBallMesh;
    private Renderer _golfBallRenderer;
    public Material moveMaterial; // reference wanted material for the golf ball in the inspector
    public Material stopMaterial;
    private ChangeLayerName _changeLayerName;
    private Rigidbody _golfBallCollider;
    private int _counter;
    private string _currentLayerName;
    private string _targetLayerName; 
    void Start()
    {
        _golfBall = GameObject.FindWithTag("GolfBall");
        _golfBallMesh = _golfBall.GetNamedChild("GolfBallMesh");
        _golfBallCollider = _golfBall.GetComponent<Rigidbody>();
        _golfBallRenderer = _golfBallMesh.GetComponent<Renderer>();
        _changeLayerName = gameObject.GetComponent<ChangeLayerName>();
        
        if (_changeLayerName != null)
        {
            _targetLayerName = _changeLayerName.targetLayerName;
            _currentLayerName = LayerMask.LayerToName(gameObject.layer);
        }
        else
        {
            Debug.LogError("Could not find an Gameobject with the GrabInteractableSetup-component");
        }
        
    }

    void Update()
    {
        // check if golf ball is moving
       if (_golfBallCollider.velocity.magnitude < 0.05f)
       {
           OnStopMoving();
       }
       else
       {
           OnMoving();
       }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GolfHole"))
        {
            _golfBallCollider.detectCollisions = false;
            Debug.Log("GolfBall reached the hole. Needed hits: " + _counter);
        }
        
        if (other.CompareTag("GolfClub")) // ToDo: need to make a check which automatically takes the game object and its child objects and read their tags
        { 
            _counter++;
            Debug.Log("GolfBall hit!");
            Debug.Log("Current hits: " + _counter);    
        }
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
