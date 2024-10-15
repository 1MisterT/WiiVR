using System;
using System.Collections;
using System.Collections.Generic;
using Bowling;
using UnityEngine;
using UnityEngine.Serialization;

public class HightLimit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<BasicResettable>()?.Reset();
        
    }
}
