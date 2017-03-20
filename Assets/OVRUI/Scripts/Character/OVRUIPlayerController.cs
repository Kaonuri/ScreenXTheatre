using UnityEngine;
using System.Collections;

public abstract class OVRUIPlayerController : VRUIPlayerController {
	private VRUIInputDevice _touchpadDevice;
	private VRUIInputDevice _gamepadDevice;

	public bool enableTouchpad;
	public bool enableGamepad;

	protected abstract void OnStart();

	void Start() {
		OnStart();

		if (enableTouchpad) {
			if (VRUIManager.manager.GetInputDevice(OVRUITouchpadDevice.DeviceID, ref _touchpadDevice) == false) {
				_touchpadDevice = new OVRUITouchpadDevice();
				VRUIManager.manager.RegisterInputDevice(_touchpadDevice);
			}
		}
		if (enableGamepad) {
			if (VRUIManager.manager.GetInputDevice(OVRUIGamepadDevice.DeviceID, ref _gamepadDevice) == false) {
				_gamepadDevice = new OVRUIGamepadDevice();
				VRUIManager.manager.RegisterInputDevice(_gamepadDevice);
			}
		}
	}

	// implements VRUIPlayerController
	protected override float horizontal {
		get {
			if (Application.isEditor == false) {
				return Mathf.Max(-1.0f, Mathf.Min(1.0f, (enableTouchpad ? -_touchpadDevice.GetCurrentValue(OVRUITouchpadDevice.AxisDragX) : 0.0f) +
				                                  (enableGamepad ? _gamepadDevice.GetCurrentValue(OVRUIGamepadDevice.AxisLeftX) : 0.0f)));
			}
			return Input.GetAxis("Horizontal");
		}
	}
	
	protected override float vertical {
		get {
			if (Application.isEditor == false) {
				return Mathf.Max(-1.0f, Mathf.Min(1.0f, (enableTouchpad ? _touchpadDevice.GetCurrentValue(OVRUITouchpadDevice.AxisDragY) : 0.0f) +
				                                  (enableGamepad ? _gamepadDevice.GetCurrentValue(OVRUIGamepadDevice.AxisLeftY) : 0.0f)));
			}
			return Input.GetAxis("Vertical");
		}
	}
}
