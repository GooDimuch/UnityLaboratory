using UnityEngine;

public class BeamSize : BaseCrewSimToUnity<float> {
	public enum Type {
		Height,
		Width,
	}

	[Range(0, 180)] public float value;
	public Type type;

	private new void Start() { base.Start(); }

	private new void Update() { base.Update(); }

	protected override float GetValue() => value;

	public override void Animate(float angle) {
		var length = transform.localScale.x;
		var size = Mathf.Tan(AngleUtils.Deg2Rad(angle / 2)) * length;
		var curScale = transform.localScale;
		curScale[(int) (type + 1)] = size;
		transform.localScale = curScale;
	}
}