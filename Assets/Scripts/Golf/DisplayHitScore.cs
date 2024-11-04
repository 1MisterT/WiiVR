using TMPro;
using UnityEngine;

namespace Golf
{
    public class DisplayHitScore : MonoBehaviour
    {
        // private Text _scoreText; // For standard UI Text

        public TextMeshProUGUI scoreText; // For TextMeshPro

        public GameObject golfBall;
        private Ball _ball; // assign in inspector or find in start()

        private void Start()
        { 
            if (golfBall != null)
            {
                _ball = golfBall.GetComponent<Ball>();
                
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

        private void FixedUpdate()
        {
            if (_ball != null)
            {
               DisplayHitCount();

               if (_ball.isInhole)
               {
                   DisplayVictoryText();
               }
            }
            else
            {
                Debug.LogWarning("Ball reference is null.");
            }
            
            
        }

        // check hitcount the Ball-Object
        private void DisplayHitCount()
        {
            scoreText.text = "Golfschläge: " + _ball.hitCount; // Update the UI text with the public value
        }
        
        // check the isInHole property of the Ball-Object and display a victory text if it is true
        private void DisplayVictoryText()
        {
            scoreText.text = "Der Ball ist im Golfloch! Benötigte Schläge: " +  _ball.hitCount;
        }
    }
}
