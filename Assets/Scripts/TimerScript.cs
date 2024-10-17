using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Action TimerComplete = () => {};  // Der Callback, der nach Ablauf des Timers ausgeführt wird.
    private Coroutine _timerCoroutine; // Die laufende Coroutine für den Timer.
    public float timerDuration = 10f;  // Dauer des Timers (10 Sekunden).

    // Startet oder setzt den Timer zurück
    public void ResetTimer()
    {
        // Wenn bereits eine Coroutine läuft, beende sie
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }

        // Starte eine neue Coroutine
        _timerCoroutine = StartCoroutine(StartTimer());
    }

    // Coroutine, die den Timer überwacht
    private IEnumerator StartTimer()
    {
        // Warte die Dauer des Timers
        yield return new WaitForSeconds(timerDuration);

        // Wenn die Wartezeit abgelaufen ist, rufe den Callback auf
        TimerComplete?.Invoke();
    }

    // Optional: Methode zum Abbrechen des Timers (falls notwendig)
    public void CancelTimer()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
    }
}