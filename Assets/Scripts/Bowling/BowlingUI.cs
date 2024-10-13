using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BowlingUI : MonoBehaviour
{
    public BowlingController bowlingController;
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<TextMeshPro>().text = "Start Game";
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = bowlingController.pinCounter.ToString();
    }
}
