using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;

public class BowlingController : MonoBehaviour
{
    // Start is called before the first frame update
    private int2 _pinCounter;
    private HashSet<GameObject> _knockedPins;
    void Start()
    {
        _pinCounter = 0;
        _knockedPins = new HashSet<GameObject>();
        StartCoroutine("Reset");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KnockPin(GameObject knockedPin)
    {
        _pinCounter += 1;
        _knockedPins.Add(knockedPin);
    }

    public void Reset()
    {
        foreach (var pin in _knockedPins)
        {
            pin.GetComponent<BownlingPin>().Reset();
        }
    }
}
