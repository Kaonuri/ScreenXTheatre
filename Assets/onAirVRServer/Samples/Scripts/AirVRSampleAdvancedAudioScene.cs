using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AirVRSampleAdvancedAudioScene : MonoBehaviour, AirVRCameraRigManager.EventHandler {
    public AirVRSampleAudioSource music;

    void Awake() {
        AirVRCameraRigManager.managerOnCurrentScene.Delegate = this;
    }

    // implements AirVRCameraRigManager.EventHandler
    public void AirVRCameraRigWillBeBound(AirVRClientConfig config, List<AirVRCameraRig> availables, out AirVRCameraRig selected) {
        selected = availables.Count > 0 ? availables[0] : null;

        if (selected) {
            AirVRSamplePlayer player = selected.GetComponentInParent<AirVRSamplePlayer>();
            player.EnableInteraction(true);

            music.Play();
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

        music.Stop();
    }
}
