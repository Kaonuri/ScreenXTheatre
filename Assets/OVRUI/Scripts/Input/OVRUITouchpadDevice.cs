using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OVRUITouchpadDevice : VRUIInputDevice {
	public static int DeviceID = VRUI.HashValue("kr.co.clicked.OVRUI.VRUITouchpadDevice");
	public static int ButtonTap 		= 0;
	public static int ButtonHome 		= 1;
	public static int AxisDragX 		= 2;
	public static int AxisDragY 		= 3;
	public static int ActionSwipeUp 	= 5;
	public static int ActionSwipeDown 	= 6;
	public static int ActionSwipeLeft 	= 7;
	public static int ActionSwipeRight 	= 8;

	public class OVRUITapInputElement : VRUIInputDeviceElement {
		private const float TouchpadAspectRatio = 5.0f;

		protected override int id {
			get {
				return OVRUITouchpadDevice.ButtonTap;
			}
		}
		
		private bool _pressing;
		private float _pressTime;
		private float _lastPressTime;
		private bool _longPressNotified;
		private float _doubleTapTimeLimit;
		private float _longPressDuration;
		private Vector2 _lastDownPosition;
		private float _thresholdDistance;
		
		public OVRUITapInputElement(float thresholdDistance) {
			_thresholdDistance = thresholdDistance;
			reset();
		}

		private Vector2 scaledPosition(Vector2 pos) {
			return pos.x * Vector2.right + pos.y * Vector2.up * TouchpadAspectRatio;
		}

		private void updateDownPosition(Vector2 pos) {
			_lastDownPosition = scaledPosition(pos);
		}

		private float distanceFromLastDownPosition(Vector2 pos) {
			return Vector2.Distance(_lastDownPosition, scaledPosition(pos));
		}
		
		private void reset() {
			_pressing = false;
			_pressTime = _lastPressTime = 0.0f;
			_longPressNotified = false;
		}
		
		protected virtual float doubleTapTimeLimit {
			get {
				return VRUIManager.manager.doubleTapTimeLimit;
			}
		}
		
		protected virtual float longPressDuration {
			get {
				return VRUIManager.manager.longPressDuration;
			}
		}
		
		// implements VRUIInputDeviceElement
		public override float value {
			get {
				return _pressing ? 1.0f : 0.0f;
			}
		}
		
		public override void ProcessAndDispatchInputEvent(VRUIManager manager, int deviceID) {
			if (_pressing == false && Input.GetMouseButton(0)) {
				dispatchEvent(manager, deviceID, id, VRUIButtonInputElement.ActionPress);
				
				_pressing = true;
				_pressTime = Time.realtimeSinceStartup;
				_longPressNotified = false;
				updateDownPosition(Input.mousePosition);
			}
			else if (_pressing) {
				if (Input.GetMouseButton(0) == false) {
					if (_longPressNotified == false) {
						if (distanceFromLastDownPosition(Input.mousePosition) < _thresholdDistance) {
							if (Time.realtimeSinceStartup - _lastPressTime < doubleTapTimeLimit) {
								dispatchEvent(manager, deviceID, id, VRUIButtonInputElement.ActionDouble);
							}
							else {
								dispatchEvent(manager, deviceID, id, VRUIButtonInputElement.ActionSingle);
							}
						}
						_lastPressTime = Time.realtimeSinceStartup;
					}
					_pressing = false;
					
					dispatchEvent(manager, deviceID, id, VRUIButtonInputElement.ActionRelease);
				}
				else if (_longPressNotified == false && Time.realtimeSinceStartup - _pressTime > longPressDuration) {
					dispatchEvent(manager, deviceID, id, VRUIButtonInputElement.ActionLongPress);
					_longPressNotified = true;
				}
			}
		}
		
		public override void Reset(VRUIManager manager, int deviceID) {
			reset();
		}
	}

	public class OVRUIHomeInputElement : VRUIButtonInputElement {
		protected override int id {
			get {
				return OVRUITouchpadDevice.ButtonHome;
			}
		}

		protected override bool isButtonUp() {
			return Input.GetKey(KeyCode.Escape) == false;
		}

		protected override bool isButtonDown() {
			return Input.GetKey(KeyCode.Escape);
		}
	}

	public void SimulateSingleTap() {
		SimulateTap(true);
		SimulateTap(false);
	}
	
	public void SimulateTap(bool pressed) {
		if (VRUIManager.manager.interactionEnabled) {
			if (pressed) {
				VRUIManager.manager.NotifyInputEvent(DeviceID, ButtonTap, VRUIButtonInputElement.ActionPress);
			}
			else {
				VRUIManager.manager.NotifyInputEvent(DeviceID, ButtonTap, VRUIButtonInputElement.ActionRelease);
			}
		}
	}

	// implements VRUIInputDevice
	public override int id {
		get {
			return DeviceID;
		}
	}

	protected override Dictionary<int, VRUIInputDeviceElement> CreateInputElements (VRUIManager manager) {
		Dictionary<int, VRUIInputDeviceElement> result = new Dictionary<int, VRUIInputDeviceElement>();
		result.Add(ButtonTap, new OVRUITapInputElement(40.0f));
		result.Add(ButtonHome, new OVRUIHomeInputElement());
		result.Add(AxisDragX, new MouseDragAxisInputElement(AxisDragX, MouseDragAxisInputElement.Direction.Horizontal, 200.0f));
		result.Add(AxisDragY, new MouseDragAxisInputElement(AxisDragY, MouseDragAxisInputElement.Direction.Vertical, 200.0f));

		return result;
	}
}
