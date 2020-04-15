using System;
using Unity.Collections;
using UnityEngine;

public class TargetMover : MonoBehaviour {
	private const float BASE_HEIGHT = 0f;

	public enum MoveType {
		Circular,
		CircularSin,
		InDirection,
	}

	public MoveType moveType;

#region Circular
	[Header("For Circular")] public float startAngle;
	[ReadOnly, SerializeField] private float angleSpeed;
	public float radius;
	private float angle;
#endregion

#region InDirection
	[Header("For InDirection")] public Vector3 startPos;
	public Vector3 direction;
#endregion

	[Header("For any")] public float speed;
	[ReadOnly] public Vector3 velocity;
	public float height;
	private float x;
	private float y;
	private Vector3 newPos;

	public Transform pvo;
	[ReadOnly] public Vector3 dirRelativeRocket;

	private void Start() {
		height += BASE_HEIGHT;
		switch (moveType) {
			case MoveType.Circular:
			case MoveType.CircularSin:
				startAngle = -startAngle + 90;
				angle = startAngle;
				transform.position = GetCircularPos(moveType);
				break;
			case MoveType.InDirection:
				transform.position = startPos;
				break;
			default: throw new ArgumentOutOfRangeException();
		}
	}

	private void Update() {
		switch (moveType) {
			case MoveType.Circular:
			case MoveType.CircularSin:
				angleSpeed = AngleUtils.Rad2Deg(speed / radius);
				newPos = GetCircularPos(moveType);
				velocity = (newPos - transform.position).normalized * speed;
				transform.position = newPos;
				break;
			case MoveType.InDirection: break;
			default: throw new ArgumentOutOfRangeException();
		}
		dirRelativeRocket = PvoUtils.DecToPolar(transform.position - pvo.position);
	}

	private Vector3 GetCircularPos(MoveType type) {
		angle -= angleSpeed * Time.deltaTime;
		x = Mathf.Cos(AngleUtils.Deg2Rad(angle)) * radius;
		y = Mathf.Sin(AngleUtils.Deg2Rad(angle)) * radius;
		return new Vector3(x, type == MoveType.CircularSin ? GetHeight(angle, height) : height, y);
	}

	private float GetHeight(float angle, float height) =>
			height + Mathf.Sin(AngleUtils.Deg2Rad(AngleUtils.NormalizeAngle(angle)) * 2) * height / 4;
}