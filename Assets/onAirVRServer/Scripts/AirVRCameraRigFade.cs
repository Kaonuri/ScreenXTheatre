using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AirVRCameraRig))]

public class AirVRCameraRigFade : MonoBehaviour {
    [SerializeField]
    private Color _fadeOutColor = Color.black;

    private void Start() {
        foreach (Camera cam in GetComponent<AirVRCameraRig>().cameras) {
            cam.gameObject.AddComponent<AirVRCameraFade>().fadeOutColor = _fadeOutColor;
        }
    }
}
