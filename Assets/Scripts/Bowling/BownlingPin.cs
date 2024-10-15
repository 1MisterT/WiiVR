using System;
using System.Collections;
using System.Collections.Generic;
using Bowling;
using Unity.VisualScripting;
using UnityEngine;

public class BownlingPin : BasicResettable
{
    // Start is called before the first frame update
    private Boolean _isKnocked = false;
    public BowlingController bowlingController;

    protected override void Start()
    {
        base.Start();
        if (bowlingController == null)
        {
            bowlingController = Component.FindObjectOfType<BowlingController>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isKnocked) return;
        if (Rigidbody.transform.up.y < 0.5f )
        {
            bowlingController.KnockPin(gameObject);
            _isKnocked = true;
        }
    }

    public override void Reset()
    {
        base.Reset();
        _isKnocked = false;
    }
}
