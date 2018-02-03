using UnityEngine;

public delegate void TweenEventHandler(Tween tween);

public class Tween
{
    public TweenData data;

    public float timescale;
    public float currentTime;
    public TweenDirection direction;
    public int loopsRemaining;

    // true timescale, all internal timescales get updated to whatever _timescale is set to once per frame by
    // the tween system.
    // WARNING: DO NOT SET DIRECTLY YOU WILL HAVE A BAD DAY
    public float internalTimescale;

	public float duration { get { return data.duration; } }
    public int loopCount { get { return data.loopCount; } }
    public TweenLoopBehavior loopBehavior { get { return data.loopBehavior; } }
    public TweenFunction function { get { return data.function; } }
    public float start { get { return data.start; } }
    public float end { get { return data.end; } }

    public event TweenEventHandler onStart;
    public event TweenEventHandler onPause;
    public event TweenEventHandler onLoop;
    public event TweenEventHandler onFinish;

    public TweenSystem tweenSystem;

    public float NormalizedTime
    {
        get
        {
            switch (loopBehavior) {
                default:
                case TweenLoopBehavior.Normal:
                    return currentTime / duration;

                case TweenLoopBehavior.Clamp:
                    return Mathf.Clamp01(currentTime / duration);

                case TweenLoopBehavior.PingPong:
                    if (currentTime < duration / 2f) {
                        return currentTime / (duration / 2f);
                    }
                    else {
                        return 1f - ((2f * currentTime - duration) / duration);
                    }
            }
        }
    }

    public float StartTime
    {
        get
        {
            switch (direction) {
                default:
                case TweenDirection.Forward:
                    return 0f;

                case TweenDirection.Backward:
                    return duration;
            }
        }
    }

    public float EndTime
    {
        get
        {
            switch (direction) {
                default:
                case TweenDirection.Forward:
                    return duration;

                case TweenDirection.Backward:
                    return 0f;
            }
        }
    }

    public bool IsPlaying
    {
        get
        {
            return !Mathf.Approximately(internalTimescale, 0f);
        }
    }

    public void Reset(float timescale = 1f, TweenDirection direction = TweenDirection.Forward)
    {
        this.timescale = timescale;
        this.direction = direction;
        this.loopsRemaining = loopCount;
        this.currentTime = StartTime;
    }

	public void Advance(float time)
	{
		tweenSystem.AdvanceTween(this, time);
	}

	public float Evaluate()
	{
		switch (data.tweenMode)
		{
			default:
			case TweenMode.Function:
				return TweenFunctions.Get(function)(start, end, NormalizedTime);

			case TweenMode.Curve:
				return (data.curve != null) ? data.curve.Evaluate(NormalizedTime) : 0f;
		}
    }

    // Because C# doesn't let you call events from outside of the owning class this is the only way
    // for the TweenSystem to call these events.
    #region Event calling functions
    public void OnStart()
    {
        if (onStart != null) {
            onStart(this);
        }
    }

    public void OnPause()
    {
        if (onPause != null) {
            onPause(this);
        }
    }

    public void OnLoop()
    {
        if (onLoop != null) {
            onLoop(this);
        }
    }

    public void OnFinish()
    {
        if (onFinish != null) {
            onFinish(this);
        }
    }
    #endregion

    public Tween(TweenData data)
    {
        this.data = data;

        timescale = (data.playOnCreate) ? 1f : 0f;
        internalTimescale = timescale;
    }
}
