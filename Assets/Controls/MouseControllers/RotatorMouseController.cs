using UnityEngine;

public class RotatorMouseController : BaseRotatorController {
	private float ROTATE_DELAY = 0.2f;
	private float COEFFICIENT_SPEED = 0.003f;
	private float leftTimer;
	private float rightTimer;

	public new void Start() { base.Start(); }

	private void OnMouseOver() {
		var shift = GetShift();
		var speed = GetSpeed(numberStates);
		shift *= Mathf.Clamp(speed, 1f, numberStates);
		ChangeState(shift);
	}

	private float GetShift() {
		if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1)) { RefreshCounters(); }
		if (Input.GetMouseButton(0)) { leftTimer += Time.deltaTime; }
		if (Input.GetMouseButton(1)) { rightTimer += Time.deltaTime; }

		if (leftTimer > ROTATE_DELAY) { return -1; }
		if (rightTimer > ROTATE_DELAY) { return 1; }

		if (Input.GetMouseButtonDown(0)) { return -1; }
		if (Input.GetMouseButtonDown(1)) { return 1; }
		return 0;
	}

	private void RefreshCounters() {
		leftTimer = 0;
		rightTimer = 0;
	}

	private float GetSpeed(int states) => Mathf.Exp(Mathf.Abs(leftTimer - rightTimer) * states * COEFFICIENT_SPEED - 1);
}