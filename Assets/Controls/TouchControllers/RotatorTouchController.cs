using UnityEngine;

public class RotatorTouchController : BaseRotatorController, ITouchListener {
	public enum WayOfRotate {
		First,
		Second,
		Third
	}

#region constants
	public float SWIPE_LENGTH = 30f; //для 1 способа, длина свайпа для изменения state
	public float OFFSET_LENGTH = 30f; //для 3 способа, радиус в котором не срабатывает поворот
#endregion

#region inspector
	public WayOfRotate wayOfRotate;
	public float changeStateZone = 0.7f;
#endregion

#region internal variable
	protected bool startingDrag;
	protected Vector2 beginDragPosition;
	protected float beginDragPositionM;
	private Vector3 startDirection;
	private float deviationAngle;
#endregion

	public new void Awake() {
		base.Awake();
		OFFSET_LENGTH = Mathf.Abs(OFFSET_LENGTH);
	}

	public new void Start() { base.Start(); }

	public void OnTouchStart(Vector2 position) {
		switch (wayOfRotate) {
			case WayOfRotate.First:
				FirstTouchStart(position);
				break;

			case WayOfRotate.Second:
				SecondTouchStart(position);
				break;

			case WayOfRotate.Third:
				ThirdTouchStart(position);
				break;

			default: return;
		}
	}

	public void OnTouchDrug(Vector2 position) {
		switch (wayOfRotate) {
			case WayOfRotate.First:
				FirstTouchDrug(position);
				break;

			case WayOfRotate.Second:
				SecondTouchDrug(position);
				break;

			case WayOfRotate.Third:
				ThirdTouchDrug(position);
				break;

			default: return;
		}
	}

	public void OnTouchEnd(Vector2 position) {
		switch (wayOfRotate) {
			case WayOfRotate.First:
				FirstTouchEnd(position);
				break;

			case WayOfRotate.Second:
				SecondTouchEnd(position);
				break;

			case WayOfRotate.Third:
				ThirdTouchEnd(position);
				break;

			default: return;
		}
	}

#region firstWay
	private void FirstTouchStart(Vector2 position) {
		beginDragPosition = position;
		beginDragPositionM = position.magnitude;
		startingDrag = true;
	}

	private void FirstTouchDrug(Vector2 position) {
		if (startingDrag) {
			beginDragPositionM = beginDragPosition.x + position.x;
			startingDrag = false;
		}

		if (beginDragPosition.x + position.x > beginDragPositionM + Mathf.Abs(SWIPE_LENGTH)) {
			ChangeState(-1);
			beginDragPositionM = beginDragPosition.x + position.x;
		} else if (beginDragPosition.x + position.x < beginDragPositionM - Mathf.Abs(SWIPE_LENGTH)) {
			ChangeState(1);
			beginDragPositionM = beginDragPosition.x + position.x;
		}
	}

	private void FirstTouchEnd(Vector2 position) { }
#endregion

#region secondWay
	private void SecondTouchStart(Vector2 position) {
		if (Input.touchCount < 2) return;
		beginDragPosition = position;
		var diff = Input.GetTouch(1).position - Input.GetTouch(0).position;
		beginDragPositionM = (Mathf.Atan2(diff.y, diff.x) / Mathf.PI) * 180;
		if (beginDragPositionM < 0) beginDragPositionM += 360f;
		startingDrag = true;
		Debug.Log(startingDrag);
	}

	private void SecondTouchDrug(Vector2 position) {
		if (Input.touchCount < 2) { return; }

		var diff = Input.GetTouch(1).position - Input.GetTouch(0).position;
		var angle = (Mathf.Atan2(diff.y, diff.x) / Mathf.PI) * 180;
		if (angle < 0) angle += 360f;

		if (startingDrag) {
			angle = beginDragPositionM;
			startingDrag = false;
		}

		if (angle > beginDragPositionM + Mathf.Abs(step)) {
			ChangeState(-1);
			beginDragPositionM = angle;
		} else if (angle < beginDragPositionM - Mathf.Abs(step)) {
			ChangeState(1);
			beginDragPositionM = angle;
		}
	}

	private void SecondTouchEnd(Vector2 position) { }
#endregion

#region thirdWay
	private void ThirdTouchStart(Vector2 position) { beginDragPosition = position; }

	private void ThirdTouchDrug(Vector2 position) {
		startDirection = GetStartDirection(axisOfRotation);
		Vector2 touchDirection = beginDragPosition - position;
		if (touchDirection.magnitude < OFFSET_LENGTH) { return; }

		deviationAngle = Vector3.SignedAngle(startDirection, touchDirection, Vector3.back);
		int newState = GetNewState(deviationAngle);
		if (newState == 0) { return; }

		ChangeState(newState);
	}

	private int GetNewState(float angle) {
		if ((int) (angle / Mathf.Abs(step * changeStateZone)) == 0) { return 0; }

		if (numberStates < 10) { return (int) Mathf.Sign(angle); }

		return (int) (Mathf.Clamp(Mathf.Abs(angle), 1, numberStates / 10f) * Mathf.Sign(angle));
	}

	private Vector2 GetStartDirection(Axis axis) {
		Vector3 startDirection3;
		switch (axis) {
			case Axis.X: return default;
			case Axis.Y:
				startDirection3 = transform.localRotation * Vector3.forward;
				return new Vector2(startDirection3.x, startDirection3.z) * -1;
			case Axis.Z:
				startDirection3 = transform.localRotation * Vector3.up;
				return new Vector2(startDirection3.x, startDirection3.y * -1);

			default: return default;
		}
	}

	private void ThirdTouchEnd(Vector2 position) { }
#endregion
}