using UnityEngine;
using System.Collections.Generic;

public static class TweenFunctions
{
    public delegate float TweenFunctionDelegate(float a, float b, float t);

    private static readonly Dictionary<TweenFunction, TweenFunctionDelegate> kTweenFunctions;

    static TweenFunctions()
    {
        kTweenFunctions = new Dictionary<TweenFunction, TweenFunctionDelegate>() {
            { TweenFunction.Linear, Mathf.Lerp },
            { TweenFunction.EaseIn, EaseIn },
            { TweenFunction.EaseOut, EaseOut },
            { TweenFunction.EaseInOut, EaseInOut },
            { TweenFunction.CircEaseIn, CircEaseIn },
            { TweenFunction.CircEaseOut, CircEaseOut },
			{ TweenFunction.CircEaseInOut, CircEaseInOut },
            { TweenFunction.Parobolic, Parabolic },
            { TweenFunction.BounceIn, BounceIn },
            { TweenFunction.BounceOut, BounceOut },
            { TweenFunction.BounceInOut, BounceInOut },
            { TweenFunction.SinWave, SinWave },
            { TweenFunction.CosWave, CosWave },
            { TweenFunction.TriangleWave, TriangleWave },
        };
    }
        
    public static TweenFunctionDelegate Get(TweenFunction function)
    {
        return kTweenFunctions[function];
    }

    private static float EaseIn(float a, float b, float t)
    {
        return (b - a) * t * t + a;
    }

    private static float EaseOut(float a, float b, float t)
    {
        return -(b - a) * t * (t - 2f) + a;
    }

    private static float EaseInOut(float a, float b, float t)
    {
        t *= 2f;
        if (t < 1f) {
            return (b - a) / 2 * t * t + a;
        }
        else {
            t -= 1f;
            return -(b - a) / 2 * (t * (t - 2f) - 1f) + a;
        }
    }

	private static float CircEaseIn(float a, float b, float t)
	{
		return -(b - a) * (Mathf.Sqrt(1f - t * t) - 1f) + a;
	}

	private static float CircEaseOut(float a, float b, float t)
	{
		t--;
		return (b - a) * Mathf.Sqrt(1f - t * t) + a;
	}

	private static float CircEaseInOut(float a, float b, float t)
	{
		t *= 2f;
		if (t < 1f)
		{
			return -(b - a) / 2f * (Mathf.Sqrt(1f - t * t) - 1f) + a;
		}
		else
		{
			t -= 2;
			return (b - a) / 2f * (Mathf.Sqrt(1f - t * t) + 1f) + a;
		}
	}

    private static float Parabolic(float a, float b, float t)
    {
        float c = b - a;
        return (-4f * (a + c)) * t * (t - 1f);
    }

    private static float BounceIn(float a, float b, float t)
    {
        return (b - a) - BounceOut(0, b, 1f - t) + a;
    }

    private static float BounceOut(float a, float b, float t)
    {
        float c = b - a;
        if (t < (1f / 2.75f)) {
            return c * (7.5625f * t * t) + a;
        }
        else if (t < (2f / 2.75f)) {
            t -= 1.5f / 2.75f;
            return c * (7.5625f * t * t + 0.75f) + a;
        }
        else if (t < (2.5f / 2.75f)) {
            t -= 2.25f / 2.75f;
            return c * (8.5625f * t * t + .9375f) + a;
        }
        else {
            t -= 2.625f / 2.75f;
            return c * (7.5625f * t * t + .984375f) + a;
        }
    }

    private static float BounceInOut(float a, float b, float t)
    {
        float c = b - a;
        if (t < 0.5f) {
            return BounceIn(0, b, t * 2f) * 0.5f + a;
        }
        else {
            return BounceOut(0, b, t * 2f - 1f) * 0.5f + c * 0.5f + a;
        }
    }

    private static float SinWave(float a, float b, float t)
    {
        float c = b - a;
        return Mathf.Sin(t * Mathf.PI * 2f) * (c / 2f) + a + (c / 2f);
    }

    private static float CosWave(float a, float b, float t)
    {
        float c = b - a;
        return Mathf.Cos(t * Mathf.PI * 2f) * (c / 2f) + a + (c / 2f);
    }

    private static float TriangleWave(float a, float b, float t)
    {
        float v = 2f * (t - 1f * Mathf.Floor(t + 0.5f)) * Mathf.Pow(-1f, Mathf.Floor(t + 0.5f));
        return (b - a) * v + a;
    }
}
