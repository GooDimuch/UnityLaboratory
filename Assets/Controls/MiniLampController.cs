using UnityEngine;

public class MiniLampController : MonoBehaviour {
	public enum LightColor {
		Yellow,
		White,
		Green,
		Red
	}

	public LightColor lightColor;
	private Light _light;
	private State<int> _state;

	private void Start() {
		_state = gameObject.GetComponent<State<int>>();
		switchLightColor(lightColor);
		switchMaterial(lightColor);
	}

	private void switchLightColor(LightColor color) {
		_light = transform.parent.GetComponentInChildren<Light>();

		switch (color) {
			case LightColor.White:
				_light.color = Color.white;
				break;

			case LightColor.Green:
				_light.color = Color.green;
				break;

			case LightColor.Red:
				_light.color = Color.red;
				break;

			case LightColor.Yellow:
				_light.color = Color.yellow;
				break;

			default:
				print("Color not found");
				break;
		}
	}

	private void switchMaterial(LightColor color) {
		var mesh = GetComponent<MeshRenderer>();
		if (mesh.materials.Length < 2) { return; }

		switch (color) {
			case LightColor.White:
				mesh.materials[1].color = new Color(0.7509804f, 0.7509804f, 0.7509804f);
				break;

			case LightColor.Green:
				mesh.materials[1].color = new Color(0, 0.2509804f, 0);
				break;

			case LightColor.Red:
				mesh.materials[1].color = new Color(0.2509804f, 0, 0);
				break;

			case LightColor.Yellow:
				mesh.materials[1].color = new Color(0.2509804f, 0.2509804f, 0);
				break;

			default:
				print("Color not found");
				break;
		}
	}

	private void Update() { _light.enabled = _state.CurrentState == 1; }
}