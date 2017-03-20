using UnityEngine;
using System.Collections;

public class SplashScene : MonoBehaviour {
	public string mainScene;
	public float duration;

	IEnumerator Start() {
		yield return new WaitForSeconds(duration);

		OVRPlayer.player.MoveTransformToRoot();

		if (string.IsNullOrEmpty(mainScene) == false) {
			yield return StartCoroutine(VRUISceneManager.manager.LoadScene(mainScene, true));
		}
	}
}
