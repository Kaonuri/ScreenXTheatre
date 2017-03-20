using UnityEngine;
using System.Collections;

public class VRUIKeepScaleInPerspective : MonoBehaviour {
	[SerializeField]
	private Transform _referenceObject;

	[SerializeField]
	private Transform _cameraPosition;

	// Be sure that the reference scale object does not change its scale in Start() (also for camera position).
	void Start() {
		if (_referenceObject != null && _cameraPosition != null) {
			gameObject.transform.localScale = _referenceObject.localScale * 
											  Vector3.Distance(gameObject.transform.position, _cameraPosition.position) /
											  Vector3.Distance(_referenceObject.position, _cameraPosition.position);
		}
	}
}
