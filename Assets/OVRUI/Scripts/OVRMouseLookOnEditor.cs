using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;

[RequireComponent(typeof(SimpleMouseRotator))]

public class OVRMouseLookOnEditor : MonoBehaviour {
#if UNITY_EDITOR
	void Start() {
		gameObject.GetComponent<SimpleMouseRotator>().enabled = true;
	}
#else
	void Start() {
		gameObject.GetComponent<SimpleMouseRotator>().enabled = false;
	}
#endif
}
