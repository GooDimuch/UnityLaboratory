using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;

public class PipeController : TimerMonoBehaviour {
	public float start = 10;
	public float end = -10;
	public float velocity = 10;
	public float frequency = 5000;
	public GameObject prefab;

	private readonly List<GameObject> _pipes = new List<GameObject>();

	private void Start() { CreatePipes(); }

	private void CreatePipes() {
		_pipes.Add(CreatePipe());
		ActionWithDelay(frequency, CreatePipes);
	}

	private GameObject CreatePipe() {
		var pipe = PrefabUtility.InstantiatePrefab(prefab, transform) as GameObject ?? throw new Exception("pipe is null");
		pipe.transform.localPosition = Vector3.right * start;
		var rb = pipe.AddComponent<Rigidbody>();
		rb.useGravity = false;
		rb.isKinematic = true;
		return pipe;
	}

	private void Update() {
		switch (GameController.Instance.Status) {
			case GameController.GameStatus.None: break;
			case GameController.GameStatus.Play:
				base.Update();
				MovePipes();
				RemovePipes();
				break;
			case GameController.GameStatus.Pause: break;
			case GameController.GameStatus.GameOver: break;
			case GameController.GameStatus.Restart:
				Restart();
				break;
			default: throw new ArgumentOutOfRangeException();
		}
	}

	private void Restart() {
		for (var i = 0; i < _pipes.Count; i++) {
			var pipe = _pipes[i];
			Destroy(pipe);
			_pipes.RemoveAt(i);
			i--;
		}
	}

	private void MovePipes() {
		foreach (var pipe in _pipes) { pipe.transform.position += Vector3.left * velocity / (Time.deltaTime * 1000); }
	}

	private void RemovePipes() {
		for (var i = 0; i < _pipes.Count; i++) {
			var pipe = _pipes[i];
			if (pipe.transform.position.x > end) continue;
			Destroy(pipe);
			_pipes.RemoveAt(i);
			i--;
		}
	}
}