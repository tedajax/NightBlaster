using System;
using UnityEngine;

public class AIDirectorController : MonoBehaviour
{
	public GameObject enemyPrefab;

	public int waveCount;
	public float waveInterval;
	public int enemiesPerWave;
	public float spawnDistance;

	private int wavesRemaining = 0;
	private float intervalTimer = 0f;

	void Awake()
	{
		wavesRemaining = waveCount;
	}

	void Update()
	{
		if (wavesRemaining > 0)
		{
			intervalTimer -= Time.deltaTime;
			if (intervalTimer <= 0f)
			{
				intervalTimer += waveInterval;
				wavesRemaining--;
				spawnWave();
			}
		}
	}

	private void spawnWave()
	{
		for (int i = 0; i < enemiesPerWave; ++i)
		{
			float angle = UnityEngine.Random.Range(0, 360f) * Mathf.Deg2Rad;
			Vector3 position = new Vector3(Mathf.Cos(angle) * spawnDistance, 0f, Mathf.Sin(angle) * spawnDistance);
			Instantiate(enemyPrefab, position, Quaternion.identity);
		}
	}
}