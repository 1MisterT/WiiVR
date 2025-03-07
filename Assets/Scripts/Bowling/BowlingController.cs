using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

/* Copyright (C) Tom Troeger */

namespace Bowling
{
    public class BowlingController : MonoBehaviour
    {
        public static BowlingController Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private readonly HashSet<GameObject> _totalKnockedPins = new HashSet<GameObject>();
        private readonly HashSet<GameObject> _newKnockedPins = new HashSet<GameObject>();
        private BowlingBall _activeBall;
        private TimerScript _timer;
        private SoundFXManager _soundFX;
        private int _turn;
        
        // Unity Inputs
        [SerializeField] private InputActionReference hardResetAction;
        [SerializeField] private InputActionReference softResetAction;
        [SerializeField] private GameObject pinGroup;
        [SerializeField] private GameObject ballGroup;
        [SerializeField] private AudioClip[] knockPinSounds;
        [SerializeField] private AudioClip looseSound;
        [FormerlySerializedAs("spareSound")] [SerializeField] private AudioClip[] spareSounds;
        [FormerlySerializedAs("strikeSound")] [SerializeField] private AudioClip[] strikeSounds;
        [SerializeField] private int turnsPerFrame = 2;
        [SerializeField] private int framesPerGame = 10;
        
    
        //Event handler to send events to other components
        public event EventHandler<int> SoftResetEvent;
        public event EventHandler<int> HardResetEvent;
        public event EventHandler BallRolledEvent;
        public event EventHandler GameResetEvent; 
        public event EventHandler<SpecialPinPosition> SpecialPinEvent;

        private int[,] _bowlingScore;
        public int currentFrame;


        private void Start()
        {
            _soundFX = SoundFXManager.Instance;
            hardResetAction.action.performed += _ => PerformCustomReset(ResetHard);
            softResetAction.action.performed += _ => PerformCustomReset(ResetSoft);
            _timer = GetComponent<TimerScript>();
            _timer.TimerComplete = TurnEnd;
            _bowlingScore = new int[10, turnsPerFrame];
            _turn = turnsPerFrame - 1;
        }

        private static void PerformCustomReset(Action resetFunction)
        {
            if (PlayerController.PlayerGame != "Bowling") return;
            resetFunction.Invoke();
        }

        private void TurnEnd()
        {
            StartCoroutine(GetSpecialPinPosition(_totalKnockedPins.Count, _newKnockedPins.Count));
            //Strike condition
            if (_newKnockedPins.Count == 10 || _turn <= 0)
            {
                ResetHard();
                return;
            }
            if (currentFrame > framesPerGame - 1)
            {
                ResetGame();
            }
            _turn--;
            ResetSoft();
        }
    
        public int pinCounter => _totalKnockedPins?.Count ?? 0 + _newKnockedPins?.Count ?? 0;

        public void KnockPin(GameObject knockedPin)
        {
            _newKnockedPins?.Add(knockedPin);
            _timer.ResetTimer();
            _soundFX.PlayRandomSoundFX(knockPinSounds, knockedPin.gameObject.transform, 0.5f);
            
        }
    
        public void BallRolled(GameObject ball, bool setTimer)
        {
            var b = ball.GetComponent<BowlingBall>();
            if (b is null) return;
            _activeBall = b;
            if (setTimer) _timer.ResetTimer();
            BallRolledEvent?.Invoke(this, EventArgs.Empty);
        }

        public void ResetHard()
        {
            ResetPin();
            ResetBall();
            if (_newKnockedPins != null) _totalKnockedPins.UnionWith(_newKnockedPins);
            if (currentFrame <= framesPerGame - 1)
            {
                _bowlingScore[currentFrame, _turn] = _newKnockedPins?.Count ?? 0;
                HardResetEvent?.Invoke(this, _totalKnockedPins.Count);
                currentFrame += 1;
            }
            else
            {
                ResetGame();
            }
            _totalKnockedPins.Clear();
            _newKnockedPins?.Clear();
            _turn = turnsPerFrame - 1;
        }

        public void ResetSoft()
        {
            foreach (var pin in pinGroup.GetComponentsInChildren<BowlingPin>())
            {
                pin.Reset();
                if (_newKnockedPins.Contains(pin.gameObject))
                {
                    pin.gameObject.SetActive(false);
                }
            }

            if (currentFrame <= framesPerGame - 1)
            {
                _bowlingScore[currentFrame, _turn] = _newKnockedPins?.Count ?? 0;
                _totalKnockedPins.UnionWith(_newKnockedPins);
                SoftResetEvent?.Invoke(this, _totalKnockedPins.Count);
            }
            _newKnockedPins?.Clear();
            _activeBall?.Reset();
        }

        public void ResetGame()
        {
            currentFrame = 0;
            _bowlingScore = new int[10, turnsPerFrame];
            GameResetEvent?.Invoke(this, EventArgs.Empty);
        }

        public void ResetBall()
        {
            foreach (var ball in ballGroup.GetComponentsInChildren<BowlingBall>(includeInactive: true))
            {
                ball.Reset();
            }
        }
        
        public void ResetPin()
        {
            foreach (var pin in pinGroup.GetComponentsInChildren<BowlingPin>(includeInactive: true))
            {
                pin.Reset();
            }
        }

        private IEnumerator GetSpecialPinPosition(int totalPins, int newPins)
        {
            if (newPins == 10)
            {
                SpecialPinEvent?.Invoke(this, SpecialPinPosition.Strike);
                _soundFX.PlayRandomSoundFX(strikeSounds, transform);
                yield return null;
            }

            if (newPins + totalPins == 10)
            {
                SpecialPinEvent?.Invoke(this, SpecialPinPosition.Spare);
                _soundFX.PlayRandomSoundFX(spareSounds, transform);
                yield return null;
            }

            if (newPins != 0) yield break;
            SpecialPinEvent?.Invoke(this, SpecialPinPosition.None);
            _soundFX.PlaySoundFX(looseSound, transform);
            yield return null;
        }

        public enum SpecialPinPosition : ushort
        {
            None = 0,
            Strike = 1,
            Spare = 2
        }
    }
}
