using System.Collections.Generic;
using UnityEngine;

public class TweenSystem
{
    List<Tween> tweens;

    public TweenSystem()
    {
        tweens = new List<Tween>(512);
    }

    public Tween CreateTween(TweenData tweenData)
    {
        var tween = new Tween(tweenData);
        tween.tweenSystem = this;
        tweens.Add(tween);
        return tween;
    }

    private float SetTimescale(Tween tween, float timescale)
    {
        bool wasPlaying = tween.IsPlaying;

        tween.internalTimescale = Mathf.Max(timescale, 0f);
        tween.timescale = tween.internalTimescale;

        if (wasPlaying && !tween.IsPlaying) {
            tween.OnStart();
        }
        else if (!wasPlaying && tween.IsPlaying) {
            tween.OnPause();
        }

        return tween.timescale;
    }

	private void advanceTween(Tween tween, float dt, float timescale)
	{
		float dirScalar = (tween.direction == TweenDirection.Forward) ? 1f : -1f;
		float delta = timescale * dt * dirScalar;

		tween.currentTime += delta;

		bool finished = false;

		if (tween.direction == TweenDirection.Forward)
		{
			finished = tween.currentTime >= tween.duration;
		}
		else
		{
			finished = tween.currentTime <= 0f;
		}

		finished &= timescale > 0f;

		if (finished)
		{
			bool overrideLooping = (tween.loopBehavior == TweenLoopBehavior.Clamp);
			bool doOnLoop = false;

			if (tween.loopsRemaining > 0 || overrideLooping)
			{
				--tween.loopsRemaining;

				if (tween.loopsRemaining <= 0 || overrideLooping)
				{
					SetTimescale(tween, 0f);

					switch (tween.loopBehavior)
					{
						case TweenLoopBehavior.Normal:
						case TweenLoopBehavior.PingPong:
							tween.currentTime = tween.StartTime;
							break;

						case TweenLoopBehavior.Clamp:
							tween.currentTime = tween.EndTime;
							break;
					}

					tween.OnFinish();
					return;
				}
				else
				{
					doOnLoop = true;
				}
			}
			else
			{
				doOnLoop = true;
			}

			tween.currentTime -= tween.duration * dirScalar;

			if (doOnLoop)
			{
				tween.OnLoop();
			}
		}
	}

	public void AdvanceTween(Tween tween, float dt)
	{
		advanceTween(tween, dt, 1f);
	}

    public void Update(float dt)
    {
        // Update internal timescales of tweens and call Play/Pause events
        foreach (var tween in tweens) {
            if (!Mathf.Approximately(tween.timescale, tween.internalTimescale)) {
                SetTimescale(tween, tween.timescale);
            }
        }

        // Tick tweens
        foreach (var tween in tweens) {
			advanceTween(tween, dt, tween.internalTimescale);
        }
    }
}
