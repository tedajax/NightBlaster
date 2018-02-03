using System;
using UnityEngine;

public class PositionTweener : MonoBehaviour
{
	public TweenData tweenData = new TweenData();

	public Vector3 normal;
	public float magnitude;

	private Vector3 basePosition;
	private Tween tween;

	public Tween Tween { get { return tween; } }

	void Start()
	{
		basePosition = transform.localPosition;
		tween = GameSystems.Instance.tweens.CreateTween(tweenData);
	}

	void Update()
	{
		float eval = tween.Evaluate();

		Vector3 position = basePosition + normal.normalized * magnitude * eval;
		transform.localPosition = position;
	}
}