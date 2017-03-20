using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(UnityVideoTexture))]

public class ScreenXLightRenderer : MonoBehaviour {
	private const string AnimatorParamTurnUp = "TurnUp";

	private Animator _thisAnimator;
	private UnityVideoTexture _videoColorSampler;
	private bool _screenRefelction;

	public Material[] materials;
	public Vector2 blendTexRange;
	public Vector2 intensityRange;

	// for animator
	[HideInInspector]
	public float tBlendTex;

	[HideInInspector]
	public float tIntensity;

	private float valueOnRange(Vector2 range, float t) {
		return (1.0f - t) * range.x + t * range.y;
	}

	void Awake() {
		_thisAnimator = gameObject.GetComponent<Animator>();
		_videoColorSampler = gameObject.GetComponent<UnityVideoTexture>();
	}
		
	void Update() {
		foreach (Material mat in materials) {
			if (mat != null) {
				mat.SetTexture("_ColorReference", _screenRefelction ? _videoColorSampler.texture : null);
				if (_videoColorSampler.texture != null) {
					if (_videoColorSampler.texture.GetType() == typeof(Texture2D)) {
						mat.SetInt("_ColorRefMipLevel", (_videoColorSampler.texture as Texture2D).mipmapCount - 2);
					}
					else {
						mat.SetInt("_ColorRefMipLevel", 0);
					}
				}
				mat.SetFloat("_BlendTex", valueOnRange(blendTexRange, tBlendTex));
				mat.SetFloat("_Intensity", valueOnRange(intensityRange, tIntensity));
			}
		}
	}

	public void TurnUpLights() {
		_thisAnimator.SetBool(AnimatorParamTurnUp, true);
	}

	public void TurnDownLights() {
		_thisAnimator.SetBool(AnimatorParamTurnUp, false);
	}

	public void EnableScreenReflection(bool enable) {
		_screenRefelction = enable;
	}
}
