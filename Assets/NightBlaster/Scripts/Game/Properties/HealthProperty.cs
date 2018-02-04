using System;
using UnityEngine;

public class HealthProperty
{
	public float MaxHealth { get; private set; }
	public float CurrentHealth { get; private set; }
	public float DamageLockoutTime { get; private set; }

	public bool IsDead { get { return CurrentHealth <= 0; } }

	public Action OnDeath;
	public Action OnDamage;

	private float damageLockoutTimer = 0f;

	public HealthProperty(float maxHealth, float damageLockoutTime)
	{
		MaxHealth = maxHealth;
		CurrentHealth = MaxHealth;
		DamageLockoutTime = damageLockoutTime;
	}

	public void Update(float dt)
	{
		if (damageLockoutTimer > 0f)
		{
			damageLockoutTimer -= dt;
		}
	}

	public void Damage(float amount)
	{
		if (damageLockoutTimer <= 0f)
		{
			damageLockoutTimer = DamageLockoutTime;
			CurrentHealth -= amount;

			OnDamage();

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
}

public interface ILivingEntity
{
	HealthProperty Health { get; }
	Rigidbody RigidBody { get; }
}