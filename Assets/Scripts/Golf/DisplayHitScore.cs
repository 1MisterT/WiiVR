using TMPro;
using Unity.AI.Navigation.Samples;
using UnityEngine;
using UnityEngine.UI;

namespace Golf
{
    public class DisplayHitScore : MonoBehaviour
    {
        public TextMeshProUGUI scoreText; // For TextMeshPro

        private GameObject _golfBall;
        private Ball _ball; // assign in inspector or find in start()

        private void Start()
        { 
            _golfBall = GameObject.Find("GolfBall");
            if (_golfBall != null)
            {
                _ball = _golfBall.GetComponent<Ball>();
                
                if (_ball == null)
                {
                    Debug.LogError("Ball component not found on the assigned golfBall GameObject.");
                }
                else
                {
                    Debug.Log("Ball is assigned: " + _ball.name);
                }
            } else
            {
                Debug.LogError("GolfBall GameObject is not assigned in the Inspector.");
            }
        }

        // check hitcount the Ball-Object
        public void DisplayHitCount()
        {
            scoreText.text = "Golfschläge: " + _ball.hitCount; // Update the UI text with the public value
        }
        
        // check the isInHole property of the Ball-Object and display a victory text if it is true
        public void DisplayVictoryText()
        {
            scoreText.text = "Der Ball ist im Golfloch!\n\nBenötigte Schläge: " +  _ball.hitCount;
        }

        public void EnableUI()
        {
            gameObject.SetActive(true);
        }

        public void DisableUI()
        {
            gameObject.SetActive(false);
        }
    }
}
