using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VRUIAudioFramework : MonoBehaviour {
	private static VRUIAudioFramework _instance;

	public static VRUIAudioFramework framework {
		get {
			return _instance;
		}
	}
	
	private Dictionary<string, VRUIAudioEvent> _globalEvents;
	private VRUIAudioEvent _lastPlayedEvent;

	public VRUIAudioListener audioListener;

	void Awake() {
		if (_instance != null) {
			throw new UnityException("ERROR : Only one VRUIAudioFramework must exist in one scene.");
		}
		_instance = this;
		GameObject.DontDestroyOnLoad(gameObject);
	}

	void Start() {
		_globalEvents = new Dictionary<string, VRUIAudioEvent>();

		VRUIAudioEvent[] events = gameObject.GetComponentsInChildren<VRUIAudioEvent>();
		foreach (VRUIAudioEvent evt in events) {
			_globalEvents.Add(evt.gameObject.name, evt);
		}
	}

	public VRUIAudioEvent lastGlobalEventPlayed {
		get {
			return _lastPlayedEvent;
		}
	}

	public void AttachAudioListener(Transform to) {
		if (audioListener != null) {
			audioListener.gameObject.transform.parent = to;
			audioListener.gameObject.transform.localPosition = Vector3.zero;
			audioListener.gameObject.transform.localRotation = Quaternion.identity;
		}
	}

	public void DetachAudioListener() {
		if (audioListener != null) {
			audioListener.gameObject.transform.parent = gameObject.transform;
		}
	}

	public VRUIAudioEvent GetGlobalEvent(string name) {
		return _globalEvents.ContainsKey(name) ? _globalEvents[name] : null;
	}

	public void PlayGlobalEvent(string name, bool transient = false) {
		if (_globalEvents.ContainsKey(name)) {
			VRUIAudioEvent evt = _globalEvents[name];
			evt.Play();

			if (transient == false) {
				_lastPlayedEvent = evt;
			}
		}
	}

	public void StopGlobalEvent(string name = null) {
		if (string.IsNullOrEmpty(name)) {
			if (_lastPlayedEvent != null) {
				_lastPlayedEvent.Stop();
			}
		}
		else {
			if (_globalEvents.ContainsKey(name)) {
				_globalEvents[name].Stop();
			}
		}
	}
}
