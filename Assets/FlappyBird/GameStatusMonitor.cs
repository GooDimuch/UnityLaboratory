using System;
using UnityEngine;

public class GameStatusMonitor : MonoBehaviour {
	public GameObject panel;
	public GameObject pause;
	public GameObject gameOver;
	private void Start() { }

	private void Update() {
		switch (GameController.Instance.Status) {
			case GameController.GameStatus.None: break;
			case GameController.GameStatus.Play:
				panel.SetActive(false);
				pause.SetActive(false);
				gameOver.SetActive(false);
				break;
			case GameController.GameStatus.Pause:
				panel.SetActive(true);
				pause.SetActive(true);
				break;
			case GameController.GameStatus.GameOver:
				panel.SetActive(true);
				gameOver.SetActive(true);
				break;
			case GameController.GameStatus.Restart: 
				panel.SetActive(false);
				break;
			default: throw new ArgumentOutOfRangeException();
		}
	}
}