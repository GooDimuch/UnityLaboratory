using System;
using Unity.Collections;
using UnityEngine;

public class CaptureMode : BaseCrewSimToUnity<int> {
	[Range(0, 3)] public int value;

	[ReadOnly] public int workMode;

	private new void Start() {
		base.Start();
	}

	private new void Update() {
		base.Update();
		workMode = (int) debug;
	}

	protected override int GetValue() { return value; }

	protected override void PreAnimate() { }

	public override void Animate(int value) { }
}