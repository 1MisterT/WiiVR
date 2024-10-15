using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Bowling;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class BowlingController : MonoBehaviour
{
    // Start is called before the first frame update
    private HashSet<GameObject> _knockedPins = new HashSet<GameObject>();
    private BowlingBall _activeBall;
    private TimerScript _timer;
    private bool _secondShot = true;
    public InputActionReference hardResetAction;
    public InputActionReference softResetAction;
    
    public event EventHandler softResetEvent;
    public event EventHandler hardResetEvent;

    public GameObject pinGroup;
    public GameObject ballGroup;

    //TODO: timeout counter nach wurf -> soft reset + neue bälle

    void Start()
    {
        hardResetAction.action.performed += context => HardReset();
        softResetAction.action.performed += context => SoftReset();
        _timer = GetComponent<TimerScript>();
        _timer.TimerComplete = TurnEnd;
    }

    // Update is called once per frame+
    private void Update()
    {
    }

    private void TurnEnd()
    {
        if (_knockedPins.Count == 10) _secondShot = false;
        if (_secondShot)
        {
            SoftReset();
            _secondShot = false;
        }
        else
        {
            HardReset();
        }
    }
    
    public int pinCounter {
        get => _knockedPins?.Count ?? 0;
    }

    public void KnockPin(GameObject knockedPin)
    {
        _knockedPins.Add(knockedPin);
        _timer.ResetTimer();
    }
    
    public void BallRolled(GameObject ball)
    {
        var b = ball.GetComponent<BowlingBall>();
        if (b is not null)
        {
            _activeBall = b;
            _timer.ResetTimer();
        }
    }

    private void HardReset()
    {
        foreach (var pin in pinGroup.GetComponentsInChildren<BownlingPin>(includeInactive: true))
        {
            pin.Reset();
        }
        foreach (var ball in ballGroup.GetComponentsInChildren<BowlingBall>(includeInactive: true))
        {
            ball.Reset();
        }
        _knockedPins.Clear();
        hardResetEvent?.Invoke(this, EventArgs.Empty);
        _secondShot = true;
    }

    private void SoftReset()
    {
        foreach (var pin in pinGroup.GetComponentsInChildren<BownlingPin>())
        {
            pin.Reset();
            if (_knockedPins.Contains(pin.gameObject))
            {
                pin.gameObject.SetActive(false);
            }
        }
        _activeBall?.Reset();
        softResetEvent?.Invoke(this, EventArgs.Empty);
    }
    
    
}
