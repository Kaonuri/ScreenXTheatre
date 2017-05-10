using UnityEngine;
using System.Collections;

public class ScreenXStateMachine : MonoBehaviour
{
	public enum State
    {
		Ready,
		TurningLightDown,
		Loading,
		Playing,
		Paused,
		Stopping
	}

	private State _currentState;
	private Timer _timer;
	private float _timeout;

	public delegate void StateChangeHandler(ScreenXStateMachine statemachine, State from, State to);
	public event StateChangeHandler StateChanged;

	private void ChangeCurrentStateTo(State to)
    {
		if (_currentState != to)
        {
			State from = _currentState;
			_currentState = to;

			if (StateChanged != null)
				StateChanged(this, from, to);
		}
	}

	private void HandleTimeout()
    {
		if (_currentState == State.TurningLightDown)
			ChangeCurrentStateTo(State.Loading);
	}

	private void Awake()
    {
		_currentState = State.Ready;
		_timer = new Timer();
	}

    private void Update()
    {
		_timer.Update(Time.deltaTime);
		if (_timer.expired)
        {
			HandleTimeout();
			_timer.Reset();
		}
	}

	public State CurrentState
    {
		get
        {
			return _currentState;
		}
	}

	public void StartMovie(float delayToLoad)
    {
		if (_currentState == State.Ready)
        {
			_timer.Set(delayToLoad);
			ChangeCurrentStateTo(State.TurningLightDown);
		}
	}

	public void StopMovie()
    {
		if (_currentState == State.Playing)
			ChangeCurrentStateTo(State.Stopping);
	}

	public void MovieLoaded() {
		if (_currentState == State.Loading)
			ChangeCurrentStateTo(State.Playing);
	}

	public void MovieStopped()
    {
		if (_currentState == State.Stopping)
			ChangeCurrentStateTo(State.Ready);
	}

	public void MoviePlaybackCompleted(bool hasNextMovie)
    {
		if (_currentState == State.Playing)
        {
			if (hasNextMovie)
				ChangeCurrentStateTo(State.Loading);

			else
				ChangeCurrentStateTo(State.Ready);
		}
	}

	public void AppPaused()
    {
		if (_currentState == State.Playing)
        {
			ChangeCurrentStateTo(State.Paused);
		}
	}

	public void AppResumed()
    {
		if (_currentState == State.Paused)
        {
			ChangeCurrentStateTo(State.Playing);
		}
	}
}
