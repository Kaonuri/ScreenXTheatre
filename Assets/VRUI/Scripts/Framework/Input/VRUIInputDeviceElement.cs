using UnityEngine;
using System.Collections;

public abstract class VRUIInputDeviceElement {
	protected void dispatchEvent(VRUIManager manager, int deviceID, int elementID, int action) {
		manager.NotifyInputEvent(deviceID, elementID, action);
	}

	protected abstract int id { get; }
	public abstract float value { get; }
	public abstract void ProcessAndDispatchInputEvent(VRUIManager manager, int deviceID);
	public abstract void Reset(VRUIManager manager, int deviceID);
}
