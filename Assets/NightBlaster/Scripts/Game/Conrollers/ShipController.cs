using UnityEngine;
using System;

[Serializable]
public class ShipConfig
{
	public float maxMoveSpeed = 10f;
	public float acceleration = 100f;
	public float moveDecay = 100f;
	public float yawSmooth = 1f;
	public float maxYawVelocity = 100f;

	public float rateOfFire = 0.5f;
	public GameObject bulletPrefab;
}

public class ShipController : MonoBehaviour
{
	public ShipConfig config;

	public Rigidbody rigidBody;
	public Camera controllingCamera;

	public Transform[] gunTransforms;

	private Vector3 velocity = Vector3.zero;

	private float yaw;
	private float yawVelocity;
	private float targetYaw;

	private float fireTimer = 0f;

	void Awake()
	{
	}

	void Update()
	{
		// Movement
		{
			Vector3 movementVector = new Vector3(
				Input.GetAxis("Horizontal"),
				0,
				Input.GetAxis("Vertical")).normalized;
			movementVector = Quaternion.AngleAxis(controllingCamera.transform.rotation.eulerAngles.y, Vector3.up) * movementVector;

			if (movementVector.magnitude > 0)
			{
				Vector3 acceleration = movementVector * config.acceleration * Time.deltaTime;
				velocity += acceleration;

				if (velocity.magnitude > config.maxMoveSpeed)
				{
					velocity = velocity.normalized * config.maxMoveSpeed;
				}
			}
			else if (velocity.magnitude > 0)
			{
				float signX = Mathf.Sign(velocity.x);
				float signZ = Mathf.Sign(velocity.z);
				velocity -= config.moveDecay * Time.deltaTime * velocity.normalized;

				if (Mathf.Sign(velocity.x) != signX || Mathf.Sign(velocity.z) != signZ)
				{
					velocity = Vector3.zero;
				}
			}
		}

		// Rotation
		{
			Ray lookAtRay = controllingCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast(lookAtRay, out hitInfo, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Environment"), QueryTriggerInteraction.Ignore))
			{
				Vector3 delta = hitInfo.point - transform.position;
				delta.y = 0;
				delta.Normalize();
				targetYaw = Mathf.Atan2(delta.x, delta.z) * Mathf.Rad2Deg;
			}
			yaw = Mathf.SmoothDampAngle(yaw, targetYaw, ref yawVelocity, config.yawSmooth, config.maxYawVelocity, Time.deltaTime);

			transform.rotation = Quaternion.AngleAxis(yaw, Vector3.up);
		}

		// Shooting
		{
			if (Input.GetButton("Fire1"))
			{
				fireTimer -= Time.deltaTime;
				if (fireTimer <= 0f)
				{
					fireTimer += config.rateOfFire;
					fireShot();
				}
			}
			else
			{
				fireTimer = 0f;
			}
		}
	}

	private void fireShot()
	{
		for (int i = 0; i < gunTransforms.Length; ++i)
		{
			Transform gun = gunTransforms[i];
			var bulletObj = Instantiate(config.bulletPrefab, gun.position, gun.rotation);
			var bullet = bulletObj.GetComponent<BulletController>();
			bullet.Fire(200f, gun.forward, 1f);
		}
	}

	void FixedUpdate()
	{
		rigidBody.velocity = velocity;
	}

	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 400, 30), velocity.ToString());
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + transform.forward * 25f);
	}
}