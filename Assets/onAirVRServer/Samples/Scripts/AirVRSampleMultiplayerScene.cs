using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AirVRSampleMultiplayerScene : MonoBehaviour, AirVRCameraRigManager.EventHandler {
    private AirVRStereoCameraRig[] _cameraRigs;

    public AudioSource music;
    public AirVRSampleDisplay display;

    private int getBoundCameraRigCount() {
        int result = 0;
        foreach (AirVRStereoCameraRig cameraRig in _cameraRigs) {
            if (cameraRig.isBoundToClient) {
                result++;
            }
        }
        return result;
    }

    void Awake() {
        _cameraRigs = FindObjectsOfType<AirVRStereoCameraRig>();

        AirVRCameraRigManager.managerOnCurrentScene.Delegate = this;
    }

    // implements AirVRCameraRigManager.EventHandler
    public void AirVRCameraRigWillBeBound(AirVRClientConfig config, List<AirVRCameraRig> availables, out AirVRCameraRig selected) {
        selected = null;

        if (availables.Count == 2) {
            foreach (AirVRCameraRig cameraRig in availables) {
                if (cameraRig.transform.parent.name.Equals("AirVRSamplePrimaryPlayer")) {
                    selected = cameraRig;
                    break;
                }
            }
        }
        else if (availables.Count == 1) {
            selected = availables[0];
        }

        if (selected) {
            AirVRSamplePlayer player = selected.GetComponentInParent<AirVRSamplePlayer>();
            player.EnableInteraction(true);

            if (getBoundCameraRigCount() == 0) {
                music.Play();
            }
            display.AddCameraPane(selected as AirVRStereoCameraRig);
        }
    }

    public void AirVRCameraRigActivated(AirVRCameraRig cameraRig) {
        // do nothing
    }

    public void AirVRCameraRigDeactivated(AirVRCameraRig cameraRig) {
        // do nothing
    }

    public void AirVRCameraRigHasBeenUnbound(AirVRCameraRig cameraRig) {
        AirVRSamplePlayer player = cameraRig.GetComponentInParent<AirVRSamplePlayer>();
        player.EnableInteraction(false);

        if (getBoundCameraRigCount() == 0) {
            music.Stop();
        }
        display.RemoveCameraPane(cameraRig as AirVRStereoCameraRig);
    }
}
