using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class VRUIAudioEvent : MonoBehaviour {
	private AudioSource _source;
	private AudioLowPassFilter _lowpassFilter;
	private float _volume;
	private float _lowpassDisabledFreq;
	private VRUIAudioEventParam _paramPlay;
	private VRUIAudioEventParam _paramLowpass;

	public float fadeInOutDuration;
	public float lowpassFadeDuration;
	public float lowpassCutoffFreq;

	void Start() {
		_source = gameObject.GetComponent<AudioSource>();
		_paramPlay = new VRUIAudioEventParam(VRUIAudioEventParamPlaybackChanged, 0.0f, 
		                                     fadeInOutDuration > 0.0f ? 1.0f / fadeInOutDuration : 0.0f);
		_volume = _source.volume;

		_lowpassFilter = gameObject.GetComponent<AudioLowPassFilter>();
		if (_lowpassFilter != null) {
			_lowpassDisabledFreq = _lowpassFilter.cutoffFrequency;
			_paramLowpass = new VRUIAudioEventParam(VRUIAudioEventParamLowpassChanged, _lowpassDisabledFreq, 
			                                        lowpassFadeDuration > 0.0f ? Mathf.Abs(_lowpassDisabledFreq - lowpassCutoffFreq) / lowpassFadeDuration : 0.0f);
		}
	}

	void Update() {
		_paramPlay.Update();
		if (_paramPlay.currentValue > 0.0f && _source.isPlaying == false) {
			Stop();
		}
		if (_paramLowpass != null) {
			_paramLowpass.Update();
		}
	}

	public void Play(bool forceToReplay = true) {
		if (forceToReplay) {
			_paramPlay.currentValue = 0.0f;
		}
		_paramPlay.targetValue = 1.0f;
	}

	public void Stop() {
		_paramPlay.targetValue = 0.0f;
	}

	public void EnableLowpass(bool enable) {
		if (_paramLowpass != null) {
			_paramLowpass.targetValue = enable ? lowpassCutoffFreq : _lowpassDisabledFreq;
		}
	}

	// implements VRUIAudioEventParam delegate
	void VRUIAudioEventParamPlaybackChanged(VRUIAudioEventParam param, float value) {
		_source.volume = value * _volume;
		if (value > 0.0f && _source.isPlaying == false) {
			_source.Play();
		}
		else if (value <= 0.001f && _source.isPlaying) {
			_source.Stop();
		}
	}

	void VRUIAudioEventParamLowpassChanged(VRUIAudioEventParam param, float value) {
		if (_lowpassFilter != null) {
			_lowpassFilter.cutoffFrequency = value;
		}
	}
}
