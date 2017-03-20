using UnityEngine;
using System.Collections;

public class ScreenXTheatrePlayer : MonoBehaviour {
	private static ScreenXTheatrePlayer _instance;

	public static ScreenXTheatrePlayer player {
		get {
			return _instance;
		}
	}

	private Transform _thisTransform;

	void Awake() {
		if (_instance != null) {
			throw new UnityException("[ERROR] There must be only one instance of ScreenXTheatrePlayer.");
		}
		_instance = this;
		GameObject.DontDestroyOnLoad(gameObject);

		_thisTransform = transform;
	}

	public void SetPosition(Transform pos) {
		_thisTransform.position = pos.position;
		_thisTransform.rotation = pos.rotation;
	}
}
