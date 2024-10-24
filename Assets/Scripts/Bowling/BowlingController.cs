using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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

        // Start is called before the first frame update
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
        [SerializeField] private AudioClip knockPinSound;
        [SerializeField] private AudioClip looseSound;
        [SerializeField] private AudioClip spareSound;
        [SerializeField] private AudioClip strikeSound;
        [SerializeField] private int turnsPerFrame = 2;
        
    
        //Event handler to send events to other components
        public event EventHandler SoftResetEvent;
        public event EventHandler HardResetEvent;
        public event EventHandler<SpecialPinPosition> SpecialPinEvent;


        private void Start()
        {
            _soundFX = SoundFXManager.instance;
            hardResetAction.action.performed += context => HardReset();
            softResetAction.action.performed += context => SoftReset();
            _timer = GetComponent<TimerScript>();
            _timer.TimerComplete = TurnEnd;
        }

        private void TurnEnd()
        {
            StartCoroutine(GetSpecialPinPosition(_totalKnockedPins.Count, _newKnockedPins.Count));
            //Strike condition
            if (_newKnockedPins.Count == 10 || _turn == 0)
            {
                HardReset();
                return;
            }

            _turn--;
            SoftReset();
        }
    
        public int pinCounter => _totalKnockedPins?.Count ?? 0 + _newKnockedPins?.Count ?? 0;

        public void KnockPin(GameObject knockedPin)
        {
            _newKnockedPins?.Add(knockedPin);
            _timer.ResetTimer();
            _soundFX.PlaySoundFX(knockPinSound, knockedPin.gameObject.transform, 0.5f);
            
        }
    
        public void BallRolled(GameObject ball, bool setTimer)
        {
            var b = ball.GetComponent<BowlingBall>();
            if (b is null) return;
            _activeBall = b;
            if (setTimer) _timer.ResetTimer();
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
            _totalKnockedPins.Clear();
            _newKnockedPins.Clear();
            HardResetEvent?.Invoke(this, EventArgs.Empty);
            _turn = turnsPerFrame - 1;
            return;
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

            _totalKnockedPins.UnionWith(_newKnockedPins);
            _newKnockedPins?.Clear();
            _activeBall?.Reset();
            SoftResetEvent?.Invoke(this, EventArgs.Empty);
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
