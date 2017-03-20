using UnityEngine;
using System.Collections;

public class VRUICamera : MonoBehaviour {
	void Start() {
		VRUIManager.manager.SetCamera(this);
	}

	void OnDestroy() {
		if (VRUIManager.manager != null) {
			VRUIManager.manager.UnsetCamera();
		}
	}

	public virtual Vector3 position {
		get {
			return gameObject.transform.position;
		}
	}

	public virtual Vector3 forward {
		get {
			return gameObject.transform.forward;
		}
	}
}
