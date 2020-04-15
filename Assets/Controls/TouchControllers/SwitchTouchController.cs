using UnityEngine;

public class SwitchTouchController : BaseSwitchController, ITouchListener {
#region constants
	public float SWIPE_LENGTH = 30f; //длина свайпа для изменения state
#endregion

#region inspector
	public Direction direction;
#endregion

#region internal variable
	private bool startingDrag;
	private Vector2 beginDragPosition;
#endregion

	public new void Awake() {
		base.Awake();
		SWIPE_LENGTH = Mathf.Abs(SWIPE_LENGTH);
	}

	public new void Start() { base.Start(); }

	public void OnTouchStart(Vector2 position) {
		beginDragPosition = position;
		startingDrag = true;
	}

	public void OnTouchDrug(Vector2 position) {
		if (startingDrag) {
			beginDragPosition = position;
			startingDrag = false;
		}
		if (Mathf.Abs(getProjectionOnDirection(position - beginDragPosition)) > SWIPE_LENGTH) {
			ChangeState((int) Mathf.Sign(getProjectionOnDirection(position - beginDragPosition)));
			beginDragPosition = position;
		}
	}

	public void OnTouchEnd(Vector2 position) { }

	private float getProjectionOnDirection(Vector2 dragVector) {
		return direction == Direction.Vertical ? dragVector.y : dragVector.x;
	}
}