using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VRUIManager : MonoBehaviour {
	private static VRUIManager _instance;
	public static VRUIManager manager {
		get {
			return _instance;
		}
	}

	public static VRUICamera currentVRUICamera {
		get {
			if (VRUIManager.manager != null) {
				return VRUIManager.manager.vruiCamera;
			}
			return null;
		}
	}

	public enum HandlerPriority {
		High,
		Low
	}

	private bool _interactionEnabled;
	private VRUICamera _camera;
	private RaycastHit _hitCached;
	private ArrayList _elementCache;
	private ArrayList _currentPointingElements;
	private Dictionary<HandlerPriority, ArrayList> _handlers;
	private List<VRUIInputDevice> _inputDevices;

	public float doubleTapTimeLimit;
	public float longPressDuration;
	public bool enableInteractionOnStart;

	private VRUIElement lastDepthPointingElement() {
		return _currentPointingElements.Count > 0 ? _currentPointingElements[0] as VRUIElement : null;
	}

	private void getCurrentPointingElement(ArrayList result) {
		result.Clear();

		if (_camera != null && Physics.Raycast(new Ray(_camera.position, _camera.forward), out _hitCached)) {
			GameObject hit = _hitCached.collider.gameObject;
			while (hit != null) {
				VRUIElement element = hit.GetComponent<VRUIElement>();
				if (element != null && element.enabled) {
					while (element != null && element.enabled) {
						result.Add(element);
						element = element.parent;
					}
					break;
				}
				hit = (hit.transform.parent != null) ? hit.transform.parent.gameObject : null;
			}
		}
	}

	private void evaluateEvents() {
		foreach (VRUIInputDevice device in _inputDevices) {
			device.ProcessAndDispatchInputEvent(this);
		}
	}

	private void updateCurrentPointingElement() {
		getCurrentPointingElement(_elementCache);
		foreach (VRUIElement element in _currentPointingElements) {
			if (_elementCache.Contains(element) == false) {
				element.HandleFocused(false);
			}
		}
		foreach (VRUIElement element in _elementCache) {
			if (_currentPointingElements.Contains(element) == false) {
				element.HandleFocused(true);
			}
		}

		ArrayList swap = _elementCache;
		_elementCache = _currentPointingElements;
		_currentPointingElements = swap;
	}

	void Awake() {
		if (_instance != null) {
			throw new UnityException("ERROR : Only one VRUIManager must exist in a scene.");
		}
		_instance = this;
		GameObject.DontDestroyOnLoad(gameObject);

		_elementCache = new ArrayList();
		_currentPointingElements = new ArrayList();

		_inputDevices = new List<VRUIInputDevice>();

		_handlers = new Dictionary<HandlerPriority, ArrayList>();
		_handlers.Add(HandlerPriority.High, new ArrayList());
		_handlers.Add(HandlerPriority.Low, new ArrayList());
	}

	void Start() {
		_interactionEnabled = enableInteractionOnStart;
	}

	void Update() {
		if (_interactionEnabled) {
			updateCurrentPointingElement();
			evaluateEvents();
		}
	}

	public bool interactionEnabled {
		get {
			return _interactionEnabled;
		}
		set {
			_interactionEnabled = value;
			if (value) {
				foreach (VRUIInputDevice device in _inputDevices) {
					device.Reset(this);
				}
			}
		}
	}

	public VRUIElement currentPointingElement {
		get {
			return lastDepthPointingElement();
		}
	}

	public VRUICamera vruiCamera {
		get {
			return _camera;
		}
	}

	public void SetCamera(VRUICamera cam) {
		_camera = cam;
	}

	public void UnsetCamera() {
		_camera = null;
	}

	public void RegisterHandler(VRUIEventHandler handler, HandlerPriority priority = HandlerPriority.Low) {
		_handlers[priority].Add(handler);
	}

	public void UnregisterHandler(VRUIEventHandler handler) {
		_handlers[HandlerPriority.High].Remove(handler);
		_handlers[HandlerPriority.Low].Remove(handler);
	}

	public void RegisterInputDevice(VRUIInputDevice device) {
		if (_inputDevices.Contains(device) == false) {
			_inputDevices.Add(device);
			device.HandleDeviceRegistration(this);
		}
	}

	public void UnregisterInputDevice(VRUIInputDevice device) {
		if (_inputDevices.Contains(device)) {
			device.HandleDeviceUnregistration(this);
			_inputDevices.Remove(device);
		}
	}

	public void UnregisterInputDevice(int deviceID) {
		List<VRUIInputDevice> target = new List<VRUIInputDevice>();
		foreach (VRUIInputDevice device in _inputDevices) {
			if (device.id == deviceID) {
				target.Add(device);
			}
		}
		foreach (VRUIInputDevice device in target) {
			device.HandleDeviceUnregistration(this);
			_inputDevices.Remove(device);
		}
	}

	public bool GetInputDevice(int deviceID, ref VRUIInputDevice result) {
		foreach (VRUIInputDevice device in _inputDevices) {
			if (device.id == deviceID) {
				result = device;
				return true;
			}
		}
		return false;
	}

	public float GetCurrentValue(int deviceID, int elementID) {
		foreach (VRUIInputDevice device in _inputDevices) {
			if (device.id == deviceID) {
				return device.GetCurrentValue(elementID);
			}
		}
		return 0.0f;
	}

	// for VRUIElement
	public void NotifyVRUIElementDestroyed(VRUIElement element) {
		_currentPointingElements.Remove(element);
	}

	// for VRUIInputDeviceElement
	public void NotifyInputEvent(int deviceID, int elementID, int action) {
		if (lastDepthPointingElement() != null && lastDepthPointingElement().enabled) {
			lastDepthPointingElement().HandleInteraction(deviceID, elementID, action);
		}
		foreach (VRUIEventHandler handler in _handlers[HandlerPriority.High]) {
			if (handler.VRUIEventOccurred(this, lastDepthPointingElement(), deviceID, elementID, action)) {
				return;
			}
		}
		foreach (VRUIEventHandler handler in _handlers[HandlerPriority.Low]) {
			if (handler.VRUIEventOccurred(this, lastDepthPointingElement(), deviceID, elementID, action)) {
				return;
			}
		}
	}
}
