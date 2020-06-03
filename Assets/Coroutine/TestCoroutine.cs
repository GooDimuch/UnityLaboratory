using System.Collections;
using UnityEngine;

public class TestCoroutine : MonoBehaviour {
	void Start() { StartCoroutine("rot45"); }

	void Update() {
		if (Input.GetKeyDown(KeyCode.Q)) { StartCoroutine("rot45"); }
		if (Input.GetKey(KeyCode.E)) { Rotate(1); }
	}

	private IEnumerator rot45() {
		Debug.Log("ololo");
		for (var i = 0; i < 3; i++) {
			Rotate(10f);
			yield return new WaitForSeconds(1f);
		}
		yield return StartCoroutine(rot45());
	}

	private void Rotate(float step) {
		var rot = transform.localRotation.eulerAngles;
		rot[2] += step;
		transform.localRotation = Quaternion.Euler(rot);
	}
}