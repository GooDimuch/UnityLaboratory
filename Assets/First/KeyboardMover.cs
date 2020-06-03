using UnityEngine;

public class KeyboardMover : MonoBehaviour {
	public float speed = 1;

	private void Start() {
		Debug.Log("start");

		var c1 = GetComponent(typeof(Transform));
		Debug.Log(c1);
		var c2 = GetComponent(typeof(KeyboardMover));
		Debug.Log(c2);
		var c3 = GetComponent(typeof(Camera));
		Debug.Log(c3);
		if (TryGetComponent(typeof(Camera), out var cam)) { Debug.Log(cam); } else { Debug.Log("Not found"); }
	}

	private void Update() {
		Debug.Log("update");
		if (Input.GetKey(KeyCode.LeftArrow)) { Move(Vector3.left); }
		if (Input.GetKey(KeyCode.RightArrow)) { Move(Vector3.right); }
		if (Input.GetKey(KeyCode.UpArrow)) { Move(Vector3.up); }
		if (Input.GetKey(KeyCode.DownArrow)) { Move(Vector3.down); }
		// for (int i = 0; i < 1000; i++) {
		// 	Debug.Log("");
		// }
	}

	public void Move(Vector3 direction) { transform.position += direction.normalized * speed * Time.deltaTime; }
}