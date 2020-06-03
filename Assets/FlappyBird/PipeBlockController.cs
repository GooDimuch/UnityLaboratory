using UnityEngine;

public class PipeBlockController : MonoBehaviour {
	private const float Height = 3;
	private const float Length = 11;

	public GameObject Block1;
	public GameObject Block2;

	private float value;

	private void Start() {
		value = (Random.value - 0.5f) * (Length - Height);

		var scale1 = Mathf.Abs(Length / 2 - (value + Height / 2));
		Block1.transform.localPosition = Vector3.up * (value + Height / 2 + scale1 / 2);
		Block1.transform.localScale = new Vector3(1, scale1, 1);

		var scale2 = Mathf.Abs(Length / 2 + (value - Height / 2));
		Block2.transform.localPosition = Vector3.up * (value - Height / 2 - scale2 / 2);
		Block2.transform.localScale = new Vector3(1, scale2, 1);
	}

	private void Update() { }
}