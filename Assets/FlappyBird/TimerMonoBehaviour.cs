using System;
using System.Collections.Generic;
using UnityEngine;

public class TimerMonoBehaviour : MonoBehaviour {
	private readonly List<Timer> _timers = new List<Timer>();

	public void Update() {
		CheckTimers();
		for (var i = 0; i < _timers.Count; i++) {
			CheckCall(_timers[i]);
		}
	}

	private void CheckTimers() {
		for (var i = 0; i < _timers.Count; i++) {
			if (!_timers[i].Status) { continue; }
			_timers.RemoveAt(i);
			i--;
		}
	}

	public void ActionWithDelay(float delay, Action action) {
		var timer = new Timer(delay, action);
		_timers.Add(timer);
		CheckCall(timer);
	}

	private void CheckCall(Timer timer) {
		if (timer.Delay < 0) {
			timer.Action.Invoke();
			timer.Status = true;
		}
		timer.Delay -= Time.deltaTime * 1000;
	}

	public class Timer {
		public float Delay { get; set; }
		public Action Action { get; }
		public bool Status { get; set; }

		public Timer(float delay, Action action) {
			Delay = delay;
			Action = action;
		}
	}
}