using System;
using UnityEngine;

[HelpURL("https://git.logika.tech/sim/landvehicles/-/wikis/Controls/BaseControl")]
// [RequireComponent(typeof(State<T>))]
[RequireComponent(typeof(BoxCollider))]
public abstract class BaseControlController<T> : MonoBehaviour {
	public enum Axis {
		X,
		Y,
		Z
	}

#region constants
#endregion

#region inspector
	public string AudioEventName;
#endregion

#region internal variable
	private State<T> state;
	// private SoundEventsSystem soundEventsSystem;

	protected T CurrentState => state.CurrentState;
#endregion

	public void Start() {
		// soundEventsSystem = GameObject.Find("SoundEventsSystem").GetComponent<SoundEventsSystem>();
		switch (Type.GetTypeCode(typeof(T))) {
			case TypeCode.Boolean:
				gameObject.AddComponent<BoolState>();
				break;
			case TypeCode.Int32:
				gameObject.AddComponent<IntState>();
				break;
			case TypeCode.Single:
				gameObject.AddComponent<FloatState>();
				break;
			default: throw new ArgumentOutOfRangeException();
		}
		state = gameObject.GetComponent<State<T>>();
		state.OnVariableChanged += OnVariableChanged;

		Animate(CurrentState);
	}

	protected void ChangeState(T shift) => state.CurrentState = GetCurrentState(CurrentState, shift);

	protected abstract T GetCurrentState(T currentState, T shift);

	private void OnVariableChanged(T newState) {
		// if (soundEventsSystem != null) { soundEventsSystem.triggerEvent(AudioEventName); }

		Animate(newState);
	}

	public abstract void Animate(T newState);
}