using System;
using UnityEngine;

public class DestroyAfterLifetime : MonoBehaviour
{
	public float lifetime = 0;

	void Update()
	{
		if (lifetime > 0)
		{
			lifetime -= Time.deltaTime;
			if (lifetime <= 0)
			{
				var destructables = gameObject.GetComponents<IDestructable>();
				foreach (var destructable in destructables)
				{
					destructable.Destruct();
				}
				Destroy(gameObject);
			}
		}
	}
}
