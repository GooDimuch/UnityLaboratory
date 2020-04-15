public class ButtonMouseController : BaseButtonController {
#region constants
#endregion

#region inspector
	public bool isButtonHold;
#endregion

#region internal variable
#endregion

	public new void Awake() { base.Awake(); }

	public new void Start() { base.Start(); }

	void OnMouseDown() { ChangeState(!isButtonHold || !CurrentState); }

	void OnMouseUp() { ChangeState(isButtonHold && CurrentState); }
}