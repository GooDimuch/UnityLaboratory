using UnityEngine;

public class TrailController : MonoBehaviour {
	public enum Axis {
		X,
		Y,
		Z
	}

	// public Transform CameraTransform;
	public Material LineMaterial;
	public float width;

	private Vector3 cameraPosition;

	void Start() {
		// cameraPosition = CameraTransform.position;
		for (int i = 0; i < 1000; i++) { CreateLine(); }
	}

	void Update() { }

	private GameObject CreateLine(string name = "") {
		var go = new GameObject($"Line_{name}");
		go.transform.parent = transform;
		var trailR = go.AddComponent<TrailRenderer>();
		trailR.startWidth = width;
		trailR.endWidth = width / 100f;
		trailR.material = LineMaterial;
		trailR.time = 1f;
		return go;
	}
}