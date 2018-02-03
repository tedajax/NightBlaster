using UnityEngine;

public class RotationTweener : MonoBehaviour
{
	public TweenData tweenData = new TweenData();

	public Vector3 axis;
	public float angle;

	private Quaternion baseRotation;
	private Tween tween;

	void Start()
	{
		baseRotation = transform.localRotation;
		tween = GameSystems.Instance.tweens.CreateTween(tweenData);
	}

	void Update()
	{
		float eval = tween.Evaluate();

		Quaternion rotation = baseRotation * Quaternion.AngleAxis(angle * eval, axis);
		transform.localRotation = rotation;
	}
}