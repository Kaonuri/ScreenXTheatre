using UnityEngine;
using System.Collections;

public class ScreenXTheatreApp : MonoBehaviour {
	private static ScreenXTheatreApp _instance;

	public static ScreenXTheatreApp app {
		get {
			return _instance;
		}
	}

	void Awake() {
		if (_instance != null) {
			throw new UnityException("[ERROR] There must be only one instance of ScreenXTheatreApp.");
		}
		_instance = this;
		GameObject.DontDestroyOnLoad(gameObject);
	}
}
