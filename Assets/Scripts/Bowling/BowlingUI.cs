using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BowlingUI : MonoBehaviour
{
    public BowlingController bowlingController;

    private TextMeshProUGUI _counterText;
    // Start is called before the first frame update
    private void Start()
    {
        _counterText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _counterText.text = bowlingController.pinCounter.ToString();
    }
}
