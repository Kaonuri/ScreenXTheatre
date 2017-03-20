using UnityEngine;
using System.Collections;

public class OVRUIPlatformMenu : MonoBehaviour, VRUIEventHandler {
	private static OVRUIPlatformMenu _instance;

	public static OVRUIPlatformMenu menu {
		get {
			return _instance;
		}
	}

	private const float DelayToShowCursor = 0.25f;

	private GameObject _instantiatedCursorTimer = null;
	private Material _cursorTimerMaterial = null;
	private bool _quitOnBackClick;
	private float _timeHomeDown;

	public GameObject cursorTimer;
	public Color cursorTimerColor = new Color(0.0f, 0.643f, 1.0f, 1.0f);	// set default color to same as native cursor timer
	public float fixedDepth = 3.0f;

	private void updateCursor(float timerRotateRatio) {
		timerRotateRatio = Mathf.Clamp( timerRotateRatio, 0.0f, 1.0f );
		if (_instantiatedCursorTimer != null) {
			_instantiatedCursorTimer.GetComponent<Renderer>().enabled = true;

			float rampOffset = Mathf.Clamp(1.0f - timerRotateRatio, 0.0f, 1.0f);
			_cursorTimerMaterial.SetFloat ( "_ColorRampOffset", rampOffset );

			Vector3 cameraForward = Camera.main.transform.forward;
			Vector3 cameraPos = Camera.main.transform.position;
			_instantiatedCursorTimer.transform.position = cameraPos + (cameraForward * fixedDepth);
			_instantiatedCursorTimer.transform.forward = cameraForward;
		}
	}
	
	private void resetCursor() {
		if (_instantiatedCursorTimer != null) {
			_cursorTimerMaterial.SetFloat("_ColorRampOffset", 1.0f);
			_instantiatedCursorTimer.GetComponent<Renderer>().enabled = false;
		}
	}

	void Awake() {
		if (_instance != null) {
			throw new UnityException("[ERROR] There should be only one instance of OVRUIPlatformMenu.");
		}
		_instance = this;

		if ((cursorTimer != null) && (_instantiatedCursorTimer == null))  {
			_instantiatedCursorTimer = Instantiate(cursorTimer) as GameObject;
			_instantiatedCursorTimer.transform.parent = transform;
			if (_instantiatedCursorTimer != null) {
				_cursorTimerMaterial = _instantiatedCursorTimer.GetComponent<Renderer>().material;
				_cursorTimerMaterial.SetColor ( "_Color", cursorTimerColor ); 
				_instantiatedCursorTimer.GetComponent<Renderer>().enabled = false;
			}
		}
		_timeHomeDown = -1.0f;
	}

	void Start() {
		VRUIManager.manager.RegisterHandler(this);
	}

	void LateUpdate() {
		if (_timeHomeDown > 0.0f && (Time.realtimeSinceStartup - _timeHomeDown >= DelayToShowCursor)) {
			updateCursor((Time.realtimeSinceStartup - _timeHomeDown - DelayToShowCursor) / 
			             (VRUIManager.manager.longPressDuration - DelayToShowCursor));
		}
	}

	void OnApplicationPause(bool pauseStatus) {
		if (!pauseStatus) {
			resetCursor();
			_timeHomeDown = -1.0f;
		}
	}
	
	void OnDestroy() {
		if (_cursorTimerMaterial != null) {
			Destroy(_cursorTimerMaterial);
		}
		if (VRUIManager.manager != null) {
			VRUIManager.manager.UnregisterHandler(this);
		}
	}

	public bool quitOnBackClick {
		get {
			return _quitOnBackClick;
		}
		set {
			_quitOnBackClick = value;
		}
	}

	public void ShowConfirmQuitMenu() {
		Debug.Log("[PlatformUI-ConfirmQuit] Showing @ " + Time.time);
#if UNITY_ANDROID && !UNITY_EDITOR
		OVRManager.PlatformUIConfirmQuit();
#endif
	}
	
	public void ShowGlobalMenu() {
		Debug.Log("[PlatformUI-Global] Showing @ " + Time.time);
#if UNITY_ANDROID && !UNITY_EDITOR
		OVRManager.PlatformUIGlobalMenu();
#endif
	}

	// implements VRUIEventHandler
	public bool VRUIEventOccurred(VRUIManager manager, VRUIElement pointingElement, int deviceID, int elementID, int action) {
		if (deviceID == OVRUITouchpadDevice.DeviceID) {
			if (elementID == OVRUITouchpadDevice.ButtonHome) {
				if (action == VRUIButtonInputElement.ActionPress) {
					_timeHomeDown = Time.realtimeSinceStartup;
				}
				else if (action == VRUIButtonInputElement.ActionRelease) {
					resetCursor();
					_timeHomeDown = -1.0f;
				}
				else if (action == VRUIButtonInputElement.ActionSingle && _quitOnBackClick) {
					ShowConfirmQuitMenu();
					return true;
				}
				else if (action == VRUIButtonInputElement.ActionLongPress) {
					ShowGlobalMenu();
					return true;
				}
			}
		}
		return false;
	}
}
