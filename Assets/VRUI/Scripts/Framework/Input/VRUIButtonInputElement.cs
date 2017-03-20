using UnityEngine;
using System.Collections;

public abstract class VRUIButtonInputElement : VRUIInputDeviceElement {
	public static int ActionPress       = 0;
	public static int ActionRelease     = 1;
	public static int ActionDouble      = 2;
	public static int ActionSingle      = 3;
	public static int ActionLongPress   = 4;

	private bool _pressing;
	private float _pressTime;
	private float _lastPressTime;
	//private float _lastLongPressTime;
	private bool _longPressNotified;
	private float _doubleTapTimeLimit;
	private float _longPressDuration;

	public VRUIButtonInputElement() {
		reset();
	}

	private void reset() {
		_pressing = false;
		_pressTime = _lastPressTime = /*_lastLongPressTime =*/ 0.0f;
		_longPressNotified = false;
	}
	
	protected abstract bool isButtonDown();
	protected abstract bool isButtonUp();

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
		if (_pressing == false && isButtonDown()) {
			dispatchEvent(manager, deviceID, id, ActionPress);

			_pressing = true;
			_pressTime = Time.realtimeSinceStartup;
			_longPressNotified = false;
		}
		else if (_pressing) {
			if (isButtonUp()) {
				if (_longPressNotified == false) {
					if (Time.realtimeSinceStartup - _lastPressTime < doubleTapTimeLimit) {
						dispatchEvent(manager, deviceID, id, ActionDouble);
					}
					else {
						dispatchEvent(manager, deviceID, id, ActionSingle);
					}
					_lastPressTime = Time.realtimeSinceStartup;
				}
				_pressing = false;

				dispatchEvent(manager, deviceID, id, ActionRelease);
			}
			else if (_longPressNotified == false && Time.realtimeSinceStartup - _pressTime > longPressDuration) {
				dispatchEvent(manager, deviceID, id, ActionLongPress);
				_longPressNotified = true;
				//_lastLongPressTime = Time.realtimeSinceStartup;
			}
		}
	}

	public override void Reset(VRUIManager manager, int deviceID) {
		reset();
	}
}
