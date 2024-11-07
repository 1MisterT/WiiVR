using Basics;
using UnityEngine;

/* Copyright (C) Tom Troeger */

namespace Bowling
{
    public class BowlingPin : BasicResettable
    {
        // Start is called before the first frame update
        [SerializeField]
        [Range(0f,1f)]
        [Tooltip("At what value is the bowling pin counted as knocked down (0 never to 1 always")]
        private float knockThreshold;
        
        private bool _isKnocked = false;
        private BowlingController _bowlingController;

        protected override void Start()
        {
            base.Start();
            _bowlingController = BowlingController.Instance;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (_isKnocked) return;
            if (!(Rigidbody.transform.up.y < 0.9f)) return;
            _bowlingController.KnockPin(gameObject);
            _isKnocked = true;
        }

        public override void Reset()
        {
            base.Reset();
            _isKnocked = false;
        }
    }
}
