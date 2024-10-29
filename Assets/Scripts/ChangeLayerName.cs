using System.Collections.Generic;
using UnityEngine;

public class ChangeLayerName : MonoBehaviour
{
    // this must be assigned in the inspector of the object that carries this script -> e.g. PassThroughPlane
    // maybe you need to check the layer collsion matrix in 'Edit > Project Settings > Physics' (e.g. if you do not want to collide with yout target layer)
    // an additional option is excluding your target layer in the object which should clip through your target layer
    public  string targetLayerName;  
    private string _currentLayerName; // get the layer name of the game object
    private Dictionary<Transform, int> _originalLayers = new Dictionary<Transform, int>();
    

    public void StoreOriginalLayers(Transform obj)
    {
        if (obj == null)
            return;

        // Store the original layer in the dictionary
        if (!_originalLayers.ContainsKey(obj))
        {
            _originalLayers[obj] = obj.gameObject.layer;
        }

        // Recursively store layers for all child objects
        foreach (Transform child in obj)
        {
            StoreOriginalLayers(child);
        }
    }
    
    // when we call this method with a different script it is not needed to pass a targeted layer name because it must be given in the inspector
    public void SetLayerRecursively(GameObject obj) // Idea: maybe just pass the targetLayerName as a variable to adjust demanded layer name for different objects
    {
        if (obj == null)
            return;

        // Set the layer for the current object
        int newLayer = LayerMask.NameToLayer(targetLayerName);
        obj.layer = newLayer;

        // Recursively set the layer for each child object
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject);
        }
    }

    public void RestoreOriginalLayers()
    {
        foreach (var kvp in _originalLayers)
        {
            if (kvp.Key != null)
            {
                kvp.Key.gameObject.layer = kvp.Value;  // Restore original layer
            }
        }

        // Clear the dictionary after restoring
        _originalLayers.Clear();
    }
}

