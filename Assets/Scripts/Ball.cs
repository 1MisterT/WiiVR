using System.Security.Cryptography;
using UnityEngine;

// script to determine some properties of a ball
// can be read and overwritten by other scripts
public class Ball : MonoBehaviour
{
    public bool isAthole;
    public bool isIdle;
    public bool isMoving;
    public bool wasMoving;
    public bool isHitable;
    private Rigidbody _rigidbody;
    private Collider _collider;
    public Color hitColor;
    public Color moveColor;
    private float _hitCooldown = 0.5f;
    private float _lastHitTime;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

  
    void FixedUpdate()
    {
        TrackMovement();
        
        // Reset hitability based on cooldown
        if (Time.time - _lastHitTime > _hitCooldown && !isMoving)
        {
            isHitable = true;
        }
        else
        {
            isHitable = false;
        }
    }

    private void TrackMovement()
    {
        bool isCurrentlyMoving = _rigidbody.velocity.magnitude >= 0.01f;
        
        // check if the ball is moving 
        if (_rigidbody.velocity.magnitude >= 0.01f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        
        // check if the state has changed 
        if (isCurrentlyMoving != wasMoving)
        {
            if (isCurrentlyMoving)
            {
                Debug.Log(name + " is moving");
                isMoving = true;
                isIdle = false;
            }
            else
            {
                Debug.Log(name + " stopped moving");
                isIdle = true;
                isMoving = false;
            }
        }
        
        
        // Update the previous state 
        wasMoving = isCurrentlyMoving;
    }
    // method to mark the ball as hit
    public void RegisterHit()
    {
        _lastHitTime = Time.time; // start cooldown
        isHitable = false; // Make the ball un-hitable during cooldown
    }

    // method that determines the color of the ball depending on if it is moving or idle
    private void ChangeColor()
    {
        if (isHitable)
        {
            
        }
    }

    private void DisableCollider()
    {
        if (isAthole)
        {
            _collider.enabled = false;
        }
    }
}
