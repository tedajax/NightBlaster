using System;
using UnityEngine;


public class BulletController : MonoBehaviour, IDestructable
{
	public GameObject impactPrefab;

	public float explosionRadius = 5f;
	public float explosionForce = 50f;

	private float speed = 0f;
	private Vector3 forward;
	private float damage = 0f;

	bool isFired = false;

	void Awake()
	{

	}

	void Update()
	{
		if (!isFired)
		{
			return;
		}

		Vector3 velocity = forward * speed * Time.deltaTime;
		transform.position += velocity;
	}

	void OnCollisionEnter(Collision collision)
	{
		Destruct();
	}

	public void Fire(float speed, Vector3 forward, float damage)
	{
		this.speed = speed;
		this.forward = forward;
		this.damage = damage;
		isFired = true;
	}

	public void Destruct()
	{
		Explode();

		if (impactPrefab != null)
		{
			Instantiate(impactPrefab, transform.position, transform.rotation);
		}

		Destroy(gameObject);
	}

	public void Explode()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
	
		for (int i = 0; i < colliders.Length; ++i)
		{
			if (colliders[i].attachedRigidbody == null)
			{
				continue;
			}

			var entity = colliders[i].attachedRigidbody.GetComponent<ILivingEntity>();

			if (entity != null)
			{
				Vector3 delta = entity.RigidBody.transform.position - transform.position;
				delta.y = 2f;
				float magnitude = delta.magnitude;
				Vector3 direction = delta.normalized;

				entity.Health.Damage(Mathf.Lerp(damage, 0f, magnitude / explosionRadius));

				if (entity.RigidBody != null)
				{
					Vector3 force = direction * Mathf.Lerp(explosionForce, 0f, magnitude / explosionRadius);
					entity.RigidBody.AddForce(force, ForceMode.Impulse);
				}
			}
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 1f, 0f, 0.25f);
		Gizmos.DrawSphere(transform.position, explosionRadius);
	}
}