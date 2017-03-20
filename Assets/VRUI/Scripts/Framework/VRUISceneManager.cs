using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VRUISceneManager {
	private static VRUISceneManager _instance;

	public static VRUISceneManager manager {
		get {
			if (_instance == null) {
				_instance = new VRUISceneManager();
			}
			return _instance;
		}
	}

	public VRUISceneManager() {
		_listeners = new List<SceneEventListener>();
	}

	public interface SceneEventListener {
		void VRUISceneLoaded(VRUISceneManager manager, VRUIScene sceneLoaded);
	}

	private VRUIScene _currentScene;
	private List<SceneEventListener> _listeners;

	public VRUIScene currentScene {
		get {
			return _currentScene;
		}
	}

	public void AddListener(SceneEventListener listener) {
		_listeners.Add(listener);
	}

	public void RemoveListener(SceneEventListener listener) {
		_listeners.Remove(listener);
	}

	public IEnumerator LoadScene(string sceneName, bool fadeScreen) {
		AsyncOperation async = Application.LoadLevelAsync(sceneName);
		async.allowSceneActivation = false;
		
		while (async.progress < 0.9f || VRUIScreenFader.screenFader.isFading) {
			yield return 0;
		}

		if (fadeScreen) {
			VRUIScreenFader.screenFader.FadeOut();
			yield return new WaitForSeconds(VRUIScreenFader.screenFader.fadeDuration + 0.1f);
		}
		
		System.GC.Collect();
		async.allowSceneActivation = true;
	}

	// for VRUIScene
	public void NotifySceneLoaded(VRUIScene scene) {
		_currentScene = scene;

		foreach (SceneEventListener listener in _listeners) {
			listener.VRUISceneLoaded(this, _currentScene);
		}
	}
}
