using Unity.Collections;
using UnityEngine;

public abstract class State<T> : MonoBehaviour {
	[ReadOnly] public bool stateChanged;
	[ReadOnly] public T prevState;
	[ReadOnly] public T currentState;

	public T CurrentState {
		get => currentState;
		set {
			if (Equals(value, CurrentState)) {
				stateChanged = false;
				return;
			}
			stateChanged = true;
			prevState = CurrentState;
			currentState = value;
			OnVariableChanged?.Invoke(CurrentState);
		}
	}

	protected abstract bool Equals(T value, T state);

	// private static bool Equals(T value, T state) {
	// 	switch (Type.GetTypeCode(typeof(T))) {
	// 		case TypeCode.Boolean: return (bool) (object) value == (bool) (object) state;
	// 		case TypeCode.Int32: return (int) (object) value == (int) (object) state;
	// 		case TypeCode.Single: return Math.Abs((float) (object) value - (float) (object) state) < TOLERANCE;
	// 		default: throw new ArgumentOutOfRangeException();
	// 	}
	// }

	public delegate void OnVariableChangeDelegate(T newState);

	public event OnVariableChangeDelegate OnVariableChanged;
}