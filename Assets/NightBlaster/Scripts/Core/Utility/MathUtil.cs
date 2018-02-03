using UnityEngine;
public static class MathUtil
{
	public static bool ApproximatelyZero(float a)
	{
		return Mathf.Approximately(a, 0f);
	}

	public static float SmoothClamp(float v, float min, float max, float decay)
	{
		if (v < min)
		{
			return Mathf.Min(min - v + decay, min);
		}
		else if (v > max)
		{
			return Mathf.Max(max + v - decay, max);
		}
		else
		{
			return v;
		}
	}

	public static float SmoothDampClamp(float v, float min, float max, float damp)
	{
		if (v < min)
		{
			float delta = min - v;
			float r = delta * damp;
			return Mathf.Min(r, min);
		}
		else if (v > max)
		{
			float delta = v - max;
			float r = delta * damp;
			return Mathf.Max(r, max);
		}
		else
		{
			return v;
		}
	}

	public static float Decay(float value, float decayRate, float target = 0f)
	{
		if (value < target)
		{
			value += decayRate;
			if (value > target)
			{
				return target;
			}
		}
		else if (value > target)
		{
			value -= decayRate;
			if (value < target)
			{
				return target;
			}
		}
		return value;
	}


	public static Vector2 Decay(Vector2 value, float decayRate, Vector2 target = default(Vector2))
	{
		return new Vector2(Decay(value.x, decayRate, target.x), Decay(value.y, decayRate, target.y));
	}

	public static Vector2 DirectionFromTransform(Transform transform)
	{
		return DirectionFromAngle(transform.localEulerAngles.z);
	}

	public static Vector2 DirectionFromAngle(float degrees)
	{
		float rads = degrees * Mathf.Deg2Rad;
		return new Vector2(Mathf.Cos(rads), Mathf.Sin(rads));
	}

	public static float AngleTowards(Vector2 from, Vector2 to)
	{
		return Mathf.Atan2(to.y - from.y, to.x - from.x) * Mathf.Rad2Deg;
	}

	public static Vector3 Vector3(Vector2 v2)
	{
		return new Vector3(v2.x, v2.y, 0f);
	}

	public static Vector2 Vector2(Vector3 v3)
	{
		return new Vector2(v3.x, v3.y);
	}

	public static Vector2 ClampVector(Vector2 vec, float mag)
	{
		Vector2 v = vec;
		float m = v.magnitude;
		if (m >= mag)
		{
			float delta = m - mag;
			v -= v.normalized * delta;
		}
		return v;
	}
}