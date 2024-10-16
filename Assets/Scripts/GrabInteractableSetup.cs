using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Monobehavior that sets up an object ready for use (e.g. golf club)
// toggle the isKinematic property then the object is grabbed for the first time
// while grabbing the object it clips through a targeted layer of objects to make it easier to use
// on release everthing is changed back to normal and the objects behaves like an usual Rigidbody
public class GrabInteractableSetup : MonoBehaviour
{
    private Rigidbody _rb;
    private XRGrabInteractable _grabInteract;
    
    // this must be assigned in the inspector of the object that carries this script -> e.g. PassThrough
    // maybe you need to check the layer collsion matrix in 'Edit > Project Settings > Physics' (e.g. if you do not want to collide with yout target layer)
    // an additional option is excluding your target layer in the object which should clip through your target layer
    public  string targetLayerName;  
    private string _currentLayerName; // get the layer name of the game object
    private Dictionary<Transform, int> originalLayers = new Dictionary<Transform, int>();
    void Start()
    {
      if (name == "GolfClub")
      {
          Debug.Log("Golf Club Setup");
      }
      
      _rb = GetComponent<Rigidbody>();
      _grabInteract = GetComponent<XRGrabInteractable>(); // get the XR Grab Interactable component
      
      EnableKinematic(); // Enable isKinematic so the objects floats before you grab it for the first time
      
      // create event listener when interactable is grabbed
      _grabInteract.selectEntered.AddListener(OnGrab);
      _grabInteract.selectExited.AddListener(OnRelease);
      
      // get the layer of the game object
      _currentLayerName = LayerMask.LayerToName(gameObject.layer);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log(gameObject.name + " is grabbed, changed parent and child layer names to " + targetLayerName);
        if (_rb.isKinematic)
        {
            DisableKinematic();
        }
        
        int newLayerName = LayerMask.NameToLayer(targetLayerName);
        if (_currentLayerName != targetLayerName)
        {
            StoreOriginalLayers(transform);
            
            // set layer names of parents and all its child objects
            SetLayerRecursively(gameObject, newLayerName);
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        Debug.Log(gameObject.name + " is released, changed parent and child layer name back to: " + _currentLayerName);
        if (_currentLayerName != targetLayerName)
        {
            // restore the previous layer names when grab interactable is released
            RestoreOriginalLayers();
        }

        if (_rb.isKinematic)
        {
            DisableKinematic();
        }
    }

    void OnDestroy()
    {
        _grabInteract.selectEntered.RemoveListener(OnGrab);
        _grabInteract.selectExited.RemoveListener(OnRelease);
    }

    void DisableKinematic()
    {
        _rb.isKinematic = false;
    }

    void EnableKinematic()
    {
        _rb.isKinematic = true;
    }

    public void StoreOriginalLayers(Transform obj)
    {
        if (obj == null)
            return;

        // Store the original layer in the dictionary
        if (!originalLayers.ContainsKey(obj))
        {
            originalLayers[obj] = obj.gameObject.layer;
        }

        // Recursively store layers for all child objects
        foreach (Transform child in obj)
        {
            StoreOriginalLayers(child);
        }
    }
    
    public void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null)
            return;

        // Set the layer for the current object
        obj.layer = newLayer;

        // Recursively set the layer for each child object
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public void RestoreOriginalLayers()
    {
        foreach (var kvp in originalLayers)
        {
            if (kvp.Key != null)
            {
                kvp.Key.gameObject.layer = kvp.Value;  // Restore original layer
            }
        }

        // Clear the dictionary after restoring
        originalLayers.Clear();
    }
}
