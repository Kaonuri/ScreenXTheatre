using UnityEngine;
using System.Collections;

public abstract class VRUIScene : MonoBehaviour {
	public abstract string sceneName { get; }

	protected virtual void Start() {
		VRUISceneManager.manager.NotifySceneLoaded(this);
	}
}
