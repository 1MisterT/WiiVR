using System;
using Basics;
using UnityEngine;

namespace Bowling
{
    public class BownlingPin : BasicResettable
    {
        // Start is called before the first frame update
        private Boolean _isKnocked = false;
        private BowlingController _bowlingController;

        protected override void Start()
        {
            base.Start();
            _bowlingController = BowlingController.instance;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (_isKnocked) return;
            if (Rigidbody.transform.up.y < 0.5f )
            {
                _bowlingController.KnockPin(gameObject);
                _isKnocked = true;
            }
        }

        public override void Reset()
        {
            base.Reset();
            _isKnocked = false;
        }
    }
}
