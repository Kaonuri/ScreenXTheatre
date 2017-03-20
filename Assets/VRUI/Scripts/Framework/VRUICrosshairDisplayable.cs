using UnityEngine;
using System.Collections;

public class VRUICrosshairDisplayable : MonoBehaviour, VRUIElement.EventListener {
	public VRUICrosshair3D crosshair;

	void Start() {
		crosshair.gameObject.SetActive(false);
	}

    // implements VRUIElement.EventListener
    public void VRUIElementFocused(VRUIElement element, bool focused) {
        if (focused && crosshair.mode == VRUICrosshair3D.CrosshairMode.FixedDepth) { 
			crosshair.fixedDepthRefObject = gameObject.transform;
        }
        crosshair.gameObject.SetActive(focused);
    }

    public void VRUIElementEventOccurred(VRUIElement element, int deviceID, int elementID, int action) { 
        // do nothing
    }

	public void VRUIElementEnabled(VRUIElement element, bool enabled) {
		// do nothing
	}
}
