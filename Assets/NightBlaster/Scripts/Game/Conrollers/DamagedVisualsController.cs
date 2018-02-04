using System;
using UnityEngine;

public class DamagedVisualsController : MonoBehaviour
{
	public Color damagedColor;
	public Color deadColor;

	public enum DamageState
	{
		Alive,
		Damaged,
		Dead,
	}

	private DamageState damageState;
	private float colorTimer = 0f;

	public Renderer[] renderers;

	void Update()
	{
		if (colorTimer > 0f)
		{
			colorTimer -= Time.deltaTime;
			if (colorTimer <= 0f)
			{
				damageState = DamageState.Alive;
				SetDamageState(DamageState.Alive);
			}
		}
	}

	private void setMaterialColors(Color color)
	{
		foreach (var renderer in renderers)
		{
			renderer.material.color = color;
		}
	}

	public void SetDamageState(DamageState state, float duration = -1f)
	{
		damageState = state;
		colorTimer = duration;
		setMaterialColors(getDamageStateColor(state));
	}

	private Color getDamageStateColor(DamageState state)
	{
		switch (state)
		{
			default:
			case DamageState.Alive: return Color.white;
			case DamageState.Damaged: return damagedColor;
			case DamageState.Dead: return deadColor;
		}
	}
}