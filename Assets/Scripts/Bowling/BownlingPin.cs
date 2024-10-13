using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BownlingPin : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody _pin;
    private Boolean _isKnocked = false;
    public BowlingController bowlingController;
    
    private  Vector3 _originalPosition;
    private Quaternion _originalRotation;
    void Start()
    {
        _pin = GetComponent<Rigidbody>();
        _originalPosition = _pin.transform.position;
        _originalRotation = _pin.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isKnocked) return;
        if (_pin.transform.up.y < 1.0f )
        {
            bowlingController.KnockPin(_pin.GameObject());
            _isKnocked = true;
        }
    }

    public void Reset()
    {
        _pin.transform.position = _originalPosition;
        _pin.transform.rotation = _originalRotation;
        _pin.velocity = Vector3.zero;
        _pin.angularVelocity = Vector3.zero;
    }
}
