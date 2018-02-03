public enum TweenLoopBehavior
{
    Normal,
    PingPong,
    Clamp,
}

public enum TweenDirection
{
    Forward,
    Backward,
}

public enum TweenFunction
{
    Linear,
    EaseIn,
    EaseOut,
    EaseInOut,
	CircEaseIn,
	CircEaseOut,
	CircEaseInOut,
    Parobolic,
    BounceIn,
    BounceOut,
    BounceInOut,
    SinWave,
    CosWave,
    TriangleWave,
}

public enum TweenMode
{
	Function,
	Curve,
}

[System.Serializable]
public class TweenData
{
    public float duration = 1f;
    public int loopCount = -1;
    public TweenLoopBehavior loopBehavior = TweenLoopBehavior.Normal;
    public TweenFunction function = TweenFunction.Linear;
    public float start = 0f;
    public float end = 1f;
    public bool playOnCreate = true;
	public TweenMode tweenMode = TweenMode.Function;
	public UnityEngine.AnimationCurve curve;
}
