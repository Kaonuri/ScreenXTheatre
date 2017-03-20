using UnityEngine;
using System.Collections;

public class MainScene : VRUIScene {
	private ScreenXMoviePlayer _moviePlayer;

	public Transform cameraPos;

	void Awake() {
		_moviePlayer = gameObject.GetComponentInChildren<ScreenXMoviePlayer>();
	}

	protected override void Start() {
		base.Start();

		ScreenXTheatrePlayer.player.SetPosition(cameraPos);
		VRUIScreenFader.screenFader.FadeIn();
	}

	void Update() {
		if (OVRUIPlatformMenu.menu.quitOnBackClick == _moviePlayer.isPlaying) {
			OVRUIPlatformMenu.menu.quitOnBackClick = !_moviePlayer.isPlaying;
		}
	}

	public override string sceneName {
		get {
			return "Main";
		}
	}
}
