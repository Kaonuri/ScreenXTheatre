using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(Animator))]

public class ScreenXLightRenderer : MonoBehaviour
{
	private const string AnimatorParamTurnUp = "TurnUp";

	private Animator _thisAnimator;
	private bool _screenRefelction;

    [SerializeField] private VideoPlayer videoPlayer;

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

	private void Awake()
    {
		_thisAnimator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
		foreach (Material mat in materials)
        {
			if (mat != null)
            {
				mat.SetTexture("_ColorReference", _screenRefelction ? videoPlayer.texture : null);

				if (videoPlayer.texture != null)
                {
                    mat.SetInt("_ColorRefMipLevel", 0);
				}

				mat.SetFloat("_BlendTex", valueOnRange(blendTexRange, tBlendTex));
				mat.SetFloat("_Intensity", valueOnRange(intensityRange, tIntensity));
			}
		}
	}

	public void TurnUpLights()
    {
		_thisAnimator.SetBool(AnimatorParamTurnUp, true);
	}

	public void TurnDownLights()
    {
		_thisAnimator.SetBool(AnimatorParamTurnUp, false);
	}

	public void EnableScreenReflection(bool enable)
    {
		_screenRefelction = enable;
	}
}
