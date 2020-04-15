using System;
using UnityEngine;

public class FloatState : State<float> {
	protected override bool Equals(float value, float state) => Math.Abs(value - currentState) < Mathf.Epsilon;
}