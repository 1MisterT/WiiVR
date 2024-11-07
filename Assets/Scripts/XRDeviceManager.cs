
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using UnityEngine.XR.Management;

public class XRDeviceManager : MonoBehaviour
{
    private XRDeviceSimulator _simulator;

    // Start is called before the first frame update
    void Start()
    {
        _simulator = FindObjectOfType<XRDeviceSimulator>();
        if (_simulator != null)
        {
            DetectXRDevice();
        }
        else
        {
            Debug.LogWarning("No XR Device Found");
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
            Debug.Log($"Deinitializing {XRSettings.loadedDeviceName}");
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        }
    }

    void DetectXRDevice()
    {
        // Get a list of all connected XR input devices
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevices(inputDevices);
        
        // Count and Check device names
        CheckAndLogXRDevices(inputDevices);

        // Check if a Meta Quest is connected by looking for a device with the correct characteristics
        bool isQuestConnected = false;

        foreach (var device in inputDevices)
        {
            if (device.characteristics.HasFlag(InputDeviceCharacteristics.HeadMounted))
            {
                string deviceName = device.name.ToLower();
                if (deviceName.Contains("oculus") || deviceName.Contains("quest"))
                {
                    isQuestConnected = true;
                    break;
                }
            }
        }

        // Disable the XR Device Simulator if Meta Quest is connected
        if (isQuestConnected)
        {
                // Disable the XR Device Simulator
                _simulator.enabled = false; 
                Debug.Log("Meta Quest detected: XR Device Simulator disabled.");
            
        }
        else
        {
            Debug.Log($"No Meta Quest detected: Switching to {XRSettings.loadedDeviceName}");
        }
    }
    
    void CheckAndLogXRDevices(List<InputDevice> inputDevices)
    {
        if (inputDevices.Count == 0)
        {
            Debug.Log("No XR devices found.");
        }
        else
        {
            Debug.Log("Found " + inputDevices.Count + " XR devices");
            foreach (var device in inputDevices)
            {
                string deviceInfo = $"Device Name: {device.name}, " +
                                    $"Characteristics: {device.characteristics}, " +
                                    $"Manufacturer: {device.manufacturer}, " +
                                    $"Loaded Device: {XRSettings.loadedDeviceName}";
                Debug.Log("Following device could be found: " + deviceInfo);
            }
        }
    }
    
    
    // Dunno if we need these later
    void OnDebugMockHmd()
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
