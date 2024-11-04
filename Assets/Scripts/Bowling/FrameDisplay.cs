using TMPro;
using UnityEngine;

/* Copyright (C) Tom Troeger */

namespace Bowling
{
    public class FrameDisplay : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private TextMeshProUGUI halfFrameText;
        [SerializeField] private TextMeshProUGUI fullFrameText;

        [SerializeField] private int frameNr;

        private void Start()
        {
            Reset();
        }

        // Update is called once per frame
        public void Reset()
        {
            halfFrameText.text = "";
            fullFrameText.text = "";
            BowlingController.Instance.GameResetEvent += (_, _) => Reset();
        }

        public void SetHalfFrame(int value)
        {
            halfFrameText.text = value < 10 ? value.ToString() : "X";
        }
    
        public void SetFullFrame(int value)
        {
            fullFrameText.text = value < 10 ? value.ToString() : halfFrameText.text == "" ? "X" : "/";
        }
    }
}
