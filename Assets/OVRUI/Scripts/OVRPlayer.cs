using UnityEngine;
using System.Collections;

public class OVRPlayer : MonoBehaviour {
	// Singleton; The instance of this type must be only one in any scene.
	private static OVRPlayer _player;
	public static OVRPlayer player {
		get {
			return _player;
		}
	}

	public AudioListener audioListener {
		get {
			return gameObject.GetComponentInChildren<AudioListener>();
		}
	}

	public OVRCameraRig cameraController {
		get {
			return gameObject.GetComponentInChildren<OVRCameraRig>();
		}
	}

	public OVRPlayerController playerController {
		get {
			OVRPlayerController result = gameObject.GetComponentInChildren<OVRPlayerController>();
			if (result == null) {
				return gameObject.GetComponent<OVRPlayerController>();
			}
			return result;
		}
	}

	public OVRUIPlayerController ovruiPlayerController {
		get {
			OVRUIPlayerController result = gameObject.GetComponentInChildren<OVRUICharacterControllerPlayerController>();
			if (result == null) {
				return gameObject.GetComponent<OVRUICharacterControllerPlayerController>();
			}
			return result;
		}
	}

	// The first access to the instance must occur after Awake().
	void Awake() {
		_player = this;
		GameObject.DontDestroyOnLoad(gameObject);
	}

	void Start() {
		EnablePlayerMovement(false);
	}

	public void SetPosition(Transform pos, bool setAsParent) {
		if (setAsParent) {
			gameObject.transform.parent = pos;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
		}
		else {
			gameObject.transform.position = pos.position;
			gameObject.transform.rotation = pos.rotation;
		}
	}

	public void MoveTransformToRoot() {
		gameObject.transform.parent = null;
	}

	public void EnablePlayerMovement(bool enable) {
		if (ovruiPlayerController != null) {
			ovruiPlayerController.Enable(enable);
		}
	}

	public void EnableCrosshair(bool enable) {
		VRUICrosshair3D crosshair = gameObject.GetComponentInChildren<VRUICrosshair3D>();
		if (crosshair != null) {
			crosshair.enabled = enable;
			crosshair.gameObject.GetComponent<MeshRenderer>().enabled = enable;
		}
	}
}
