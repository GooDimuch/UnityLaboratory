using UnityEngine;

public class BeamPosition : BaseCrewSimToUnity<int> {
	[Range(-1, 3)] public int value;
	private CaptureMode CaptureMode;
	private MeshRenderer BeamMesh;
	private bool isEnable;

	private bool IsEnable {
		get => isEnable;
		set {
			isEnable = value;
			BeamMesh.enabled = isEnable;
		}
	}

	private new void Start() {
		base.Start();
		CaptureMode = GetComponent<CaptureMode>();
		BeamMesh = GetComponent<MeshRenderer>();
	}

	private new void Update() { base.Update(); }

	protected override int GetValue() { return value; }

	protected override void PreAnimate() {
		IsEnable = CaptureMode.workMode == 2 ||
							CaptureMode.workMode == 3;
	}

	public override void Animate(int value) {
		if (!IsEnable) return;
		var newPosition = transform.localPosition;
		var step = 1f / 4;
		newPosition.x = -0.5f + step / 2f + value * step;
		transform.localPosition = newPosition;
	}
}