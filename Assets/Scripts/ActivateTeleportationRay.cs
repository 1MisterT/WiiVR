using UnityEngine;
using UnityEngine.InputSystem;

public class ActivateTeleportationRay : MonoBehaviour
{
    public GameObject leftTeleportationRay;
    public GameObject rightTeleportationRay;

    public InputActionProperty leftActivate;
    public InputActionProperty rightActivate;

    // Update is called once per frame
    void Update()
    {
        leftTeleportationRay.SetActive(leftActivate.action.ReadValue<float>() >= 0.1f);
        rightTeleportationRay.SetActive(rightActivate.action.ReadValue<float>() >= 0.1f);
    }
}
