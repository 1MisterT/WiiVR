using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    // Start is called before the first frame update
    private HashSet<GameObject> _rolledBalls;
    void Start()
    {
        _rolledBalls = new HashSet<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BallRolled(GameObject ball)
    {
        var rollingBall = ball.GetComponent<BowlingBall>();
        if (rollingBall != null)
        {
            _rolledBalls.Add(ball);
        }
        else
        {
            Debug.Log("wrong object passed" + ball.name.ToString());
        }
    }
}
