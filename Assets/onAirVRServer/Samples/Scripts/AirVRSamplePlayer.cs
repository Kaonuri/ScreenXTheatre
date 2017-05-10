using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class AirVRSamplePlayer : MonoBehaviour {
    private const float ThrowSpeed = 12.0f;
    private const float ThrowTorqueSpeed = 20.0f;

    private Transform _thisTransform;
    private CharacterController _thisCharacterController;
    private AirVRStereoCameraRig _cameraRig;
    private Transform _headLocator;
    private float _fallingSpeed;
    private AirVRSampleAudioSource _soundShot;

    public AirVRSampleCan canPrefab;
    public float gravity;
    public float speed;

    private void resetFalling() {
        _fallingSpeed = 0.0f;
    }

    private void updateFalling(float deltaTime) {
		if (_thisCharacterController.isGrounded) {
			_fallingSpeed = 0.0f;
		}
		else {
			_fallingSpeed += gravity * deltaTime;
		}
	}

    private float horizontal() {
        return Mathf.Clamp(AirVRInput.GetAxis(_cameraRig, AirVRInput.Touchpad.Axis.DragX) + AirVRInput.GetAxis(_cameraRig, AirVRInput.Gamepad.Axis.LThumbstickX), -1.0f, 1.0f);
    }

    private float vertical() {
        return Mathf.Clamp(AirVRInput.GetAxis(_cameraRig, AirVRInput.Touchpad.Axis.DragY) + AirVRInput.GetAxis(_cameraRig, AirVRInput.Gamepad.Axis.LThumbstickY), -1.0f, 1.0f);
    }

    private void processMovement() {
        if (_thisCharacterController.enabled) {
            Vector3 moveDirection = Vector3.forward * vertical() + Vector3.right * horizontal();
            if (moveDirection.sqrMagnitude > 1.0f) {
                moveDirection = moveDirection.normalized;
            }

            Vector3 velocity = speed * (_cameraRig as AirVRStereoCameraRig).centerEyeAnchor.TransformDirection(moveDirection);
            Vector3 horizontalDir = new Vector3(velocity.x, 0.0f, velocity.z).normalized;

            Vector3 movingDir = velocity.magnitude * horizontalDir * Time.deltaTime;
            if (_fallingSpeed > 0.0f) {
                _thisCharacterController.Move(movingDir + Mathf.Max(_fallingSpeed * Time.deltaTime, movingDir.magnitude / Mathf.Tan(_thisCharacterController.slopeLimit)) * Vector3.down);
            }
            else {
                _thisCharacterController.Move(movingDir);
            }
            updateFalling(Time.deltaTime);
        }
    }

    private void processInput() {
        if (AirVRInput.GetButtonDown(_cameraRig, AirVRInput.Touchpad.Button.Touch) || AirVRInput.GetButtonDown(_cameraRig, AirVRInput.Gamepad.Button.A)) {
            throwCan();
        }
    }

    public void throwCan() {
        Vector3 forward = _cameraRig.centerEyeAnchor.forward;

        AirVRSampleCan can = Instantiate(canPrefab, transform.position, _cameraRig.centerEyeAnchor.rotation) as AirVRSampleCan;
        can.Throw(forward * ThrowSpeed, Vector3.right * ThrowTorqueSpeed);

        _soundShot.Play();
    }

    void Awake() {
        _thisTransform = transform;
        _thisCharacterController = GetComponent<CharacterController>();
        _cameraRig = GetComponentInChildren<AirVRStereoCameraRig>();
        _headLocator = transform.FindChild("Body/HeadLocator");
        _soundShot = transform.FindChild("SoundShot").GetComponent<AirVRSampleAudioSource>();
    }

    void Update() {
        _headLocator.rotation = _cameraRig.centerEyeAnchor.rotation;
        
        processMovement();
        processInput();
    }

    public void SetPosition(Transform pos) {
        _thisTransform.position = pos.position;
        _thisTransform.rotation = pos.rotation;
    }

    public void EnableInteraction(bool enable) {
        _thisCharacterController.enabled = enable;
        if (_cameraRig.pointer != null) {
            _cameraRig.pointer.isEnabled = enable;
        }
        if (enable == false) {
            resetFalling();
        }
    }
}
