using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GolfBallInteraction : MonoBehaviour
{
    private GameObject _golfBall;
    private GrabInteractableSetup _grabSetup; // Reference the script that we wanna use for this Monobehavior of the golf ball
    private Rigidbody _golfBallCollider;
    private int _counter;
    private string _currentLayerName;
    private string _targetLayerName; // this can be public and set in the inspector but it currently depends on the golf club and is read there
    void Start()
    {
        _golfBall = GameObject.FindGameObjectWithTag("GolfBall");
        _golfBallCollider = _golfBall.GetComponent<Rigidbody>();
        
        GameObject golfclub = GameObject.FindGameObjectWithTag("GolfClub");
        _grabSetup = golfclub.GetComponent<GrabInteractableSetup>();
        
        _targetLayerName = _grabSetup.targetLayerName;
        _currentLayerName = LayerMask.LayerToName(gameObject.layer);
    }

    void Update()
    {
        // check if golf ball is moving
        //if (_golfBallCollider.velocity.magnitude < 0.1f && _targetLayerName != _currentLayerName)
        //{
        //    OnStopMoving();
        //}
        //else if (_currentLayerName == _targetLayerName)
        //{
        //    OnMoving();
        //}
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GolfHole")
        {
            _golfBallCollider.detectCollisions = false;
            Debug.Log("GolfBall reached the hole. Needed hits: " + _counter);
        }
        
        if (other.tag == "GolfClub")
        { 
            _counter++;
            Debug.Log("GolfBall hit!");
            Debug.Log("Current hits: " + _counter);    
        }
    }

    // Method that makes the golf ball untouchable while it is moving
    void OnMoving()
    {
        int newLayerName = LayerMask.NameToLayer(_grabSetup.targetLayerName);
        _grabSetup.StoreOriginalLayers(transform);
        _grabSetup.SetLayerRecursively(gameObject, newLayerName);
    }

    void OnStopMoving()
    {
        // _grabSetup.RestoreOriginalLayers();
    }
}
