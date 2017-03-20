using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OVRCameraControllerOverlay : MonoBehaviour {
	private Transform _thisTransform;
	private Queue<Quaternion> _queue;

	public OVRCameraRig cameraController;
	public int orientationSyncDelay;

	void Start() {
		_thisTransform = transform;
		_queue = new Queue<Quaternion>();
	}

	void LateUpdate() {
		if (cameraController != null) {
			_queue.Enqueue(cameraController.centerEyeAnchor.rotation);
			if (_queue.Count > Mathf.Max(0, orientationSyncDelay)) {
				_thisTransform.rotation = _queue.Dequeue();
			}
		}
	}
}
