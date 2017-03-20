using UnityEngine;
using System.Collections;

public class ExampleScene : MonoBehaviour {
	public Transform playerStart;

	void Start() {
		OVRPlayer.player.SetPosition(playerStart, false);
		OVRPlayer.player.EnablePlayerMovement(true);

		VRUIScreenFader.screenFader.FadeIn();
		VRUIManager.manager.interactionEnabled = true;
	}
}
