using UnityEngine;
using System.Collections;

public class UnityVideoTexture : MonoBehaviour {	
#if UNITY_ANDROID && !UNITY_EDITOR
	private System.IntPtr _videoTexture;
	private Texture2D _texture;
#endif

	public int width;
	public int height;
	public bool generateMipmap;

	void Awake() {
#if UNITY_ANDROID && !UNITY_EDITOR
		_videoTexture = System.IntPtr.Zero;
		_texture = new Texture2D(width, height, TextureFormat.ARGB32, generateMipmap);
#endif
	}

	void Start() {
#if UNITY_ANDROID && !UNITY_EDITOR
		_videoTexture = UnityVideoTexturePlugin.AddVideoTexture(_texture.GetNativeTexturePtr(),
		                                                        _texture.width,
		                                                        _texture.height,
		                                                        generateMipmap);
#endif
		UnityVideoPlayer.player.RegisterVideoTexture(this);
	}

	void OnDestroy() {
		UnityVideoPlayer.player.UnregisterVideoTexture(this);

#if UNITY_ANDROID && !UNITY_EDITOR
		if (_videoTexture != System.IntPtr.Zero) {
			UnityVideoTexturePlugin.RemoveVideoTexture(_videoTexture);
		}
#endif
	}

	public Texture texture {
		get {
#if UNITY_ANDROID && !UNITY_EDITOR
			return _texture;
#else
			return UnityVideoPlayer.player.movieTexture;
#endif
		}
	}

	public void Apply() {
#if UNITY_ANDROID && !UNITY_EDITOR
		_texture.filterMode = generateMipmap ? FilterMode.Trilinear : FilterMode.Bilinear;
		_texture.Apply(generateMipmap);
#endif
	}
}
