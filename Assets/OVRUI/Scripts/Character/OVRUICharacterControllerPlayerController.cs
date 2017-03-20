using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class OVRUICharacterControllerPlayerController : OVRUIPlayerController {
	private CharacterController _character;
	private float _fallingSpeed;

	public float gravity = 10.0f;

	private void UpdateGravity(float deltaTime) {
		if (_character.isGrounded) {
			_fallingSpeed = 0.0f;
		}
		else {
			_fallingSpeed += gravity * deltaTime;
		}
	}

	private void ResetGravity() {
		_fallingSpeed = 0.0f;
	}

	void Awake() {
		_character = gameObject.GetComponent<CharacterController>();
	}

	protected override void OnStart() {}

	protected override void OnEnabled(bool enable) {
		_character.enabled = enable;
	}

	protected override void Move(Vector3 velocity, float deltaTime) {
		Vector3 horizontalDir = velocity;
		horizontalDir.y = 0.0f;
		horizontalDir.Normalize();

		Vector3 movingDir = velocity.magnitude * horizontalDir * deltaTime;
		if (_fallingSpeed > 0.0f) {
			_character.Move(movingDir + 
			                Mathf.Max(_fallingSpeed * deltaTime, movingDir.magnitude / Mathf.Tan(_character.slopeLimit)) * Vector3.down);
		}
		else {
			_character.Move(movingDir);
		}

		UpdateGravity(deltaTime);
	}

	protected override void Stop() {
		ResetGravity();
	}
}
