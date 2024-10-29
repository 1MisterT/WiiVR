using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
            _bowlingController = BowlingController.instance;
            if (frameDisplayGroup == null)
            {
                //frameDisplayGroup = GetComponentInChildren<FrameDisplay>().gameObject.GetComponentInParent<GameObject>();
            }
            _frameDisplays = frameDisplayGroup.GetComponentsInChildren<FrameDisplay>();
            
            _bowlingController.BallRolledEvent += (sender, args) =>
            {
                pinCam.enabled = true;
            };
            _bowlingController.SoftResetEvent += (sender, args) =>
            {
                pinCam.enabled = false;
                _frameDisplays[_bowlingController.currentFrame].SetHalfFrame(args);
            };
            _bowlingController.HardResetEvent += (sender, args) =>
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
