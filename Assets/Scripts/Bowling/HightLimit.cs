using Basics;
using UnityEngine;

namespace Bowling
{
    public class HightLimit : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.GetComponent<BasicResettable>()?.Reset();
        
        }
    }
}
