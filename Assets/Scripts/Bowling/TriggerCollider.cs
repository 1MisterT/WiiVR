using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TriggerCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public BowlingController bowlingController;
    public bool triggerTurn = false;
    private Rigidbody _activeBall;
    void Start()
    {
        if (bowlingController == null)
        {
            bowlingController = Component.FindObjectOfType<BowlingController>();
        }

        bowlingController.softResetEvent += (sender, args) =>
        {
            _activeBall = null;
        };
        bowlingController.hardResetEvent += (sender, args) =>
        {
            _activeBall = null;
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_activeBall && other.attachedRigidbody && other.attachedRigidbody != _activeBall)
        {
            other.attachedRigidbody.velocity = Vector3.zero;
            other.attachedRigidbody.angularVelocity = Vector3.zero;
            return;
        }
        _activeBall = other.gameObject.GetComponent<Rigidbody>();
        if (triggerTurn) bowlingController.BallRolled(other.gameObject);
    }
}
