using System;
using Unity.Collections;
using UnityEngine;

public class PuRotator : BaseCrewSimToUnity<float> {
	public enum RotateType {
		Azimut,
		Elevation,
	}

	private const float BASE_H_SPEED = 1;
	private const float BASE_V_SPEED = 1;
	public float speed = 10f;
	private const float pMinHAngle = -5f;
	private const float pMaxHAngle = 80f;

	public RotateType rotateType;
	public string inputName;
	public float startAngle;
	public float step;
	public bool invert;
	[ReadOnly] public float value = 0f;

	// private Vector2 localOrientation;
	private const float TOLERANCE = 0.001f;

	private new void Start() { }

	private new void Update() {
		value = rotateType == RotateType.Azimut
				? GetAzimut(Input.GetAxis(inputName))
				: GetElevation(Input.GetAxis(inputName));
		base.Update();
	}

	private float GetAzimut(float potential) =>
			AngleUtils.NormalizeAngle(potential * Time.deltaTime * 360 / (BASE_H_SPEED * speed));

	private float GetElevation(float potential) =>
			AngleUtils.NormalizeAngle180(potential * Time.deltaTime * 360 / (BASE_V_SPEED * speed));

	protected override void PreAnimate() { }

	protected override float GetValue() => value;

	public override void Animate(float newState) {
		if (Math.Abs(newState) < TOLERANCE) { return; }
		newState *= invert ? -1 : 1;
		var newAngle = startAngle + newState * step;
		var curAngle = transform.localRotation.eulerAngles;
		curAngle[(int) (1 - rotateType)] += newAngle;
		if (rotateType == RotateType.Elevation) {
			curAngle[(int) (1 - rotateType)] =
					Mathf.Clamp(AngleUtils.NormalizeAngle180(curAngle[(int) (1 - rotateType)]), -pMaxHAngle, -pMinHAngle);
		}
		transform.localRotation = Quaternion.Euler(curAngle);
	}
}