using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]

public class VRUIScreenFadeCamera : MonoBehaviour {
    private Material _fadeMaterial;

    public Material fadeMaterial {
        set {
            _fadeMaterial = value;
        }
    }

    void Start() {
        VRUIScreenFader.screenFader.RegisterScreenFadeCamera(this);
    }

    void OnDestroy() {
        VRUIScreenFader.screenFader.UnregisterScreenFadeCamera(this);
    }

    void OnPostRender() {
        if (enabled && _fadeMaterial != null) {
            _fadeMaterial.SetPass(0);
            GL.PushMatrix();
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);
            GL.Vertex3(0.0f, 0.0f, -12.0f);
            GL.Vertex3(0.0f, 1.0f, -12.0f);
            GL.Vertex3(1.0f, 1.0f, -12.0f);
            GL.Vertex3(1.0f, 0.0f, -12.0f);
            GL.End();
            GL.PopMatrix();
        }
    }
}
