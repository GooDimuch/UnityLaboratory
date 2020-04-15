using UnityEngine;
using System.Collections.Generic;

public interface ICameras {
	Camera GetCurrentCamera();
}

public class TouchProcessor : MonoBehaviour {
	private Dictionary<int, Touch> currentInput = new Dictionary<int, Touch>();
	private List<HandleHit> currentHit = new List<HandleHit>();

	private bool MouseMode;
	private Touch touch_mouse_mode;

	public ICameras DisplayController;

	private void Start() { Input.simulateMouseWithTouches = false; }

	private void UpdateInput() {
		if (Input.GetMouseButtonDown(0)) {
			currentInput.Clear();
			currentHit.Clear();
			MouseMode = true;
			touch_mouse_mode.phase = TouchPhase.Began;
		}

		if (MouseMode) {
			if (Input.GetMouseButtonUp(0)) {
				MouseMode = false;
				touch_mouse_mode.phase = TouchPhase.Ended;
			}
			touch_mouse_mode.fingerId = 0;
			touch_mouse_mode.position = Input.mousePosition;
			currentInput[0] = touch_mouse_mode;
			touch_mouse_mode.phase = TouchPhase.Moved;
		} else {
			for (var i = 0; i < Input.touchCount && !MouseMode; i++)
				currentInput[Input.GetTouch(i).fingerId] = Input.GetTouch(i);
		}
	}

	void Update() {
		UpdateInput();

		foreach (var input in currentInput) {
			if (input.Value.phase != TouchPhase.Began) { continue; }
			var currentCamera = DisplayController.GetCurrentCamera();
			var ray = currentCamera.ScreenPointToRay(GetInputPosition(input));
			var layerMask = currentCamera.cullingMask;
			var maxDistance = currentCamera.farClipPlane;

			if (!Physics.Raycast(ray.origin, Vector3.back, out var hit, maxDistance, layerMask) ||
					hit.transform.gameObject.GetComponent<ITouchListener>() == null) { continue; }

			if (currentHit.Find(x => x.gameObject.GetInstanceID() == hit.transform.gameObject.GetInstanceID()) == null) {
				currentHit.Add(new HandleHit(hit, input.Key, currentInput));
			}
		}

		foreach (var curHit in currentHit.ToArray()) {
			curHit.update(currentInput);
			if (!curHit.Alive) currentHit.Remove(curHit);
		}
	}

#if (UNITY_EDITOR)
	public static Vector3 GetInputPosition(KeyValuePair<int, Touch> input) { return input.Value.position; }
#else
		public static Vector3 GetInputPosition(KeyValuePair<int, Touch> input) {
			return new Vector2(Display.RelativeMouseAt(input.Value.position).x,
												Display.RelativeMouseAt(input.Value.position).y);
		}
#endif

	private class HandleHit {
		public bool Alive { get; private set; } = true;

		public readonly GameObject gameObject;

		private readonly int inputIndex;
		private readonly List<ITouchListener> listeners;

		public HandleHit(RaycastHit hit, int _indexInput, Dictionary<int, Touch> dataTouch) {
			listeners = new List<ITouchListener>(hit.transform.gameObject.GetComponents<ITouchListener>());
			gameObject = hit.transform.gameObject;
			inputIndex = _indexInput;

			if (listeners.Count == 0 || listeners == null) {
				Alive = false;
				return;
			}
			foreach (ITouchListener listener in listeners) listener.OnTouchStart(dataTouch[inputIndex].position);
		}

		public void update(Dictionary<int, Touch> input) {
			if (!Alive) return;

			if (input[inputIndex].phase == TouchPhase.Ended) {
				foreach (ITouchListener listener in listeners) listener.OnTouchEnd(input[inputIndex].position);
				Alive = false;
				return;
			}
			foreach (ITouchListener listener in listeners) listener.OnTouchDrug(input[inputIndex].position);
		}
	}
}