using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Bowling
{
    public class BowlingController : MonoBehaviour
    {
        public static BowlingController instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
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
        [SerializeField] private AudioClip spareSound;
        [SerializeField] private AudioClip strikeSound;
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
            _soundFX = SoundFXManager.instance;
            hardResetAction.action.performed += context => HardReset();
            softResetAction.action.performed += context => SoftReset();
            _timer = GetComponent<TimerScript>();
            _timer.TimerComplete = TurnEnd;
            _bowlingScore = new int[10, turnsPerFrame];
            _turn = turnsPerFrame - 1;
        }

        private void TurnEnd()
        {
            StartCoroutine(GetSpecialPinPosition(_totalKnockedPins.Count, _newKnockedPins.Count));
            //Strike condition
            if (_newKnockedPins.Count == 10 || _turn <= 0)
            {
                HardReset();
                return;
            }
            if (currentFrame > framesPerGame - 1)
            {
                currentFrame = 0;
                _bowlingScore = new int[10, turnsPerFrame];
                GameResetEvent?.Invoke(this, EventArgs.Empty);
            }
            _turn--;
            SoftReset();
            
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
            _bowlingScore[currentFrame, _turn] = _newKnockedPins?.Count ?? 0;
            if (_newKnockedPins != null)
            {
                _totalKnockedPins.UnionWith(_newKnockedPins);
                _newKnockedPins?.Clear();
            }

            HardResetEvent?.Invoke(this, _totalKnockedPins.Count);
            _totalKnockedPins.Clear();
            
            currentFrame += 1;
            _turn = turnsPerFrame - 1;
        }

        private void SoftReset()
        {
            foreach (var pin in pinGroup.GetComponentsInChildren<BownlingPin>())
            {
                pin.Reset();
                if (_newKnockedPins.Contains(pin.gameObject))
                {
                    pin.gameObject.SetActive(false);
                }
            }
            _bowlingScore[currentFrame, _turn] = _newKnockedPins?.Count ?? 0;
            _totalKnockedPins.UnionWith(_newKnockedPins);
            SoftResetEvent?.Invoke(this, _totalKnockedPins.Count);
            _newKnockedPins?.Clear();
            _activeBall?.Reset();
        }

        private IEnumerator GetSpecialPinPosition(int totalPins, int newPins)
        {
            if (newPins == 10)
            {
                SpecialPinEvent?.Invoke(this, SpecialPinPosition.Strike);
                _soundFX.PlaySoundFX(strikeSound, transform);
                yield return null;
            }

            if (newPins + totalPins == 10)
            {
                SpecialPinEvent?.Invoke(this, SpecialPinPosition.Spare);
                _soundFX.PlaySoundFX(spareSound, transform);
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
