using System;
using UnityEngine;

[Serializable]
public class EnemyZombieData
{
	public float chaseSpeed;
	public float acceleration;
	public float awarenessRadius;
	public float turnSmoothFactor;
	public float maxHealth = 5f;
}

public class EnemyZombieController : MonoBehaviour, ILivingEntity
{
	public enum State
	{
		Idle,
		Chase,
		Dead,
	}

	public EnemyZombieData config;
	public Rigidbody rigidBody;
	public PositionTweener walkTweener;
	public DamagedVisualsController damagedVisuals;

	private State state;
	private float speed;
	private Vector3 velocity = Vector3.zero;
	private float angularVelocity;
	private Transform target;

	private HealthProperty health;

	public HealthProperty Health { get { return health; } }
	public Rigidbody RigidBody { get { return rigidBody; } }

	void Awake()
	{
		state = State.Idle;
		health = new HealthProperty(config.maxHealth, 0.2f);
		health.OnDamage += onDamage;
		health.OnDeath += onDeath;
	}

	void OnDestroy()
	{
		health.OnDeath -= onDeath;
	}

	void Update()
	{
		health.Update(Time.deltaTime);

		switch (state)
		{
			case State.Idle: idle(); break;
			case State.Chase: chase(); break;
			case State.Dead: break;
		}
	}

	void FixedUpdate()
	{
		if (state != State.Dead)
		{
			rigidBody.velocity = velocity;
		}
	}

	private void onDamage()
	{
		damagedVisuals.SetDamageState(DamagedVisualsController.DamageState.Damaged, health.DamageLockoutTime);
	}

	private void onDeath()
	{
		state = State.Dead;
		rigidBody.constraints = RigidbodyConstraints.None;
		rigidBody.useGravity = true;

		rigidBody.AddTorque(Vector3.right * 20f, ForceMode.Impulse);

		damagedVisuals.SetDamageState(DamagedVisualsController.DamageState.Dead, -1f);

		if (walkTweener != null)
		{
			walkTweener.Tween.Reset(0f);
		}
	}

	private void idle()
	{
		if (target == null)
		{
			target = searchForTarget(config.awarenessRadius);

			if (target != null)
			{
				state = State.Chase;
			}
		}
	}

	private void chase()
	{
		// Turn towards target
		Vector3 delta = target.position - transform.position;
		delta.y = 0f;
		Vector3 direction = delta.normalized;

		float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

		float angle = transform.rotation.eulerAngles.y;
		angle = Mathf.SmoothDampAngle(angle, targetAngle, ref angularVelocity, config.turnSmoothFactor * Time.deltaTime);
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

		float rigidBodySpeed = rigidBody.velocity.magnitude;
		if (rigidBodySpeed < speed)
		{
			speed = rigidBodySpeed;
		}

		speed += config.acceleration * Time.deltaTime;
		speed = Mathf.Clamp(speed, 0f, config.chaseSpeed);
		velocity = transform.forward * speed;

		if (walkTweener != null)
		{
			walkTweener.Tween.Advance(Time.deltaTime * (speed / config.chaseSpeed));
		}
	}

	private Transform searchForTarget(float radius)
	{
		Transform result = null;
		Collider[] colliders = Physics.OverlapSphere(transform.position, radius, 1 << LayerMask.NameToLayer("Player"), QueryTriggerInteraction.Ignore);
		
		if (colliders.Length > 0)
		{
			result = colliders[0].attachedRigidbody.GetComponent<ShipController>().transform;
		}

		return result;
	}


	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10f);
	}
}