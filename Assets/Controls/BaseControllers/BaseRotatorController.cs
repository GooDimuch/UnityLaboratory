using UnityEngine;

public abstract class BaseRotatorController : BaseControlController<float> {
#region constants
#endregion

#region inspector
	public Axis axisOfRotation;
	public float startAngle;
	public float step;
	public int numberStates;
	public bool circular;
	public bool invertRotation;
#endregion

#region internal variable
#endregion

	public void Awake() { }

	public new void Start() {
		numberStates = Mathf.Abs(numberStates);
		step = Mathf.Abs(step);
		if (circular) { step = 360f / numberStates; }
		if (invertRotation) { step *= -1; }
		base.Start();
	}

	protected override float GetCurrentState(float currentState, float value) {
		if (invertRotation) { value *= -1; }
		currentState += value;

		if (circular) {
			if (currentState > numberStates - 1) { currentState -= numberStates; }
			if (currentState < 0) { currentState += numberStates; }
		} else { currentState = Mathf.Clamp(currentState, 0, numberStates - 1); }
		return currentState;
	}

	public override void Animate(float newState) {
		var newAngle = startAngle + newState * step;
		var directionVector = Vector3.zero;
		directionVector[(int) axisOfRotation] = 1;
		transform.localRotation = Quaternion.AngleAxis(newAngle, directionVector);
	}
}