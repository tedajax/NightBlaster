using System;
using UnityEngine;

public static class GizmoUtil
{
	public static void DrawCircle(Vector2 position, float radius, Color color, int segments = 11)
	{
		Color lastColor = Gizmos.color;
		Gizmos.color = color;

		float delta = 360f / segments * Mathf.Deg2Rad;

		for (int i = 0; i < segments; ++i)
		{
			float a1 = delta * i;
			float a2 = delta * (i + 1);

			Vector2 p1 = new Vector2(Mathf.Cos(a1) * radius + position.x, Mathf.Sin(a1) * radius + position.y);
			Vector2 p2 = new Vector2(Mathf.Cos(a2) * radius + position.x, Mathf.Sin(a2) * radius + position.y);

			Gizmos.DrawLine(p1, p2);
		}

		Gizmos.color = lastColor;
	}

	public static void DrawRect(Vector2 center, Vector2 size, Color color)
	{
		Color lastColor = Gizmos.color;
		Gizmos.color = color;

		float hw = size.x / 2f;
		float hh = size.y / 2f;

		Vector2 tl = new Vector2(center.x - hw, center.y + hh);
		Vector2 tr = new Vector2(center.x + hw, center.y + hh);
		Vector2 bl = new Vector2(center.x - hw, center.y - hh);
		Vector2 br = new Vector2(center.x + hw, center.y - hh);

		Gizmos.DrawLine(tl, tr);
		Gizmos.DrawLine(tr, br);
		Gizmos.DrawLine(tl, bl);
		Gizmos.DrawLine(bl, br);

		Gizmos.color = lastColor;
	}
}