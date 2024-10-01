using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

public class XRDeviceManager : MonoBehaviour
{
    private XRDeviceSimulator simulator;
    // Start is called before the first frame update
    void Start()
    {
        simulator = GetComponent<XRDeviceSimulator>();
        
        if (!XRSettings.isDeviceActive)
        {
            Debug.Log("No Headset plugged");
        }
        else if (XRSettings.isDeviceActive &&
                 (XRSettings.loadedDeviceName == "Mock HMD" || XRSettings.loadedDeviceName == "MockHMDDisplay"))
        {
            Debug.Log("Using Mock HMD");
        }
        else
        {
            Debug.Log("Your are currently using the headset " + XRSettings.loadedDeviceName);
             simulator.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
