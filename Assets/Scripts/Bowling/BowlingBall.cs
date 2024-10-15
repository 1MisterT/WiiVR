using System.Collections;
using System.Collections.Generic;
using Bowling;
using UnityEngine;

public class BowlingBall : BasicResettable
{
    public Vector3 startSpeed;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Rigidbody.velocity = startSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
