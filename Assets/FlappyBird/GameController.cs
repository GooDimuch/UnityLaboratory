using System;
using UnityEngine;

public class GameController : MonoBehaviour {
	public static GameController Instance { get; private set; }

	public enum GameStatus {
		None,
		Play,
		Pause,
		GameOver,
		Restart,
	}

	private GameStatus _status;

	public GameStatus Status {
		get => _status;
		set {
			_status = value;
			Debug.Log(Status);
		}
	}

	private GameController() { }

	private void Awake() { Instance = this; }

	private void Start() { }

	private void Update() {
		switch (Status) {
			case GameStatus.None:
				if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) { Status = GameStatus.Play; }
				break;
			case GameStatus.Play:
				if (Input.GetKeyDown(KeyCode.P)) { Status = GameStatus.Pause; }
				break;
			case GameStatus.Pause:
				if (Input.GetKeyDown(KeyCode.P)) { Status = GameStatus.Play; }
				break;
			case GameStatus.GameOver:
				if (Input.GetKeyDown(KeyCode.R)) { Status = GameStatus.Restart; }
				break;
			case GameStatus.Restart:
				if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) { Status = GameStatus.Play; }
				break;
			default: throw new ArgumentOutOfRangeException();
		}
	}
}