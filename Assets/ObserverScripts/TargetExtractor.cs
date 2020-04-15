using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TargetExtractor : MonoBehaviour {
	[Serializable] public class PositionEvent : UnityEvent<float[]> { }

	[Serializable] public class VelocityEvent : UnityEvent<float[]> { }

	private readonly float[] pos = {0f, 0f, 0f};
	private readonly float[] vel = {0f, 0f, 0f};
	private Vector3 targetPosition;
	private Vector3 targetVelocity;
	public GameObject targetsGO;
	public Transform pvo;

	public PositionEvent positionEvent;
	public VelocityEvent velocityEvent;

	[ReadOnly] public string DEBUG = "No data";

	[ReadOnly, SerializeField] private bool inBeam;
	private MeshRenderer BeamMesh;
	private PuRotator puAzimut;
	private PuRotator puElevation;
	private BeamSize widthBeam;
	private BeamSize heightBeam;
	private List<Transform> targets;
	[ReadOnly] public List<Vector3> targetDecart1;
	[ReadOnly] public List<Vector3> targetPositions;
	[ReadOnly] public List<Vector3> targetDecart;
	[ReadOnly] public List<Vector3> targetVelocities;
	private Vector3 tempV3;
	private Vector2 beamOrientation;
	private Vector2 beamSize;

	private new void Start() {
		BeamMesh = GetComponent<MeshRenderer>();
		targetPositions = new List<Vector3>();
		targetDecart = new List<Vector3>();
		targetDecart1 = new List<Vector3>();
		targetVelocities = new List<Vector3>();
		puAzimut = transform.parent.GetComponents<PuRotator>()
											.FirstOrDefault(script => script.rotateType == PuRotator.RotateType.Azimut);
		puElevation = transform.parent.GetComponents<PuRotator>()
													.FirstOrDefault(script => script.rotateType == PuRotator.RotateType.Elevation);
		widthBeam = transform.GetComponents<BeamSize>().FirstOrDefault(script => script.type == BeamSize.Type.Width);
		heightBeam = transform.GetComponents<BeamSize>().FirstOrDefault(script => script.type == BeamSize.Type.Height);
		targets = targetsGO.GetComponentsInChildren<Transform>()
											.Where((transform => transform.TryGetComponent(typeof(TargetMover), out _)))
											.ToList();
	}

	private void Update() {
		if (!BeamMesh.enabled) { SetDefaultPosition(); } else {
			UpdateTargets();
			UpdateBeam();
			inBeam = CheckTargetInBeam(out targetPosition, out targetVelocity);
			if (!inBeam) SetDefaultPosition();
		}

		invokeEvent(positionEvent, pos, targetPosition);
		invokeEvent(velocityEvent, vel, targetVelocity);
#if (UNITY_EDITOR)
		DEBUG = $"[{string.Join(",", targetPosition)}] [{string.Join(",", targetVelocity)}]";
#endif
	}

	private void invokeEvent(UnityEvent<float[]> unityEvent, float[] floats, Vector3 vector3) {
		floats[0] = vector3.x;
		floats[1] = vector3.y;
		floats[2] = vector3.z;
		unityEvent?.Invoke(floats);
	}

	private void UpdateTargets() {
		for (var i = 0; i < targets.Count; i++)
			if (targetPositions.Count > i) {
				targetPositions[i] = ConvertDecardToPolar(targets[i].position);
				targetDecart1[i] = targets[i].position;
				targetDecart[i] = PvoUtils.PolarToDec(ConvertDecardToPolar(targets[i].position));
				targetVelocities[i] = targets[i].GetComponent<TargetMover>().velocity;
			} else {
				targetPositions.Add(ConvertDecardToPolar(targets[i].position));
				targetDecart1.Add(targets[i].position);
				targetDecart.Add(PvoUtils.PolarToDec(ConvertDecardToPolar(targets[i].position)));
				targetVelocities.Add(targets[i].GetComponent<TargetMover>().velocity);
			}
	}

	private void UpdateBeam() {
		beamOrientation.x = puAzimut.debug;
		beamOrientation.y = puElevation.debug;
		beamSize.x = widthBeam.debug;
		beamSize.y = heightBeam.debug;
	}

	private Vector3 ConvertDecardToPolar(Vector3 vector3) => PvoUtils.DecToPolar(vector3 - pvo.position);

	private bool CheckTargetInBeam(out Vector3 position, out Vector3 velocity) {
		position = default;
		velocity = default;
		var index = -1;
		float length;
		var minLength = float.MaxValue;
		foreach (var p in targetPositions) {
			if (AngleUtils.GetRangeBetweenAnglesAbs(p.y, beamOrientation.x) > beamSize.x / 2 ||
					AngleUtils.GetRangeBetweenAnglesAbs(p.z, beamOrientation.y) > beamSize.y / 2) { continue; }
			length = Mathf.Pow(AngleUtils.GetRangeBetweenAnglesAbs(p.y, beamOrientation.x), 2) +
							Mathf.Pow(AngleUtils.GetRangeBetweenAnglesAbs(p.z, beamOrientation.y), 2);
			if (length < minLength) {
				minLength = length;
				index = targetPositions.IndexOf(p);
			}
		}
		if (index == -1) return false;
		position = targetPositions[index];
		velocity = targetVelocities[index];
		return true;
	}

	public void SetDefaultPosition() {
		targetPosition.x = float.NaN;
		targetPosition.y = float.NaN;
		targetPosition.z = float.NaN;
		targetVelocity.x = float.NaN;
		targetVelocity.y = float.NaN;
		targetVelocity.z = float.NaN;
	}

	public float getDistance() { return targetPosition.x; }

	public float getAzimut() { return targetPosition.y; }

	public float getElevation() { return targetPosition.z; }

	public float getVelocityX() { return targetVelocity.x; }

	public float getVelocityY() { return targetVelocity.y; }

	public float getVelocityZ() { return targetVelocity.z; }

	public bool isEnemy() { return true; }
}