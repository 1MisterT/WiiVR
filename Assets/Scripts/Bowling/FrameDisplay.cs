using System.Collections;
using System.Collections.Generic;
using Bowling;
using TMPro;
using UnityEngine;

public class FrameDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI halfFrameText;
    [SerializeField] private TextMeshProUGUI fullFrameText;

    [SerializeField] private int frameNr;
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    public void Reset()
    {
        halfFrameText.text = "";
        fullFrameText.text = "";
        BowlingController.instance.GameResetEvent += (sender, args) => Reset();
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
