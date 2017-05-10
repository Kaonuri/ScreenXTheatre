using UnityEngine;
using System.Collections;

public class FixedDurationMessage : MonoBehaviour {
	private Timer _timer;

	void Awake() {
		_timer = new Timer();
	}

	void Start() {
		HideImmediately();
	}

	void Update() {
		_timer.Update(Time.deltaTime);
		if (_timer.expired) {
			HideImmediately();
			_timer.Reset();
		}
	}

	public void Show(float duration) {
		if (gameObject.activeSelf == false) {
			gameObject.SetActive(true);
		}
		_timer.Set(duration);
	}

	public IEnumerator ShowCoroutine(float duration) {
		Show(duration);
		yield return new WaitForSeconds(duration);
	}

	public void HideImmediately() {
		if (gameObject.activeSelf) {
			gameObject.SetActive(false);
		}
	}
}
