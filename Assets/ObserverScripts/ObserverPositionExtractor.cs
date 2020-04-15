using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ObserverPositionExtractor : MonoBehaviour {
	[System.Serializable] public class OrientationEvent : UnityEvent<float[]> { }

	private readonly float[] position = {0f, 0f, 0f};
	public Transform target;
	public Transform pvo;

	public OrientationEvent orientationEvent;

	[ReadOnly] public string DEBUG = "No data";

	new void Start() { }

	void Update() {
		position[0] = target.position.x;
		position[1] = target.position.y;
		position[2] = target.position.z;
		orientationEvent?.Invoke(position);
#if (UNITY_EDITOR)
		DEBUG = "[" + string.Join(",", position) + "]";
#endif
	}

	public float getX() { return position[0]; }

	public float getY() { return position[1]; }

	public float getZ() { return position[2]; }
}