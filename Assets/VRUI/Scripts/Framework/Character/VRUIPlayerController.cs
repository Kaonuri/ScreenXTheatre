using UnityEngine;
using System.Collections;

public abstract class VRUIPlayerController : MonoBehaviour {
	public Transform forwardDirection;
	public float speed;

	protected abstract float horizontal { get; }
	protected abstract float vertical { get; }
	protected abstract void OnEnabled(bool enable);
	protected abstract void Move(Vector3 velocity, float deltaTime);
	protected abstract void Stop();
	
	private Vector3 inputDirection() {
		Vector3 result = horizontal * Vector3.right + vertical * Vector3.forward;
		return (result.sqrMagnitude > 1.0f) ? result.normalized : result;
	}
	
	void Update() {
		if (VRUIManager.manager.interactionEnabled) {
			Move(forwardDirection.TransformDirection(inputDirection()) * speed, Time.deltaTime);
		}
		else {
			Stop();
		}
	}
	
	public void Enable(bool enable) {
		enabled = enable;
		
		if (enable == false) {
			Stop();
		}
		OnEnabled(enable);
	}
}
