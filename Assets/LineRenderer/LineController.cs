using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class LineController : MonoBehaviour {
	public enum Axis {
		X,
		Y,
		Z
	}

	public Transform CameraTransform;
	public Material LineMaterial;
	public int numberOfLines;
	public float length;
	public float width;
	public Vector3 cameraOffset;
	public bool move;
	public float delta;

	private Vector3 cameraPosition;
	[ReadOnly] private int parts = 2;
	private float deltaCounter;

	private readonly List<LineRenderer> lineList = new List<LineRenderer>();
	private readonly List<List<Vector3>> linePoints = new List<List<Vector3>>();

	void Start() { cameraPosition = CameraTransform.position; }

	void Update() {
		delta = Time.deltaTime * 1000;
		deltaCounter += delta;
		if (delta > 100) { Debug.Log(delta); }
		UpdateNumberOfLines();
		for (var i = 0; i < lineList.Count; i++) {
			SetPoints(i);
			drawGraph(lineList[i], linePoints[i]);
		}
	}

	private GameObject CreateLine(string name = "") {
		var go = new GameObject($"Line_{name}");
		go.transform.parent = transform;
		var lineR = go.AddComponent<LineRenderer>();
		lineR.startWidth = width/100f;
		lineR.endWidth = width;
		lineR.material = LineMaterial;
		return go;
	}

	private void UpdateNumberOfLines() {
		if (linePoints.Count == numberOfLines) { return; }
		var count = numberOfLines - linePoints.Count;
		if (count > 0) { allocateMemory(count); } else { freeUpMemory(Mathf.Abs(count)); }
	}

	private void allocateMemory(int count) {
		for (int i = 0; i < count; i++) {
			lineList.Add(CreateLine($"{lineList.Count + 1}").GetComponent<LineRenderer>());
			var points = new List<Vector3>();
			for (var j = 0; j < parts; j++) { points.Add(new Vector3()); }
			linePoints.Add(points);
		}
	}

	private void freeUpMemory(int count) {
		for (int i = 0; i < count; i++) {
			Destroy(lineList[lineList.Count - 1].gameObject);
			lineList.RemoveAt(lineList.Count - 1);
			linePoints.RemoveAt(linePoints.Count - 1);
		}
	}

	private Vector3 tempV3;

	private void SetPoints(int i) {
		tempV3 = new Vector3(0, (i + 1) * 360f / numberOfLines);
		for (int j = 0; j < parts; j++) {
			tempV3.x = (float) (j == 0 ? 0 : (j + 1)) / parts * length;
			if (move) { tempV3.y += Mathf.Sin(1 / 2f * j + deltaCounter / 100); }
			linePoints[i][j] = SwapAxises(PolarToDec(tempV3), Axis.Y, Axis.Z);
		}
	}

	public void drawGraph(LineRenderer lineRenderer, IList<Vector3> pointList) {
		lineRenderer.positionCount = pointList.Count;

		for (var i = 0; i < pointList.Count; i++) { lineRenderer.SetPosition(i, toGlobalCoordinate(pointList[i])); }
	}

	public Vector3 toLocalCoordinate(Vector3 position, float coefficient = 1) {
		position.x = toLocalCoordinate(position, Axis.X) / coefficient;
		position.y = toLocalCoordinate(position, Axis.Y) / coefficient;
		position.z = toLocalCoordinate(position, Axis.Z);
		return position;
	}

	public float toLocalCoordinate(Vector3 position, Axis axis) =>
			position[(int) axis] - cameraPosition[(int) axis] - cameraOffset[(int) axis];

	public Vector3 toGlobalCoordinate(Vector3 position) {
		position.x = toGlobalCoordinate(position, Axis.X);
		position.y = toGlobalCoordinate(position, Axis.Y);
		position.z = toGlobalCoordinate(position, Axis.Z);
		return position;
	}

	public float toGlobalCoordinate(Vector3 position, Axis axis) =>
			cameraPosition[(int) axis] + cameraOffset[(int) axis] + position[(int) axis];

	public const float ZERO_ACCURACY = 0.000001f;

	/// <summary>
	/// Конвертирует полярные координаты в декартовые. Используется координатная
	/// система unity, где ось Y направленна вверх.
	/// </summary>
	/// <param name="decart">Вектор в полярных координатах
	///												[дальность; азимутальный угол; угол места].</param>
	/// <returns>Возвращает вектор в декартовых координатах (Ось Y направленна вверх).</returns>
	public static Vector3 PolarToDec(Vector3 polar) {
		polar.y = 90 - polar.y;
		polar.z = 90 - polar.z;
		var vector = new Vector3(polar.x * Mathf.Sin(polar.z * Mathf.Deg2Rad) * Mathf.Cos(polar.y * Mathf.Deg2Rad),
				polar.x * Mathf.Cos(polar.z * Mathf.Deg2Rad),
				polar.x * Mathf.Sin(polar.z * Mathf.Deg2Rad) * Mathf.Sin(polar.y * Mathf.Deg2Rad));
		return vector;
	}

	/// <summary>
	/// Конвертирует декартовые координаты в полярные. Используется координатная
	/// система unity, где ось Y направленна вверх.
	/// </summary>
	/// <param name="decart">Вектор в декартовых координатах.
	///												(Ось Y направленна вверх)</param>
	/// <returns>Возвращает вектор в полярных координатах
	///						[дальность; азимутальный угол; угол места]</returns>
	public static Vector3 DecToPolar(Vector3 decart) =>
			decart == Vector3.zero
					? Vector3.zero
					: new Vector3(decart.magnitude,
							AngleUtils.NormalizeAngle(90 -
									AngleUtils.Rad2Deg(decart.x > 0 && decart.z >= 0 ? Mathf.Atan(decart.z / decart.x) :
											decart.x > 0 && decart.z < 0 ? Mathf.Atan(decart.z / decart.x) + 2 * Mathf.PI :
											decart.x < 0 ? Mathf.Atan(decart.z / decart.x) + Mathf.PI :
											Mathf.Abs(decart.x) < ZERO_ACCURACY && decart.z > 0 ? Mathf.PI / 2 :
											Mathf.Abs(decart.x) < ZERO_ACCURACY && decart.z < 0 ? 3 * Mathf.PI / 2 : 0)),
							AngleUtils.NormalizeAngle(90 - AngleUtils.Rad2Deg(Mathf.Acos(decart.y / decart.magnitude))));

	private float temp;

	private Vector3 SwapAxises(Vector3 vector3, Axis axis1 = Axis.X, Axis axis2 = Axis.Y) {
		temp = vector3[(int) axis1];
		vector3[(int) axis1] = vector3[(int) axis2];
		vector3[(int) axis2] = temp;
		return vector3;
	}

	private Vector3 Invert(Vector3 vector3, params Axis[] axises) {
		foreach (var axis in axises) { vector3[(int) axis] = -vector3[(int) axis]; }
		return vector3;
	}
}