using UnityEngine;
using System.Collections;

[RequireComponent(typeof(VRUIElement))]
[RequireComponent(typeof(MeshRenderer))]
public class VRUIElementSwitchMaterialFx : MonoBehaviour, VRUIElement.EventListener {
	private MeshRenderer _renderer;

	public Material normal;
	public Material focused;
	public Material disabled;
	public MeshRenderer[] attachedRenderers;

	void Start() {
		_renderer = gameObject.GetComponent<MeshRenderer>();
	}

	// implements VRUIElement.EventListener
	public void VRUIElementFocused(VRUIElement element, bool focused) {
		_renderer.sharedMaterial = focused ? this.focused : normal;
		foreach (MeshRenderer attached in attachedRenderers) {
			attached.sharedMaterial = focused ? this.focused : normal;
		}
	}

	public void VRUIElementEventOccurred(VRUIElement element, int deviceID, int elementID, int action) {
		// do nothing
	}

	public void VRUIElementEnabled(VRUIElement element, bool enabled) {
		_renderer.sharedMaterial = enabled ? normal : disabled;
		foreach (MeshRenderer attached in attachedRenderers) {
			attached.sharedMaterial = enabled ? normal : disabled;
		}
	}
}
