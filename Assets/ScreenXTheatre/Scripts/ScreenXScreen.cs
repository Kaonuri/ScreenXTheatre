using UnityEngine;
using System.Collections;

public class ScreenXScreen : MonoBehaviour {
	public enum State {
		Off,
		Single,
		ScreenX
	}

	private void setTexture(MeshRenderer renderer, Texture texture) {
		foreach (Material mat in renderer.sharedMaterials) {
			mat.mainTexture = texture;
		}
	}

	private void handleStateChange(State from, State to) {
		if (to == State.Off) {
			singleScreen.enabled = false;

			setTexture(singleScreen, null);
			setTexture(screenX, turnedOffDisplay);
		}
		else {
			singleScreen.enabled = to == State.Single;

			setTexture(singleScreen, (to == State.Single) ? _content : null);
			setTexture(screenX, (to == State.ScreenX) ? _content : turnedOffDisplay);
		}
	}

	private State _currentState;
	private Texture _content;

	public MeshRenderer singleScreen;
	public MeshRenderer screenX;
	public Texture2D turnedOffDisplay;

	void Start() {
		_currentState = State.Off;
		handleStateChange(State.Off, State.Off);
	}

	public Texture content {
		set {
			_content = value;
		}
	}

	public void SetState(State state) {
		if (_currentState != state) {
			handleStateChange(_currentState, state);
			_currentState = state;
		}
	}
}
