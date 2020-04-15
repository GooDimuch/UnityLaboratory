public class IntState : State<int> {
	protected override bool Equals(int value, int state) => value == currentState;
}