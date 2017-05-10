using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

[RequireComponent(typeof(Camera))]

public class AirVRCameraFade : MonoBehaviour {
    private static List<AirVRCameraFade> _cameraFades = new List<AirVRCameraFade>();

    public static IEnumerator FadeAllCameras(MonoBehaviour caller, bool fadeIn, float duration) {
        foreach (AirVRCameraFade cameraFade in _cameraFades) {
            caller.StartCoroutine(cameraFade.Fade(fadeIn, duration));
        }

        for (bool anyCameraIsFading = true; anyCameraIsFading;) {
            anyCameraIsFading = false;
            foreach (AirVRCameraFade cameraFade in _cameraFades) {
                anyCameraIsFading = anyCameraIsFading || cameraFade.isFading;
                if (anyCameraIsFading) {
                    break;
                }
            }
            if (anyCameraIsFading) {
                yield return null;
            }
        }
    }

    private Material _fadeMaterial;
    private Color _startFadeColor;
    private Color _endFadeColor;
    private float _startTimeToFade;

    [SerializeField]
    internal Color fadeOutColor = Color.black;

    private void Awake() {
        _fadeMaterial = new Material(Shader.Find("ONAIRVR/Unlit transparent color"));
        _fadeMaterial.color = Color.clear;

        _cameraFades.Add(this);
    }

    private void OnDestroy() {
        _cameraFades.Remove(this);
    }

    private void OnPostRender() {
        if (_fadeMaterial.color != Color.clear) {
            _fadeMaterial.SetPass(0);
            GL.PushMatrix();
            GL.LoadOrtho();
            GL.Color(_fadeMaterial.color);
            GL.Begin(GL.QUADS);
            GL.Vertex3(0.0f, 0.0f, -1.0f);
            GL.Vertex3(0.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 0.0f, -1.0f);
            GL.End();
            GL.PopMatrix();
        }
    }

    public bool isFading { get; private set; }

    public static bool anyCameraIsFading
    {
        get
        {
            foreach (var cameraFade in _cameraFades)
            {
                if (cameraFade.isFading)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public IEnumerator Fade(bool fadeIn, float duration) {
        _startFadeColor = isFading ? _fadeMaterial.color : (fadeIn ? fadeOutColor : Color.clear);
        _endFadeColor = fadeIn ? Color.clear : fadeOutColor;

        _startTimeToFade = Time.realtimeSinceStartup;

        if (isFading == false) {
            isFading = true;
            while (_fadeMaterial.color != _endFadeColor) {
                _fadeMaterial.color = Color.Lerp(_startFadeColor, _endFadeColor, (Time.realtimeSinceStartup - _startTimeToFade) / duration);
                yield return null;
            }
            isFading = false;
        }
    }
}
