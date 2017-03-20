using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OVRUIInputSimulatorOnEditor : MonoBehaviour {
#if UNITY_EDITOR
	public class TouchpadTapSimulationElement : VRUIButtonInputElement {
		protected override int id {
			get {
				return OVRUITouchpadDevice.ButtonTap;
			}
		}
		
		protected override bool isButtonDown() {
			return Input.GetKeyDown(KeyCode.Space);
		}
		
		protected override bool isButtonUp() {
			return Input.GetKeyUp(KeyCode.Space);
		}
	}
	
	public class TouchpadTapSimulationDevice : VRUIInputDevice {
		public override int id {
			get {
				return OVRUITouchpadDevice.DeviceID;
			}
		}
		
		protected override Dictionary<int, VRUIInputDeviceElement> CreateInputElements(VRUIManager manager) {
			Dictionary<int, VRUIInputDeviceElement> result = new Dictionary<int, VRUIInputDeviceElement>();
			result.Add(OVRUITouchpadDevice.ButtonTap, new TouchpadTapSimulationElement());
			
			return result;
		}
	}
	
	private TouchpadTapSimulationDevice _touchpadSimulation;

	void Start() {
		_touchpadSimulation = new TouchpadTapSimulationDevice();
		VRUIManager.manager.RegisterInputDevice(_touchpadSimulation);
	}

	void OnDestroy() {
		if (VRUIManager.manager != null) {
			VRUIManager.manager.UnregisterInputDevice(_touchpadSimulation);
		}
	}
#endif
}
