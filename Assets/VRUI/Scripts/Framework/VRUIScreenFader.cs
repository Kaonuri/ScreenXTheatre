using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VRUIScreenFader : MonoBehaviour {
	private static VRUIScreenFader _instance;

	public static VRUIScreenFader screenFader {
		get {
			return _instance;
		}
	}

	private float _t;
	private Color _fadeFrom;
	private Color _fadeTo;
	private Color _clearColor;
    private List<VRUIScreenFadeCamera> _fadeCameras;

    public Material fadeMaterial;
    public Color fadeColor;
    public float fadeDuration;

	private void setScreenFadeCamerasColor(Color color) {
        fadeMaterial.SetColor("_TintColor", color);
	}

    private void enableScreenFadeCameras(bool enable) {
        foreach (VRUIScreenFadeCamera cam in _fadeCameras) {
            cam.enabled = enable;
        }
    }

	void Awake() {
		if (_instance != null) {
			throw new UnityException("ERROR : Only one VRUIScreenFader should exist in one scene.");
		}
		_instance = this;
		GameObject.DontDestroyOnLoad(gameObject);

		_clearColor = fadeColor;
		_clearColor.a = 0.0f;
		_t = -1.0f;
        _fadeCameras = new List<VRUIScreenFadeCamera>();
	}

	void Update() {
		if (0 <= _t && _t <= 1.0f) {
			setScreenFadeCamerasColor(Color.Lerp(_fadeFrom, _fadeTo, _t));
			_t += Time.deltaTime / (fadeDuration - 0.05f);
		}
		else if (_t > 1.0f) {
			setScreenFadeCamerasColor(_fadeTo);
			if (_fadeTo == _clearColor) {
                enableScreenFadeCameras(false);
			}
			_t = -1.0f;
		}
	}

	public bool isFading {
		get {
			return _t >= 0.0f;
		}
	}

	public void RegisterScreenFadeCamera(VRUIScreenFadeCamera cam) {
        if (_fadeCameras.Contains(cam) == false) {
            _fadeCameras.Add(cam);
            cam.fadeMaterial = fadeMaterial;
            if (isFading == false && _fadeTo == _clearColor) {
                cam.enabled = false;
            }
            else {
                cam.enabled = true;
            }
        }
	}

    public void UnregisterScreenFadeCamera(VRUIScreenFadeCamera cam) {
        _fadeCameras.Remove(cam);
        cam.fadeMaterial = null;
        cam.enabled = false;
    }

	public void FadeIn(bool immediately = false) {
        if (immediately) {
            _t = -1.0f;
			setScreenFadeCamerasColor(_clearColor);
            enableScreenFadeCameras(false);
		} 
		else {
			_fadeFrom = fadeColor;
			_fadeTo = _clearColor;
			_t = 0.0f;
		}
	}

	public void FadeOut(bool immediately = false) {
        enableScreenFadeCameras(true);

		if (immediately) {
			_t = -1.0f;
			setScreenFadeCamerasColor(fadeColor);
		}
		else {
			_fadeFrom = _clearColor;
			_fadeTo = fadeColor;
			_t = 0.0f;
		}
	}
}
