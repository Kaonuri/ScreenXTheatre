using UnityEngine;
using System.Collections;

[RequireComponent(typeof(OVRCameraRig))]

public class OVRUICamera : VRUICamera {
	public static OVRCameraRig currentCameraController {
		get {
			VRUICamera result = VRUIManager.currentVRUICamera;
			return (result != null) ? (result as OVRUICamera).cameraController : null;
		}
	}

	public OVRCameraRig cameraController {
		get {
			return gameObject.GetComponent<OVRCameraRig>();
		}
	}

	public override Vector3 position {
		get {
			return cameraController.centerEyeAnchor.position;
		}
	}

	public override Vector3 forward {
		get {
			return cameraController.centerEyeAnchor.forward;
		}
	}
}
