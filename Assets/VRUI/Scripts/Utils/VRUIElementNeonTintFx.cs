using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(VRUIElement))]

public class VRUIElementNeonTintFx : MonoBehaviour, VRUIElement.EventListener {
	private Color _unfocused;

	public Color focused;

	private Material meshMaterial {
		get {
			return gameObject.GetComponent<MeshRenderer>().material;
		}
	}

	void Start() {
		_unfocused = meshMaterial.GetColor("_Color");
	}

	// handle VRUIElement
	public void VRUIElementFocused() {
		meshMaterial.SetColor("_Color", focused);
	}
	
	public void VRUIElementUnfocused() {
		meshMaterial.SetColor("_Color", _unfocused);
	}

	// implements VRUIElement.EventListener
	public void VRUIElementFocused(VRUIElement element, bool focused) {
		meshMaterial.SetColor("_Color", focused ? this.focused : _unfocused);
	}

	public void VRUIElementEventOccurred(VRUIElement element, int deviceID, int elementID, int action) {
		// do nothing
	}

	public void VRUIElementEnabled(VRUIElement element, bool enabled) {
		// do nothing
	}
}
