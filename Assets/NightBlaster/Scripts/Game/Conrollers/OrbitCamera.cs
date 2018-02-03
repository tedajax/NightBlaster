using System;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
	public Transform target;
	public float radius = 1f;		// r
	public float inclination = 0f;  // θ
	public float azimuth = 0f;      // φ

	void Update()
	{
		if (target == null)
		{
			return;
		}

		inclination = Mathf.Clamp(inclination, 0, 90f);

		float inclinationRad = (90f - inclination) * Mathf.Deg2Rad;
		float azimuthRad = azimuth * Mathf.Deg2Rad;

		Vector3 localPosition = new Vector3(
			radius * Mathf.Sin(inclinationRad) * Mathf.Cos(azimuthRad),
			radius * Mathf.Cos(inclinationRad),
			radius * Mathf.Sin(inclinationRad) * Mathf.Sin(azimuthRad));

		transform.position = localPosition + target.position;

		transform.LookAt(target.position);
	}
}