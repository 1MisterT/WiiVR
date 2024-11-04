using UnityEngine;
using UnityEngine.UI;

/* Copyright (C) Tom Troeger */

namespace Bowling
{
    public class BowlingUI : MonoBehaviour
    {
        private BowlingController _bowlingController;
        
        //[SerializeField] private TextMeshProUGUI pinCounterText;
        //[SerializeField] private TextMeshProUGUI frameCounterText;

        [SerializeField] private RawImage pinCam;

        [SerializeField] private GameObject frameDisplayGroup;

        private FrameDisplay[] _frameDisplays;
        // Start is called before the first frame update
        private void Start()
        {
            _bowlingController = BowlingController.Instance;
            if (frameDisplayGroup == null)
            {
                //frameDisplayGroup = GetComponentInChildren<FrameDisplay>().gameObject.GetComponentInParent<GameObject>();
            }
            _frameDisplays = frameDisplayGroup.GetComponentsInChildren<FrameDisplay>();
            
            _bowlingController.BallRolledEvent += (_, _) =>
            {
                pinCam.enabled = true;
            };
            _bowlingController.SoftResetEvent += (_, args) =>
            {
                pinCam.enabled = false;
                _frameDisplays[_bowlingController.currentFrame].SetHalfFrame(args);
            };
            _bowlingController.HardResetEvent += (_, args) =>
            {
                pinCam.enabled = false;
                _frameDisplays[_bowlingController.currentFrame].SetFullFrame(args);
            };
            pinCam.enabled = false;
        }

        // Update is called once per frame
        private void Update()
        {
            //pinCounterText.text = _bowlingController.pinCounter.ToString();
            //frameCounterText.text = _bowlingController.CurrentFrame.ToString();
        }
    }
}
