using Basics;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/* Copyright (C) Tom Troeger */

namespace Bowling
{
    public class BowlingBall : BasicResettable
    { 
        [SerializeField] private Vector3 startSpeed;
        [SerializeField] private float ballMass;
        [SerializeField] private Canvas massDisplay;
        
        private XRGrabInteractable _grabInteractable;
        private AudioSource _audioSource;
        private SoundFXManager _soundFXManager;

        private TextMeshProUGUI _massScale;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            Rigidbody.velocity = startSpeed;
            _grabInteractable = GetComponent<XRGrabInteractable>();
            _massScale = massDisplay.GetComponentInChildren<TextMeshProUGUI>();
            _audioSource = GetComponent<AudioSource>();
            _soundFXManager = SoundFXManager.Instance;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (_grabInteractable.firstInteractorSelecting == null)
            {
                massDisplay.gameObject.SetActive(false);
            }
            else
            {
                massDisplay.gameObject.SetActive(true);
                _audioSource.mute = true;
            }
            _audioSource.volume = Mathf.Clamp(Rigidbody.velocity.magnitude, 0, 1);
            if (!Mathf.Approximately(ballMass, Rigidbody.mass))
                ChangeBallMass();
        }

        private void ChangeBallMass()
        {
            if (Rigidbody is not null)
            {
                Rigidbody.mass = ballMass;
            }
            _massScale.text = Rigidbody?.mass.ToString("0.0");
        }

        public void IncreaseMass(float amount)
        {
            _soundFXManager.PlaySoundFX(_soundFXManager.uiSound, gameObject.transform);
            ballMass += amount;
        }

        public void DecreaseMass(float amount)
        {
            _soundFXManager.PlaySoundFX(_soundFXManager.uiSound, gameObject.transform);
            ballMass -= amount;
        }

        private void OnCollisionEnter(Collision other)
        {
            _audioSource.mute = false;
        }
    }
}
