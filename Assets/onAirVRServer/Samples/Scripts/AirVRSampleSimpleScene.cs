using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Collections.Generic;

public class AirVRSampleSimpleScene : MonoBehaviour, AirVRCameraRigManager.EventHandler {
    private float _remainingTimeToAllowToTravelScene;
    private bool _sceneBeingUnloaded;

    public AirVRCameraRig cameraRig;
    public AudioSource music;
    public string sceneNameToTravel;

    void Awake() {
        AirVRCameraRigManager.managerOnCurrentScene.Delegate = this;
    }

    void Start() {
        _remainingTimeToAllowToTravelScene = 1.0f;
    }

    void Update() {
        if (_remainingTimeToAllowToTravelScene >= 0.0f) {
            _remainingTimeToAllowToTravelScene -= Time.deltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.T) || AirVRInput.GetButtonDown(cameraRig, AirVRInput.Touchpad.Button.BackButton) || AirVRInput.GetButtonDown(cameraRig, AirVRInput.Gamepad.Button.B)) {
            _sceneBeingUnloaded = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNameToTravel);
        }
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
        // NOTE : This event occurs in OnDestroy() of AirVRCameraRig during unloading scene.
        //        You should be careful because some objects in the scene might be destroyed already on this event.
        if (_sceneBeingUnloaded == false) {
            AirVRSamplePlayer player = cameraRig.GetComponentInParent<AirVRSamplePlayer>();
            player.EnableInteraction(false);
            
            music.Stop();
        }
    }
}
