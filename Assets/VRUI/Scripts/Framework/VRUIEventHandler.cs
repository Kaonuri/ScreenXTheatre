using UnityEngine;
using System.Collections;

public interface VRUIEventHandler {
	bool VRUIEventOccurred(VRUIManager manager, VRUIElement pointingElement, int deviceID, int elementID, int action);
}
