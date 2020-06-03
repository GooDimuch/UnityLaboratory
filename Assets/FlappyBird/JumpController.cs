using System;
using UnityEngine;

public class JumpController : MonoBehaviour {
	public float Power;
	public Vector3 Direction;

	private Rigidbody _rigidbody;

	private void Start() { _rigidbody = GetComponent<Rigidbody>(); }

	private void Update() {
		_rigidbody.isKinematic = GameController.Instance.Status != GameController.GameStatus.Play;
		switch (GameController.Instance.Status) {
			case GameController.GameStatus.None: break;
			case GameController.GameStatus.Play:
				if (GameController.Instance.Status != GameController.GameStatus.Play) { return; }
				if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
					_rigidbody.AddForce(Direction.normalized * Power);
				break;
			case GameController.GameStatus.Pause: break;
			case GameController.GameStatus.GameOver: break;
			case GameController.GameStatus.Restart:
				transform.position = Vector3.zero;
				break;
			default: throw new ArgumentOutOfRangeException();
		}
	}
}