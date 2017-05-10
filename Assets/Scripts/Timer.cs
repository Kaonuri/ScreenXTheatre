public class Timer {
	public Timer() {
		_timer = -1.0f;
	}

	private float _timer;
	private float _timeout;
	private bool _expired;

	public bool expired {
		get {
			return _expired;
		}
	}

	public void Set(float timeout) {
		_timeout = timeout;
		_timer = 0.0f;
	}

	public void Reset() {
		_timer = -1.0f;
		_expired = false;
	}

	public void Update(float deltaTime) {
		if (0.0f <= _timer && _timer < _timeout) {
			_timer += deltaTime;
		}
		else if (_timer >= _timeout) {
			_expired = true;
			_timer = -1.0f;
		}
	}
}
