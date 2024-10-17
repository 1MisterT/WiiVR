using Basics;
using UnityEngine;

namespace Bowling
{
    public class BowlingBall : BasicResettable
    {
        public Vector3 startSpeed;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            Rigidbody.velocity = startSpeed;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
