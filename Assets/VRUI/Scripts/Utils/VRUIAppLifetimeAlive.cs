using UnityEngine;
using System.Collections;

public class VRUIAppLifetimeAlive : MonoBehaviour {	
	void Awake() {
		GameObject.DontDestroyOnLoad(gameObject);
	}
}
