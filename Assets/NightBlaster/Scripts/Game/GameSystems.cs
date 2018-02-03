using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class GameConfigData
{
	public GameObject uiCanvasPrefab;
}

public class GameSystems : MonoBehaviour
{
	public static GameSystems Instance { get; private set; }

	public GameConfigData Config;


	public TweenSystem tweens { get; private set; }

	public Canvas uiCanvas { get; private set; }

	public HudController hudController { get; private set; }

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		uiCanvas = FindObjectOfType<Canvas>();
		if (uiCanvas == null)
		{
			var canvasObj = Instantiate(Config.uiCanvasPrefab);
			uiCanvas = canvasObj.GetComponent<Canvas>();
		}

		hudController = uiCanvas.GetComponent<HudController>();

		DontDestroyOnLoad(uiCanvas.gameObject);

		tweens = new TweenSystem();
	}

	void Update()
	{
		tweens.Update(Time.deltaTime);
	}
}