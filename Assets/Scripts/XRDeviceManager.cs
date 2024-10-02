
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using UnityEngine.XR.Management;

public class XRDeviceManager : MonoBehaviour
{
    private XRDeviceSimulator simulator;
    // Start is called before the first frame update
    void Start()
    {
        if (!XRSettings.isDeviceActive)
        {
            Debug.Log("No Headset plugged, trying to connect to Mock HMD");
        }
        
        else if (XRSettings.isDeviceActive &&
                 (XRSettings.loadedDeviceName == "Mock HMD" || XRSettings.loadedDeviceName == "MockHMDDisplay"))
        {
            Debug.Log("Using Mock HMD");
           
        }
        else
        {
            Debug.Log("Your are currently using the headset " + XRSettings.loadedDeviceName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Call this method when the scene or application is stopping to clean up XR resources
    void OnApplicationQuit()
    {
        StopAndDeinitializeXR();
    }
    
    // method to stop XR System and deinitialize current loader (helpful when using Mock HMD and switching to VR headset) and intialize XR system again
        void StopAndDeinitializeXR()
        {
            if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager != null)
            {
                Debug.Log("Deinitializing " + XRSettings.loadedDeviceName);
                XRGeneralSettings.Instance.Manager.StopSubsystems();
                XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            }
        }

        // Coroutine to initialize XR loader (after stopping any existing loaders)
        IEnumerator InitializeXR()
        {
            // Stop and deinitialize any currently active loader first
            StopAndDeinitializeXR();

            // Initialize the XR loader
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

            // Start the subsystems if initialization is successful
            if (XRGeneralSettings.Instance.Manager.activeLoader != null)
            {
                XRGeneralSettings.Instance.Manager.StartSubsystems();
                Debug.Log("XR loader initialized and subsystems started.");
            }
            else
            {
                Debug.LogError("Failed to initialize XR loader.");
            }
        }
}
