using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnityVideoPlayer : MonoBehaviour {
	private static UnityVideoPlayer _instance;

	public static UnityVideoPlayer player {
		get {
			return _instance;
		}
	}

	public interface EventListener {
		void UnityVideoPlayerAboutToLoad(UnityVideoPlayer player);
		void UnityVideoPlayerPlaybackReady(UnityVideoPlayer player);
		void UnityVideoPlayerAboutToPlay(UnityVideoPlayer player, int videoWidth, int videoHeight);
		void UnityVideoPlayerBufferingStart(UnityVideoPlayer player);
		void UnityVideoPlayerBufferingEnd(UnityVideoPlayer player);
		void UnityVideoPlayerPlaybackCompleted(UnityVideoPlayer player);
		void UnityVideoPlayerErrorOccurred(UnityVideoPlayer player, string message);
	}
	
	public enum Status {
		Idle,
		Loading,
		Ready,
		Playing
	}
	
	private EventListener _listener;
	private Status _status;
	private List<UnityVideoTexture> _videoTextures;
	private int _videoWidth;
	private int _videoHeight;

#if UNITY_EDITOR || !UNITY_ANDROID
	private MovieTexture _movieTexture;
	private WWW _www;
#endif

	void Awake() {
		if (_instance != null) {
			throw new UnityException("[ERROR] There must be only one instance of UnityVideoTextureManager.");
		}
		_instance = this;
		GameObject.DontDestroyOnLoad(gameObject);

		_videoTextures = new List<UnityVideoTexture>();
		_status = Status.Idle;

		UnityVideoTexturePlugin.LoadPlugin();
		UnityVideoTexturePlugin.IssueRenderInit();
	}

	void Update() {
		UnityVideoTexturePlugin.IssueRenderUpdate();

#if UNITY_EDITOR || !UNITY_ANDROID
		if (_status == Status.Loading && _movieTexture.isReadyToPlay) {
			_status = Status.Ready;
			if (_listener != null) {
				_listener.UnityVideoPlayerPlaybackReady(this);
			}
		}
		if (_status == Status.Playing && _movieTexture.isPlaying == false) {
			_status = Status.Ready;
			if (_listener != null) {
				_listener.UnityVideoPlayerPlaybackCompleted(this);
			}
		}
#endif
	}

	void OnApplicationQuit() {
		UnityVideoTexturePlugin.UnloadPlugin();
	}

	public bool isLoading {
		get {
			return _status == Status.Loading;
		}
	}
	
	public bool isPlaying {
		get {
			return _status == Status.Playing;
		}
	}
	
	public EventListener listener {
		set {
			_listener = value;
		}
	}

#if UNITY_EDITOR || !UNITY_ANDROID
	public MovieTexture movieTexture {
		get {
			return _movieTexture;
		}
	}
#endif

	public void Load(string uri, bool isStreamingAsset) {
		_status = Status.Loading;
		
		if (_listener != null) {
			_listener.UnityVideoPlayerAboutToLoad(this);
		}
		UnityVideoTexturePlugin.LoadVideo(uri, isStreamingAsset,
                                          gameObject.name,
                                          "UnityVideoTexturePrepared",
                                          "UnityVideoTextureFirstFrameAvailable",
                                          "UnityVideoTextureVideoSizeChanged",
                                          "UnityVideoTextureBufferingStart",
                                          "UnityVideoTextureBufferingEnd",
                                          "UnityVideoTexturePlaybackCompleted",
                                          "UnityVideoTextureErrorOccurred");
#if UNITY_EDITOR || !UNITY_ANDROID
		_www = new WWW(uri);
		_movieTexture = _www.movie;
#endif
	}
	
	public void Play() {
		if (_status == Status.Ready) {
			UnityVideoTexturePlugin.Play();

#if UNITY_EDITOR || !UNITY_ANDROID
			if (_listener != null) {
				_listener.UnityVideoPlayerAboutToPlay(this, _movieTexture.width, _movieTexture.height);
			}
			_movieTexture.Play();
			_status = Status.Playing;
#endif
		}
	}

	public void Pause() {
		if (_status == Status.Playing) {
			UnityVideoTexturePlugin.Pause();
			_status = Status.Ready;

#if UNITY_EDITOR || !UNITY_ANDROID
			_movieTexture.Pause();
#endif
		}
	}

	public void Resume() {
		if (_status == Status.Ready) {
			UnityVideoTexturePlugin.Resume();
			_status = Status.Playing;
			
#if UNITY_EDITOR || !UNITY_ANDROID
			_movieTexture.Play();
#endif
		}
	}
	
	public void Stop() {
		if (_status == Status.Playing) {
			UnityVideoTexturePlugin.Stop();
			_status = Status.Idle;

#if UNITY_EDITOR || !UNITY_ANDROID
			_movieTexture.Stop();
#endif
		}
	}

	public void ResetPlayer() {
		if (_status == Status.Playing || _status == Status.Ready) {
			UnityVideoTexturePlugin.Reset();
			_status = Status.Idle;
		}
	}

	// for UnityVideoTexture
	public void RegisterVideoTexture(UnityVideoTexture videoTexture) {
		_videoTextures.Add(videoTexture);
	}

	public void UnregisterVideoTexture(UnityVideoTexture videoTexture) {
		_videoTextures.Remove(videoTexture);
	}
	
	// handle UnityVideoTexture messages
	private void UnityVideoTexturePrepared(string arg) {
		_status = Status.Ready;

		foreach (UnityVideoTexture videoTexture in _videoTextures) {
			videoTexture.Apply();
		}
		
		if (_listener != null) {
			_listener.UnityVideoPlayerPlaybackReady(this);
		}
	}
	
	private void UnityVideoTextureFirstFrameAvailable(string arg) {
		_status = Status.Playing;
		
		if (_listener != null) {
			_listener.UnityVideoPlayerAboutToPlay(this, _videoWidth, _videoHeight);
		}
	}
	
	private void UnityVideoTextureVideoSizeChanged(string arg) {
		_videoWidth = UnityVideoTexturePlugin.GetWidth();
		_videoHeight = UnityVideoTexturePlugin.GetHeight();
	}
	
	private void UnityVideoTextureBufferingStart(string arg) {
		if (_listener != null) {
			_listener.UnityVideoPlayerBufferingStart(this);
		}
	}
	
	private void UnityVideoTextureBufferingEnd(string arg) {
		if (_listener != null) {
			_listener.UnityVideoPlayerBufferingEnd(this);
		}
	}
	
	private void UnityVideoTexturePlaybackCompleted(string arg) {
		if (_status == Status.Playing) {
			_status = Status.Ready;
		}
		
		if (_listener != null) {
			_listener.UnityVideoPlayerPlaybackCompleted(this);
		}
	}
	
	private void UnityVideoTextureErrorOccurred(string arg) {
		_status = Status.Idle;
		
		if (_listener != null) {
			_listener.UnityVideoPlayerErrorOccurred(this, arg);
		}
	}
}
