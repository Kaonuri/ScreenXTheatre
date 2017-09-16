using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class AirVRPointer : MonoBehaviour {
    public static List<AirVRPointer> pointers = new List<AirVRPointer>();

    private Texture2D _cookie;

    [SerializeField] private AirVRCameraRig _cameraRig;
    [SerializeField] private string _cookieTextureFilename;
    [SerializeField] private float _depthScaleMultiplier;

    protected float depthScaleMultiplier {
        get {
            return _depthScaleMultiplier;
        }
    }

    private void Awake() {
        pointers.Add(this);
    }

    private IEnumerator Start() {
        if (string.IsNullOrEmpty(_cookieTextureFilename) == false) {
            WWW www = new WWW("file://" + System.IO.Path.Combine(Application.streamingAssetsPath, _cookieTextureFilename));
            yield return www;

            if (string.IsNullOrEmpty(www.error)) {
                _cookie = www.texture;
            }
        }
    }

    protected virtual void Update() {
        if (AirVRInput.IsDeviceAvailable(_cameraRig, device) && AirVRInput.IsDeviceFeedbackEnabled(_cameraRig, device) == false && cookie != null) {
#if UNITY_2017_1_OR_NEWER
            AirVRInput.EnablePointerDeviceFeedback(_cameraRig, device, ImageConversion.EncodeToPNG(cookie), depthScaleMultiplier);
#else
            AirVRInput.EnablePointerDeviceFeedback(_cameraRig, device, cookie.EncodeToPNG(), depthScaleMultiplier);
#endif
        }
    }

    private void OnDisable() {
        if (AirVRInput.IsDeviceFeedbackEnabled(_cameraRig, device)) {
            AirVRInput.DisableDeviceFeedback(_cameraRig, device);
        }
    }

    private void OnDestroy() {
        pointers.Remove(this);
    }

    protected Texture2D cookie {
        get {
            return _cookie;
        }
    }

    protected abstract AirVRInput.Device device { get; }

    public AirVRCameraRig cameraRig {
        get {
            return _cameraRig;
        }
    }

    public bool interactable {
        get {
            return AirVRInput.IsDeviceFeedbackEnabled(_cameraRig, device);
        }
    }

    public abstract bool primaryButtonPressed { get; }
    public abstract bool primaryButtonReleased { get; }

    public Ray GetWorldRay() {
        switch (device) {
            case AirVRInput.Device.HeadTracker:
                return new Ray(_cameraRig.headPose.position, _cameraRig.headPose.forward);
            case AirVRInput.Device.TrackedController:
                Vector3 position = Vector3.zero;
                Quaternion orientation = Quaternion.identity;
                AirVRInput.GetPointerPositionAndOrientation(_cameraRig, AirVRInput.Device.TrackedController, out position, out orientation);
                return new Ray(position, orientation * Vector3.forward);
        }
        return new Ray();
    }

    public void UpdateRaycastResult(Ray ray, RaycastResult raycastResult) {
        if (raycastResult.isValid) {
            AirVRInput.FeedbackPointerDevice(_cameraRig, device, ray.origin, raycastResult.worldPosition, raycastResult.worldNormal);
        }
        else {
            AirVRInput.FeedbackPointerDevice(_cameraRig, device, Vector3.zero, Vector3.zero, Vector3.zero);
        }
    }
}
