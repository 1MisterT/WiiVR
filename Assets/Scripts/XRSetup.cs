using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class XRSetup : MonoBehaviour
{
    private float _cameraHeight = 0.6f;
    void Start()
    {
        // Ensure XR General Settings instance is available
        if (XRGeneralSettings.Instance.Manager != null)
        {
            // Start the XR Manager
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            // Set the camera's position to the desired height
            transform.position = new Vector3(transform.position.x, _cameraHeight, transform.position.z);

        }
    }

    void OnDestroy()
    {
        // Stop the XR Manager on destroy
        if (XRGeneralSettings.Instance.Manager != null)
        {
            XRGeneralSettings.Instance.Manager.StopSubsystems();
        }
    }
}

