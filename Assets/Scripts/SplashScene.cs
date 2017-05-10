using UnityEngine;
using System.Collections;

public class SplashScene : MonoBehaviour
{
    [SerializeField] private AirVRCameraRig cameraRig;
    public float duration;

    IEnumerator Start()
    {
        while (!cameraRig.isBoundToClient)
        {
            yield return null;
        }

        yield return new WaitForSeconds(duration);
        yield return AirVRCameraFade.FadeAllCameras(this, false, 1f);        
        yield return StartCoroutine(SceneManager.Instance.LoadScene("Main"));
    }
}
