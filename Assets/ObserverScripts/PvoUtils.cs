using UnityEngine;

public static class PvoUtils {
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
		return new Vector3(polar.x * Mathf.Sin(polar.z * Mathf.Deg2Rad) * Mathf.Cos(polar.y * Mathf.Deg2Rad),
				polar.x * Mathf.Cos(polar.z * Mathf.Deg2Rad),
				polar.x * Mathf.Sin(polar.z * Mathf.Deg2Rad) * Mathf.Sin(polar.y * Mathf.Deg2Rad));
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

	/// <summary>
	/// Пробует расчитать координаты точки упреждения и вектор направления ракеты.
	/// Может использоваться для покадрового пересчета направления ракеты.
	/// </summary>
	/// <param name="targetPosition">Текущая позиция цели.</param>
	/// <param name="targetVelocity">Текущий вектор скорости цели,
	///																где длина равна линейной скорости цели.</param>
	/// <param name="rocketSpeed">Линейная скорость полета ракеты.</param>
	/// <param name="contactPosition">Вектор позиции точки упреждения.</param>
	/// <param name="rocketVelocity">Вектор направления полета ракеты.</param>
	/// <param name="maxLength">Максимальное расстояние для поражения цели.
	///														По умолчанию 10000</param>
	/// <returns>Возвращает true, если удалось расчитать точку упреждения.</returns>
	public static bool TryCalculateLeadPoint(Vector3 targetPosition,
			Vector3 targetVelocity,
			float rocketSpeed,
			out Vector3 contactPosition,
			out Vector3 rocketVelocity,
			float maxLength = 10000) {
		rocketVelocity = Vector3.zero;
		contactPosition = Vector3.zero;
		var vt = targetVelocity.magnitude;
		var r = targetPosition.magnitude;

		if (Mathf.Abs(vt) < ZERO_ACCURACY) { contactPosition = targetPosition; } else {
			var k = rocketSpeed / vt;
			var cosFi = Mathf.Cos(AngleUtils.Deg2Rad(Vector3.Angle(-targetPosition, targetVelocity)));
			var a = k * k - 1;
			var b = 2 * r * cosFi;
			var c = -r * r;
			float x1;
			float x2;

			if (!QuadraticRoots(a, b, c, out x1, out x2)) {
				Debug.LogError("can't calculated lead point");
				return false;
			}
			var x = ChooseValidX(x1, x2);
			var t = x * k / rocketSpeed;
			contactPosition = targetPosition + targetVelocity * t;
			if (contactPosition.magnitude > maxLength) {
				Debug.LogError("lead point too far");
				return false;
			}
		}
		rocketVelocity = contactPosition.normalized * rocketSpeed;
		return true;
	}

	private static bool QuadraticRoots(float a, float b, float c, out float x1, out float x2) {
		x1 = x2 = float.NaN;
		var D = b * b - 4 * a * c;

		if (Mathf.Abs(D) < ZERO_ACCURACY) { x1 = x2 = -b / (2 * a); } else if (D > 0) {
			x1 = (-b + Mathf.Sqrt(D)) / (2 * a);
			x2 = (-b - Mathf.Sqrt(D)) / (2 * a);
		} else { return false; }
		return true;
	}

	private static float ChooseValidX(float x1, float x2) {
		if (Mathf.Max(x1, x2) < 0) return float.NaN;
		if (x1 > 0 && x2 > 0) return Mathf.Min(x1, x2);
		return x1 > 0 ? x1 : x2;
	}
}

public static class AngleUtils {
	/// <summary>
	/// Нормализирует угол к [0..360].
	/// </summary>
	/// <param name="angle">Угол в градусах.</param>
	/// <returns>Возвращает угол от 0 до 360 градусов.</returns>
	public static float NormalizeAngle(float angle) {
		while (angle >= 360) angle -= 360f;
		while (angle < 0) angle += 360f;
		return angle;
	}

	/// <summary>
	/// Нормализирует угол к [-180..0..180]. При чем 0 остается нулем.
	/// </summary>
	/// <param name="angle">Угол в градусах.</param>
	/// <returns>Возвращает угол от -180 до 180 градусов.</returns>
	public static float NormalizeAngle180(float angle) {
		var result = NormalizeAngle(angle);
		if (result >= 180) { result -= 360f; }
		return result;
	}

	/// <summary>
	/// Возвращает угол между двумя векторами. Где '-' указывает направление.
	/// </summary>
	/// <param name="angle1">Угол 1-го вектора в градусах.</param>
	/// <param name="angle2">Угол 2-го вектора в градусах.</param>
	/// <returns>Возвращает угол в градусах.</returns>
	public static float GetRangeBetweenAngles(float angle1, float angle2) =>
			180 - (NormalizeAngle(angle1) - NormalizeAngle(angle2) + 360 + 180) % 360;

	/// <summary>
	/// Возвращает положительный угол между двумя векторами.
	/// </summary>
	/// <param name="angle1">Угол 1-го вектора в градусах.</param>
	/// <param name="angle2">Угол 2-го вектора в градусах.</param>
	/// <returns>Возвращает угол в градусах по модулю.</returns>
	public static float GetRangeBetweenAnglesAbs(float angle1, float angle2) =>
			Mathf.Abs(GetRangeBetweenAngles(angle1, angle2));

	/// <summary>
	/// Переводит градусы в гадианы.
	/// </summary>
	/// <param name="angle">Угол в градусах.</param>
	/// <returns>Возвращает угол в радианах.</returns>
	public static float Deg2Rad(float angle) => angle * Mathf.Deg2Rad;

	/// <summary>
	/// Переводит гадианы в градусы. Возможна нормализация значения.
	/// </summary>
	/// <param name="angle">Угол в радианах.</param>
	/// <param name="isNormalize">true: нормализовать значение.</param>
	/// <returns>Возвращает угол в градусах.</returns>
	public static float Rad2Deg(float angle, bool isNormalize = false) =>
			isNormalize ? NormalizeAngle(angle * Mathf.Rad2Deg) : angle * Mathf.Rad2Deg;
}