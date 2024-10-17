using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    [SerializeField] private GameObject holePos;

    public float maxHoleDropOffset;
    
    private float _stayTimer = 0;
    public float maxStayTimer;
    private bool _hasDropped = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.enabled && other.CompareTag("GolfBall"))
        {
            _stayTimer += Time.deltaTime;
        }
        
        Vector3 ballXYpos = new Vector3(other.transform.position.x, 0f, other.transform.position.z);
        Vector3 holeXYpos = new Vector3(other.transform.position.x, 0f, other.transform.position.z);
        if (Mathf.Abs(ballXYpos.x - holeXYpos.x) < maxHoleDropOffset &&
            Mathf.Abs(ballXYpos.y - holeXYpos.y) < maxHoleDropOffset &&
            (other.attachedRigidbody.velocity.magnitude < 1 || _stayTimer > maxStayTimer))
        {
            if (!_hasDropped)
            {
                other.transform.position = holePos.transform.position;
                other.attachedRigidbody.velocity = Vector3.zero;
                _hasDropped = true;
                // StartCoroutine(Game.instance.StartGame());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GolfBall"))
        {
          
        }
    }
}
