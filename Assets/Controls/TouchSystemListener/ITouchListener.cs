using UnityEngine;


public interface ITouchListener {

	void OnTouchStart(Vector2 position);

	void OnTouchDrug(Vector2 position); //activate only after OnTouchStart

	void OnTouchEnd(Vector2 position); //activate only after OnTouchStart

}
