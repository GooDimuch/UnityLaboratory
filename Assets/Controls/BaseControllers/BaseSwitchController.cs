using System;
using System.Text.RegularExpressions;
using MyBox;
using UnityEngine;

public abstract class BaseSwitchController : BaseControlController<int> {
	public enum Trigger {
		Down,
		Mid,
		UP,
	}

	public enum Animation {
		States,
		Rotation,
	}

	public enum Direction {
		Vertical,
		Horizontal,
	}

#region constants
#endregion

#region inspector
	public bool invertRotation;
	[Header("For states anim name = 'Switch'")] public Animation anim;

	[ConditionalField(nameof(anim), false, Animation.Rotation)]
	public Axis axis;

	[ConditionalField(nameof(anim), false, Animation.Rotation)]
	public int numberStates;

	[ConditionalField(nameof(anim), false, Animation.Rotation)]
	public float angleDeviation;
#endregion

#region internal variable
	protected Animator animator;
	[ReadOnly, SerializeField, Header("")] protected int nStates;
#endregion

	public void Awake() {
		animator = gameObject.GetComponent<Animator>();
		nStates = anim == Animation.States ? GetNumberStates() : numberStates;
	}

	public new void Start() {
		if (anim == Animation.Rotation && TryGetComponent(out animator)) { animator.enabled = false; }
		base.Start();
	}

	private int GetNumberStates() {
		var nameAnimatorController = animator.runtimeAnimatorController.name.ToLower();
		var shortName = Regex.Matches(nameAnimatorController, "switch(.*?)controller")[0].ToString();
		return Convert.ToInt32(shortName.Replace("switch", "").Replace("controller", ""));
	}

	protected override int GetCurrentState(int currentState, int shift) {
		if (invertRotation) { shift *= -1; }
		currentState += shift;
		return Mathf.Clamp(currentState, 0, nStates - 1);
	}

	public override void Animate(int newState) {
		switch (anim) {
			case Animation.States:
				StateAnimate(newState);
				return;
			case Animation.Rotation:
				RotateAnimate(newState);
				return;
			default: return;
		}
	}

	private void StateAnimate(int newState) {
		if (nStates == 2) { newState *= 2; }
		gameObject.GetComponent<Animator>().SetTrigger(((Trigger) newState).ToString());
	}

	private void RotateAnimate(int newState) {
		if (nStates == 2) { newState *= 2; }
		newState -= 1;
		var newAngle = newState * angleDeviation;
		var directionVector = Vector3.zero;
		directionVector[(int) axis] = 1;
		transform.localRotation = Quaternion.AngleAxis(newAngle, directionVector);
	}
}