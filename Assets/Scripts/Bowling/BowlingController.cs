using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class BowlingController : MonoBehaviour
{
    // Start is called before the first frame update
    private HashSet<GameObject> _knockedPins = new HashSet<GameObject>();
    public InputActionReference hardResetAction;
    public InputActionReference softResetAction;

    public int pinCounter
    {
        get => _knockedPins.Count;
    } 

    void Start()
    {
        _knockedPins = new HashSet<GameObject>();
        hardResetAction.action.performed += context => HardReset();
        softResetAction.action.performed += context => SoftReset();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void KnockPin(GameObject knockedPin)
    {
        _knockedPins.Add(knockedPin);
    }

    private void HardReset()
    {
        foreach (var pin in GetComponentsInChildren<BownlingPin>(includeInactive: true))
        {
            pin.Reset();
        }
        _knockedPins.Clear();
    }

    private void SoftReset()
    {
        foreach (var pin in _knockedPins)
        {
            pin.SetActive(false);
        }
    }
    
    
}
