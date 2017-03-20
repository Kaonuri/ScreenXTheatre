using UnityEngine;
using System.Collections;

[RequireComponent(typeof(VRUIElement))]
public class OVRIntField : MonoBehaviour, VRUIElement.EventListener {
	private TextMesh _field;
	private int _currentValue;
	private int _scrollBaseValue;
	private bool _scrolling;

	public int minValue;
	public int maxValue;
	public int deltaPerOneFullScroll;

	private void startScroll() {
		_scrolling = true;
		_scrollBaseValue = _currentValue;
	}

	private void stopScroll() {
		_scrolling = false;
	}

	private void setCurrentValue(int value) {
		_currentValue = Mathf.Max(minValue, Mathf.Min(maxValue, value));
		_field.text = _currentValue.ToString();
	}

	void Start() {
		_field = gameObject.GetComponentInChildren<TextMesh>();
		setCurrentValue(minValue);
	}

	void Update() {
		if (_scrolling) {
			float axis = VRUIManager.manager.GetCurrentValue(OVRUITouchpadDevice.DeviceID, OVRUITouchpadDevice.AxisDragY);
			setCurrentValue(_scrollBaseValue + (int)(axis * deltaPerOneFullScroll));
		}
	}

	public int currentValue {
		get {
			return _currentValue;
		}
		set {
			setCurrentValue(value);
		}
	}

	// implements VRUIElement.EventListener
	public void VRUIElementFocused(VRUIElement element, bool focused) {
		if (focused == false) {
			stopScroll();
		}
	}

	public void VRUIElementEventOccurred(VRUIElement element, int deviceID, int elementID, int action) {
		if (deviceID == OVRUITouchpadDevice.DeviceID) {
			if (elementID == OVRUITouchpadDevice.ButtonTap) {
				if (action == VRUIButtonInputElement.ActionPress) {
					startScroll();
				}
				else if (action == VRUIButtonInputElement.ActionRelease) {
					stopScroll();
				}
			}
		}
	}

	public void VRUIElementEnabled(VRUIElement element, bool enabled) {
		// do nothing
	}
}
