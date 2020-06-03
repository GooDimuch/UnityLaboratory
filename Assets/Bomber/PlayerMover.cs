using UnityEngine;

public class PlayerMover : MonoBehaviour {
	public float velocity;
	public float sensitivity = 10f;
	public float maxYAngle = 80f;

	public GameObject head;
	public GameObject hands;
	public GameObject legs;

	private Vector2 currentRotation;
	private bool isMove;

	private void Start() { }

	private void Update() {
		currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
		transform.localRotation = Quaternion.Euler(0, currentRotation.x, 0);
		currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
		currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
		head.transform.localRotation = Quaternion.Euler(currentRotation.y, 0, 0);

		if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
			if (isMove) { GetComponent<Animator>().SetTrigger("stop"); }
			isMove = false;
		}

		if (Input.GetKey(KeyCode.W)) {
			transform.position += transform.forward * velocity;
			if (!isMove) { GetComponent<Animator>().SetTrigger("move"); }
			isMove = true;
		}
		if (Input.GetKey(KeyCode.S)) {
			transform.position -= transform.forward * velocity;
			if (!isMove) { GetComponent<Animator>().SetTrigger("move"); }
			isMove = true;
		}
		if (Input.GetKey(KeyCode.A)) {
			transform.position -= transform.right * velocity;
			if (!isMove) { GetComponent<Animator>().SetTrigger("move"); }
			isMove = true;
		}
		if (Input.GetKey(KeyCode.D)) {
			transform.position += transform.right * velocity;
			if (!isMove) { GetComponent<Animator>().SetTrigger("move"); }
			isMove = true;
		}
		if (transform.position.y < -1) { UnityEditor.EditorApplication.isPlaying = false; }
	}
}