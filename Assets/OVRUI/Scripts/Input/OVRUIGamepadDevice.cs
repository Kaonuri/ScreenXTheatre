using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OVRUIGamepadDevice : VRUIInputDevice {
	public static int DeviceID = VRUI.HashValue("kr.co.clicked.OVRUI.VRUIGamepadDevice");

	public static int ButtonA = 0;
	public static int ButtonB = 1;
	public static int ButtonX = 2;
	public static int ButtonY = 3;
	public static int AxisLeftX = 4;
	public static int AxisLeftY = 5;

	private class GamepadButtonInputElement : VRUIButtonInputElement {
		public GamepadButtonInputElement(int id, OVRGamepadController.Button button) {
			_id = id;
			_button = button;
		}

		private int _id;
		private OVRGamepadController.Button _button;

		protected override int id {
			get {
				return _id;
			}
		}

		protected override bool isButtonDown() {
			return OVRGamepadController.GPC_GetButton(_button);
		}

		protected override bool isButtonUp() {
			return OVRGamepadController.GPC_GetButton(_button) == false;
		}
	}

	private class GamepadAxisInputElement : VRUIInputDeviceElement {
		public GamepadAxisInputElement(int id, OVRGamepadController.Axis axis) {
			_id = id;
			_axis = axis;
		}

		private int _id;
		private OVRGamepadController.Axis _axis;

		protected override int id {
			get {
				return _id;
			}
		}

		public override float value { 
			get {
				return OVRGamepadController.GPC_GetAxis(_axis);
			}
		}

		public override void ProcessAndDispatchInputEvent(VRUIManager manager, int deviceID) {}
		public override void Reset(VRUIManager manager, int deviceID) {}
	}

	public override int id {
		get {
			return DeviceID;
		}
	}

	protected override Dictionary<int, VRUIInputDeviceElement> CreateInputElements(VRUIManager manager) {
		Dictionary<int, VRUIInputDeviceElement> result = new Dictionary<int, VRUIInputDeviceElement>();
		result.Add(ButtonA, new GamepadButtonInputElement(ButtonA, OVRGamepadController.Button.A));
		result.Add(ButtonB, new GamepadButtonInputElement(ButtonB, OVRGamepadController.Button.B));
		result.Add(ButtonX, new GamepadButtonInputElement(ButtonX, OVRGamepadController.Button.X));
		result.Add(ButtonY, new GamepadButtonInputElement(ButtonY, OVRGamepadController.Button.Y));
		result.Add(AxisLeftX, new GamepadAxisInputElement(AxisLeftX, OVRGamepadController.Axis.LeftXAxis));
		result.Add(AxisLeftY, new GamepadAxisInputElement(AxisLeftY, OVRGamepadController.Axis.LeftYAxis));

		return result;
	}
}
