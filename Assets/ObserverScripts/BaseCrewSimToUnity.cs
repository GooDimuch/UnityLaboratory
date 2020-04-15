using Unity.Collections;
using UnityEngine;

public abstract class BaseCrewSimToUnity<T> : MonoBehaviour {
	[ReadOnly] public T debug;

	protected new void Start() { }

	protected void Update() {
		debug = (T) (GetValue());
		PreAnimate();
		Animate(debug);
	}

	protected abstract T GetValue();
	protected virtual void PreAnimate() { }

	public abstract void Animate(T value);
}