using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VRUIElement : MonoBehaviour {
	public interface EventListener {
		void VRUIElementFocused(VRUIElement element, bool focused);
		void VRUIElementEventOccurred(VRUIElement element, int deviceID, int elementID, int action);
		void VRUIElementEnabled(VRUIElement element, bool enabled);
	}

	private List<EventListener> _listener;
	private Collider[] _colliders;
	private VRUIElement[] _vruiElements;

	public bool disableOnStart;
	public VRUIElement parent;

	void Start() {
		_listener = new List<EventListener>();
		_colliders = gameObject.GetComponentsInChildren<Collider>();
		_vruiElements = gameObject.GetComponentsInChildren<VRUIElement>();

		Component[] comps = gameObject.GetComponents<Component>();
		foreach (Component comp in comps) {
			if (comp.GetType().GetInterface("EventListener") != null) {
				_listener.Add(comp as EventListener);
			}
		}
	}

	void OnDestroy() {
		if (VRUIManager.manager != null) {
			VRUIManager.manager.NotifyVRUIElementDestroyed(this);
		}
	}

	public void Enable(bool enable) {
		if (enabled != enable) {
			foreach (Collider c in _colliders) {
				c.enabled = enable;
			}
			foreach (VRUIElement e in _vruiElements) {
				e.enabled = enable;
			}

			foreach (EventListener listener in _listener) {
				listener.VRUIElementEnabled(this, enable);
			}
		}
	}

	// for VRUIManager
	public void HandleFocused(bool focused) {
		foreach (EventListener listener in _listener) {
			listener.VRUIElementFocused(this, focused);
		}
	}

	public void HandleInteraction(int deviceID, int elementID, int action) {
		foreach (EventListener listener in _listener) {
			listener.VRUIElementEventOccurred(this, deviceID, elementID, action);
		}
	}
}
