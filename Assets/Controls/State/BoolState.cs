public class BoolState : State<bool> {
	protected override bool Equals(bool value, bool state) => value == currentState;
}