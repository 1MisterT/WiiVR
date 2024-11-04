using Basics;
using UnityEngine;

/* Copyright (C) Tom Troeger */

namespace Bowling
{
    public class HeightLimit : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.GetComponent<BasicResettable>()?.Reset();
        
        }
    }
}
