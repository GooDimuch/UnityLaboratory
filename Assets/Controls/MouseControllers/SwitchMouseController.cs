using MyBox;
using UnityEngine;

public class SwitchMouseController : BaseSwitchController {
#region constants
	private const float DELAY_TIME = 100f;
#endregion

#region inspector
	[Header("If return to state")] public bool isReturn;
	[ConditionalField(nameof(isReturn))] public Trigger state;
#endregion

#region internal variable
	private bool isOver;
	private bool pressed;
	private bool lmbPressed;
	private bool rmbPressed;
	private bool lmbIsPressed;
	private bool rmbIsPressed;
	private float delay;
#endregion

	public new void Awake() { base.Awake(); }

	public new void Start() { base.Start(); }

	public void Update() {
		if (!isOver && !pressed) {
			if (isReturn) { ReturnSwitch(); }
			return;
		}
		lmbPressed = Input.GetMouseButton(0);
		rmbPressed = Input.GetMouseButton(1);
		lmbIsPressed = Input.GetMouseButtonDown(0);
		rmbIsPressed = Input.GetMouseButtonDown(1);
		if (lmbIsPressed || rmbIsPressed) {
			pressed = true;
			delay = 0;
			var shift = lmbIsPressed ? 1 : (rmbIsPressed ? -1 : 0);
			ChangeState(shift);
		} else if (!lmbPressed && !rmbPressed && pressed) {
			delay += Time.deltaTime * 1000f;
			if (delay < DELAY_TIME) { return; }
			pressed = false;
			if (!isReturn) { return; }
			ReturnSwitch();
		}
		ChangeState(0);
	}

	private void ReturnSwitch() {
		var shift = (nStates == 2 ? (int) state / 2 : (int) state) - CurrentState;
		ChangeState(shift);
	}

	void OnMouseOver() { isOver = true; }

	void OnMouseExit() { isOver = false; }
}