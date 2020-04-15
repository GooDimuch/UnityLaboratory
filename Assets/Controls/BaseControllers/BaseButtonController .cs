using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class BaseButtonController : BaseControlController<bool> {

#region constants
#endregion

#region inspector
	public Axis axisPush;
	public float heightPush;
#endregion

#region internal variable
#endregion

	public void Awake() { }

	public new void Start() { base.Start(); }

	protected override bool GetCurrentState(bool currentState, bool newState) { return newState; }

	private Vector3 v3;

	public override void Animate(bool newState) {
		v3[(int) axisPush] = newState ? -heightPush : 0;
		transform.localPosition = v3;
	}
}