using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ScreenXStateMachine))]
[RequireComponent(typeof(UnityVideoTexture))]

public class ScreenXMoviePlayer : MonoBehaviour, UnityVideoPlayer.EventListener, VRUIEventHandler {
	private const string AssetsRoot = "/storage/sdcard0/Clicked";
	private const float HelpTextDuration = 2.5f;

	private int _iterPlaylist;
	private ScreenXMoviePlaylist.Item _currentPlaylistItem;
	private ScreenXStateMachine _statemachine;
	private VRUIFixedDurationMessage _helpText;
	private UnityVideoTexture _videoTexture;

	public ScreenXMoviePlaylist playlist;
	public ScreenXMoviePlaylist playlistOnEditor;
	public ScreenXScreen screen;
	public ScreenXLightRenderer lightRenderer;

	private void loadMovie(ScreenXMoviePlaylist.Item item) {
#if UNITY_ANDROID && !UNITY_EDITOR
		UnityVideoPlayer.player.Load(item.isStreamingAsset ? item.uri : (AssetsRoot + "/" + item.uri), item.isStreamingAsset);
#else
		UnityVideoPlayer.player.Load(item.uri, item.isStreamingAsset);
#endif
	}

	private ScreenXMoviePlaylist currentPlaylist() {
#if UNITY_ANDROID && !UNITY_EDITOR
		return playlist;
#else
		return playlistOnEditor;
#endif
	}

	void Awake() {
		_statemachine = gameObject.GetComponent<ScreenXStateMachine>();
		_helpText = gameObject.GetComponentInChildren<VRUIFixedDurationMessage>();
		_videoTexture = gameObject.GetComponent<UnityVideoTexture>();
	}

	void Start() {
		UnityVideoPlayer.player.listener = this;
		_statemachine.StateChanged += ScreenXStateMachineStateChanged;

		VRUIManager.manager.RegisterHandler(this);
		VRUIManager.manager.RegisterInputDevice(new OVRUITouchpadDevice());
		VRUIManager.manager.interactionEnabled = true;
	}

	void OnApplicationPause(bool pauseStatus) {
		if (pauseStatus) {
			_statemachine.AppPaused();
		}
		else {
			_statemachine.AppResumed();
		}
	}

	void OnDestroy() {
		if (UnityVideoPlayer.player != null) {
			UnityVideoPlayer.player.listener = null;
		}
		if (VRUIManager.manager != null) {
			VRUIManager.manager.UnregisterHandler(this);
		}
	}

	public bool isPlaying {
		get {
			return _statemachine.currentState != ScreenXStateMachine.State.Ready;
		}
	}

	// handle ScreenXStateMachine events
	private void ScreenXStateMachineStateChanged(ScreenXStateMachine statemachine, ScreenXStateMachine.State from, ScreenXStateMachine.State to) {
		if (from == ScreenXStateMachine.State.Ready && to == ScreenXStateMachine.State.TurningLightDown) {
			lightRenderer.TurnDownLights();
		}
		else if (from == ScreenXStateMachine.State.TurningLightDown && to == ScreenXStateMachine.State.Loading) {
			_currentPlaylistItem = currentPlaylist().GetFirstItem(ref _iterPlaylist);
			loadMovie(_currentPlaylistItem);
		}
		else if (from == ScreenXStateMachine.State.Playing && to == ScreenXStateMachine.State.Loading) {
			_currentPlaylistItem = currentPlaylist().GetNextItem(ref _iterPlaylist);
			loadMovie(_currentPlaylistItem);
		}
		else if (from == ScreenXStateMachine.State.Loading && to == ScreenXStateMachine.State.Playing) {
			UnityVideoPlayer.player.Play();
		}
		else if (from == ScreenXStateMachine.State.Playing && to == ScreenXStateMachine.State.Paused) {
			UnityVideoPlayer.player.Pause();
		}
		else if (from == ScreenXStateMachine.State.Paused && to == ScreenXStateMachine.State.Playing) {
			UnityVideoPlayer.player.Resume();
		}
		else if (from == ScreenXStateMachine.State.Playing && to == ScreenXStateMachine.State.Stopping) {
			UnityVideoPlayer.player.Stop();
			_statemachine.MovieStopped();
		}
		else if ((from == ScreenXStateMachine.State.Playing || from == ScreenXStateMachine.State.Stopping) &&
		         to == ScreenXStateMachine.State.Ready) {
			screen.SetState(ScreenXScreen.State.Off);
			UnityVideoPlayer.player.ResetPlayer();

			lightRenderer.TurnUpLights();
			lightRenderer.EnableScreenReflection(false);
			_helpText.HideImmediately();
		}
	}

	// implements UnityVideoPlayer.EventListener
	public void UnityVideoPlayerAboutToLoad(UnityVideoPlayer player) {}

	public void UnityVideoPlayerPlaybackReady(UnityVideoPlayer player) {
		_statemachine.MovieLoaded();
	}

	public void UnityVideoPlayerAboutToPlay(UnityVideoPlayer player, int videoWidth, int videoHeight) {
		screen.content = _videoTexture.texture;
		screen.SetState(_currentPlaylistItem.isScreenX ? ScreenXScreen.State.ScreenX : ScreenXScreen.State.Single);
		lightRenderer.EnableScreenReflection(true);
	}

	public void UnityVideoPlayerBufferingStart(UnityVideoPlayer player) {}

	public void UnityVideoPlayerBufferingEnd(UnityVideoPlayer player) {}

	public void UnityVideoPlayerPlaybackCompleted(UnityVideoPlayer player) {
		_currentPlaylistItem = currentPlaylist().GetNextItem(ref _iterPlaylist);
		_statemachine.MoviePlaybackCompleted(_currentPlaylistItem != null);
	}

	public void UnityVideoPlayerErrorOccurred(UnityVideoPlayer player, string message) {}

	// implements VRUIEventHandler
	public bool VRUIEventOccurred(VRUIManager manager, VRUIElement pointingElement, int deviceID, int elementID, int action) {
		if (deviceID == OVRUITouchpadDevice.DeviceID) {
			if (elementID == OVRUITouchpadDevice.ButtonTap && action == VRUIButtonInputElement.ActionSingle) {
				if (_statemachine.currentState == ScreenXStateMachine.State.Ready) {
					_statemachine.StartMovie(1.5f);
				}
				else if (_statemachine.currentState == ScreenXStateMachine.State.Playing) {
					_helpText.Show(HelpTextDuration);
				}
				return true;
			}
			else if (elementID == OVRUITouchpadDevice.ButtonHome && action == VRUIButtonInputElement.ActionSingle) {
				if (_statemachine.currentState == ScreenXStateMachine.State.Playing) {
					_statemachine.StopMovie();
				}
			}
		}
		return false;
	}
}
