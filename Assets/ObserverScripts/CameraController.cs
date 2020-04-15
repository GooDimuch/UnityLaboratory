using System;
using Unity.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public enum Mode {
		Off,
		_1,
		ToTarget,
	}

	public Mode mode;

	public GameObject MTLB;
	[Range(-5, 80)] public float UM;
	[Range(-360, 360)] public float Azimuth = 0;

	public TargetMover Target;
	[ReadOnly] public Vector3 dirToTarget;

	void Start() { }

	void Update() {
		switch (mode) {
			case Mode.Off: break;
			case Mode._1:
				// calcRotation();
				break;
			case Mode.ToTarget:
				dirToTarget = Target.dirRelativeRocket;
				SetDirectionToTarget();
				break;
			default: throw new ArgumentOutOfRangeException();
		}
	}

	private void SetDirectionToTarget() {
		float mtlbPitch = MTLB.transform.eulerAngles.x;
		float mtlbYaw = MTLB.transform.eulerAngles.y;
		float mtlbRoll = MTLB.transform.eulerAngles.z;

		Vector3 pvoOrientation = new Vector3(mtlbPitch, mtlbYaw, mtlbRoll);
		Vector2 puOrientation = new Vector2(-dirToTarget.z, dirToTarget.y);
		transform.eulerAngles = GetLocalPvoOrientation(pvoOrientation, puOrientation);
	}

	public void calcRotation() {
		float mtlbPitch = MTLB.transform.eulerAngles.x;
		float mtlbYaw = MTLB.transform.eulerAngles.y;
		float mtlbRoll = MTLB.transform.eulerAngles.z;

		Vector3 pvoOrientation = new Vector3(mtlbPitch, mtlbYaw, mtlbRoll);
		Vector2 puOrientation = new Vector2(-UM, Azimuth);
		// transform.position = MTLB.transform.position;
		transform.eulerAngles = GetLocalPvoOrientation(pvoOrientation, puOrientation);
	}

	private Vector3 GetLocalPvoOrientation(Vector3 pvoOrientation, Vector2 puOrientation) {
		Quaternion mtlbQuat = Quaternion.Euler(pvoOrientation.x, pvoOrientation.y, pvoOrientation.z);
		Quaternion PUQuat = Quaternion.Euler(puOrientation.x, puOrientation.y, 0);
		return (mtlbQuat * PUQuat).eulerAngles;
	}
}