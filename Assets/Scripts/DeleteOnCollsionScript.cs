using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteScript : MonoBehaviour
{
    // delete all game objects that hit the script object
    // add tags for specific objects only
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
        }
        
        Destroy(other.gameObject);
        Debug.Log(other.gameObject.name + " got deleted");
    }
}
