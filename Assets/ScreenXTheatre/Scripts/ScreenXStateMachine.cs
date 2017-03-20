using UnityEngine;
using System.Collections;

public class ScreenXStateMachine : MonoBehaviour {
	public enum State {
		Ready,
		TurningLightDown,
		Loading,
		Playing,
		Paused,
		Stopping
	}

	private State _currentState;
	private VRUITimer _timer;
	private float _timeout;

	public delegate void StateChangeHandler(ScreenXStateMachine statemachine, State from, State to);
	public event StateChangeHandler StateChanged;

	private void changeCurrentStateTo(State to) {
		if (_currentState != to) {
			State from = _currentState;
			_currentState = to;

			if (StateChanged != null) {
				StateChanged(this, from, to);
			}
		}
	}

	private void handleTimeout() {
		if (_currentState == State.TurningLightDown) {
			changeCurrentStateTo(State.Loading);
		}
	}

	void Awake() {
		_currentState = State.Ready;
		_timer = new VRUITimer();
	}

	void Update() {
		_timer.Update(Time.deltaTime);
		if (_timer.expired) {
			handleTimeout();
			_timer.Reset();
		}
	}

	public State currentState {
		get {
			return _currentState;
		}
	}

	public void StartMovie(float delayToLoad) {
		if (_currentState == State.Ready) {
			_timer.Set(delayToLoad);
			changeCurrentStateTo(State.TurningLightDown);
		}
	}

	public void StopMovie() {
		if (_currentState == State.Playing) {
			changeCurrentStateTo(State.Stopping);
		}
	}

	public void MovieLoaded() {
		if (_currentState == State.Loading) {
			changeCurrentStateTo(State.Playing);
		}
	}

	public void MovieStopped() {
		if (_currentState == State.Stopping) {
			changeCurrentStateTo(State.Ready);
		}
	}

	public void MoviePlaybackCompleted(bool hasNextMovie) {
		if (_currentState == State.Playing) {
			if (hasNextMovie) {
				changeCurrentStateTo(State.Loading);
			}
			else {
				changeCurrentStateTo(State.Ready);
			}
		}
	}

	public void AppPaused() {
		if (_currentState == State.Playing) {
			changeCurrentStateTo(State.Paused);
		}
	}

	public void AppResumed() {
		if (_currentState == State.Paused) {
			changeCurrentStateTo(State.Playing);
		}
	}
}
