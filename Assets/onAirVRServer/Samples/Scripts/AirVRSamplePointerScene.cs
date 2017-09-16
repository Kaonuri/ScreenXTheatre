using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AirVRSamplePointerScene : MonoBehaviour {
    private const string BasicSampleSceneName = "A. Basic";

    private bool _loadingBasicScene;

    private IEnumerator loadScene(string sceneName) {
        yield return StartCoroutine(AirVRCameraFade.FadeAllCameras(this, false, 0.5f));
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    IEnumerator Start() {
        yield return StartCoroutine(AirVRCameraFade.FadeAllCameras(this, true, 0.5f));
    }

    public void GoToBasicScene() {
        if (_loadingBasicScene == false) {
            _loadingBasicScene = true;

            StartCoroutine(loadScene(BasicSampleSceneName));
        }
    }
}
