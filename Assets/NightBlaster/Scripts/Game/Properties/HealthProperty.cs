using System;
using UnityEngine;

public class HealthProperty
{
	public float MaxHealth { get; private set; }
	public float CurrentHealth { get; private set; }

	public Action OnDeath;

	public HealthProperty(float maxHealth)
	{
		MaxHealth = maxHealth;
		CurrentHealth = MaxHealth;
	}

	public void Damage(float amount)
	{
		CurrentHealth -= amount;
		if (CurrentHealth <= 0f)
		{
			CurrentHealth = 0f;
			if (OnDeath != null)
			{
				OnDeath();
			}
		}
	}
}

public interface ILivingEntity
{
	HealthProperty Health { get; }
	Rigidbody RigidBody { get; }
}