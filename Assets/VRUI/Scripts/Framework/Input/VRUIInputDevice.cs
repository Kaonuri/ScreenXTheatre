using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class VRUIInputDevice {
	private Dictionary<int, VRUIInputDeviceElement> _elements;

	public abstract int id { get; }
	protected abstract Dictionary<int, VRUIInputDeviceElement> CreateInputElements(VRUIManager manager);
	protected virtual void OnRegister(VRUIManager manager) {}
	protected virtual void OnUnregister(VRUIManager manager) {}
	protected virtual void OnPreReset(VRUIManager manager) {}

	protected Dictionary<int, VRUIInputDeviceElement> elements {
		get {
			return _elements;
		}
	}

	public virtual float GetCurrentValue(int elementID) {
		if (_elements != null && _elements.ContainsKey(elementID)) {
			return _elements[elementID].value;
		}
		return 0.0f;
	}

	// for VRUIManager
	public void HandleDeviceRegistration(VRUIManager manager) {
		_elements = CreateInputElements(manager);

		OnRegister(manager);
	}

	public void HandleDeviceUnregistration(VRUIManager manager) {
		OnUnregister(manager);
	}

	public void ProcessAndDispatchInputEvent(VRUIManager manager) {
		foreach (int key in _elements.Keys) {
			_elements[key].ProcessAndDispatchInputEvent(manager, id);
		}
	}

	public void Reset(VRUIManager manager) {
		OnPreReset(manager);

		foreach (int key in _elements.Keys) {
			_elements[key].Reset(manager, id);
		}
	}
}
