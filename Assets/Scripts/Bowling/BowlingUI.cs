using TMPro;
using UnityEngine;

namespace Bowling
{
    public class BowlingUI : MonoBehaviour
    {
        private TextMeshProUGUI _counterText;
        // Start is called before the first frame update
        private void Start()
        {
            _counterText = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            _counterText.text = BowlingController.instance.pinCounter.ToString();
        }
    }
}
