using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSwitch : MonoBehaviour
{
	[Header("Drop scene instances here")]
	public List<GameObject> platformPrefabs; // Scene objects to clone from

	private List<GameObject> currentPlatforms = new List<GameObject>();
	private bool playerIsInRange;

	[Header("Optional: Manual Switch")]
	public bool isManualSwitch = false;
	private GeneralMessageUI messageUI;
	private string msg;

	private void Start()
	{
		if (isManualSwitch)
		{
			messageUI = GameObject.FindGameObjectWithTag("Player").GetComponent<GeneralMessageUI>();
			msg = I18nManager.control.GetValue("ui_platform_switch", "Presionar T para mover plataforma");
		}

	}

	private void Update()
	{
		if (isManualSwitch && playerIsInRange && Input.GetKeyDown(KeyCode.T))
		{
			ActivatePlatforms();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player") && !other.CompareTag("PlayerSkull")) return;

		playerIsInRange = true;
		SpawnPlatforms();


		if (isManualSwitch)
		{
			messageUI.DisplayMessage(msg, 0);
		}
		else
		{
			StartCoroutine(nameof(StartActivation));
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag("Player") && !other.CompareTag("PlayerSkull")) return;

		playerIsInRange = false;

		if (isManualSwitch)
		{
			messageUI.HideMessageImmediatly();
		}
	}

	private void SpawnPlatforms()
	{
		// Destroy any previously spawned platforms
		foreach (var old in currentPlatforms)
		{
			if (old != null) Destroy(old);
		}
		currentPlatforms.Clear();

		// Instantiate new copies at the prefab's original position/rotation
		foreach (GameObject prefab in platformPrefabs)
		{
			GameObject clone = Instantiate(prefab);
			clone.transform.SetPositionAndRotation(prefab.transform.position, prefab.transform.rotation);

			currentPlatforms.Add(clone);
			clone.SetActive(true);
			prefab.SetActive(false);
		}

	}


	void ActivatePlatforms()
	{
		foreach (GameObject go in currentPlatforms)
		{
			if (go.TryGetComponent(out PingPongPlatform platform))
			{
				platform.MovePlatform();
			}
		}
	}

	IEnumerator StartActivation()
	{
		yield return null; // espera al próximo frame, suficiente para que Start() se ejecute
		ActivatePlatforms();
	}

}
