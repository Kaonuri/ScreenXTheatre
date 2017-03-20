using UnityEngine;
using System.Collections;

public class VRUILookAtObject : MonoBehaviour {
	public Transform target;
	public bool holdAxisX;
	public bool holdAxisY;
	public bool holdAxisZ;

	void Update() {
		Vector3 euler = transform.localRotation.eulerAngles;
		transform.LookAt(target);

		Vector3 targetEuler = transform.localRotation.eulerAngles;
		if (holdAxisX) {
			targetEuler.x = euler.x;
		}
		if (holdAxisY) {
			targetEuler.y = euler.y;
		}
		if (holdAxisZ) {
			targetEuler.z = euler.z;
		}
		transform.localRotation = Quaternion.Euler(targetEuler);
	}
}
