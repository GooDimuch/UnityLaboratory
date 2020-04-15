using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ObserverOrientationExtractor : MonoBehaviour {
	[System.Serializable] public class OrientationEvent : UnityEvent<float[]> { }

	private readonly float[] orientation = {0f, 0f, 0f};
	public Transform target;

	public OrientationEvent orientationEvent;

	[ReadOnly] public string DEBUG = "No data";

	new void Start() {   }

	void Update() {
		orientation[0] = target.localEulerAngles.x;
		orientation[1] = target.localEulerAngles.y;
		orientation[2] = target.localEulerAngles.z;
		orientationEvent?.Invoke(orientation);
#if (UNITY_EDITOR)
		DEBUG = "[" + string.Join(",", orientation) + "]";
#endif
	}

	public float getPitch() { return orientation[0]; }

	public float getYaw() { return orientation[1]; }

	public float getRoll() { return orientation[2]; }
}