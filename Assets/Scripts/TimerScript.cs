using System;
using System.Collections;
using UnityEngine;

// Copyright (C) Tom Troeger

public class TimerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Action TimerComplete = () => {};  // Callback after Timer
    private Coroutine _timerCoroutine; // Timer Routine
    [SerializeField] private float defaultDuration = 10f;

    // Starts and Resets the Timer
    public void ResetTimer(float timerDuration = 0f)
    {
        // End running Coroutine and start new
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }

        // Start new Coroutine
        _timerCoroutine = StartCoroutine(StartTimer(timerDuration == 0 ? defaultDuration : timerDuration));
    }

    // Coroutine, for Timer
    private IEnumerator StartTimer(float timerDuration)
    {
        // Wait and execute Callback
        yield return new WaitForSeconds(timerDuration);
        TimerComplete?.Invoke();
    }

    // Optional: Cancel Timer
    public void CancelTimer()
    {
        if (_timerCoroutine == null) return;
        StopCoroutine(_timerCoroutine);
        _timerCoroutine = null;
    }
}