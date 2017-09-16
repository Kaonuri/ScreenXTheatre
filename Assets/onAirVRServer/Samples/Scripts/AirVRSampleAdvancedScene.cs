using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVRSampleAdvancedScene : MonoBehaviour, AirVRCameraRigManager.EventHandler {
    [SerializeField]
    private AirVRCameraRig _primaryCameraRig;

    void Awake() {
        AirVRCameraRigManager.managerOnCurrentScene.Delegate = this;
    }

    // implements AirVRCameraRigMananger.EventHandler
    public void AirVRCameraRigWillBeBound(AirVRClientConfig config, List<AirVRCameraRig> availables, out AirVRCameraRig selected) {
        Debug.Log("userID : " + config.userID);

        if (availables.Contains(_primaryCameraRig)) {
            selected = _primaryCameraRig;
        }
        else if (availables.Count > 0) {
            selected = availables[0];
        }
        else {
            selected = null;
        }
    }

    public void AirVRCameraRigActivated(AirVRCameraRig cameraRig) {}
    public void AirVRCameraRigDeactivated(AirVRCameraRig cameraRig) {}
    public void AirVRCameraRigHasBeenUnbound(AirVRCameraRig cameraRig) {}
}
