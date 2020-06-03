using UnityEngine;

public class LifeController : MonoBehaviour {
	private void Start() { }

	private void Update() {
		if (GameController.Instance.Status == GameController.GameStatus.Play &&
				(transform.position.y > 5.5 || transform.position.y < -5.5)) {
			GameController.Instance.Status = GameController.GameStatus.GameOver;
		}
	}

	private void OnCollisionEnter(Collision collision) {
		GameController.Instance.Status = GameController.GameStatus.GameOver;
	}
}