using UnityEngine;
using System.Collections;

public class VRUIAudioEventParam {
	public delegate void VRUIAudioEventParamChangedDelegate(VRUIAudioEventParam param, float value);

	private VRUIAudioEventParamChangedDelegate _delegate;
	private float _targetValue;
	private float _value;
	private float _changeDelta;

	public VRUIAudioEventParam(VRUIAudioEventParamChangedDelegate aDelegate, float initial, float changeDelta) {
		_delegate = aDelegate;
		_value = _targetValue = initial;
		_changeDelta = changeDelta;
	}

	public float targetValue {
		get {
			return _targetValue;
		}
		set {
			_targetValue = value;
			if (_changeDelta <= 0) {
				_value = value;
				_delegate(this, value);
			}
		}
	}

	public float currentValue {
		get {
			return _value;
		}
		set {
			if (_value != value) {
				_delegate(this, value);
			}
			_value = value;
		}
	}

	public float changeSpeed {
		get {
			return _changeDelta;
		}
		set {
			_changeDelta = value;
			if (_changeDelta <= 0) {
				currentValue = _targetValue;
			}
		}
	}

	public void Update() {
		if (_value != _targetValue) {
			if (_changeDelta > 0.0f) {
				if (Mathf.Abs(_targetValue - _value) < _changeDelta * Time.deltaTime) {
					_value = _targetValue;
				}
				else {
					_value += Mathf.Sign(_targetValue - _value) * _changeDelta * Time.deltaTime;
				}
			}
			else {
				_value = _targetValue;
			}
			_delegate(this, _value);
		}
	}
}
