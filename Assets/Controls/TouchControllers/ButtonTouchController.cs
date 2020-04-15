using UnityEngine;

public class ButtonTouchController : BaseButtonController, ITouchListener {
#region constants
#endregion

#region inspector
	public bool isButtonHold;
#endregion

#region internal variable
#endregion

	public new void Awake() { base.Awake(); }

	public new void Start() { base.Start(); }

	public void OnTouchStart(Vector2 position) {
		if (!isButtonHold) { ChangeState(true); } else { ChangeState(!CurrentState); }
	}

	public void OnTouchDrug(Vector2 position) { }

	public void OnTouchEnd(Vector2 position) {
		if (!isButtonHold) ChangeState(false);
	}
}