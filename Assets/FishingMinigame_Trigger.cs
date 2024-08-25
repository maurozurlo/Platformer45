using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMinigame_Trigger : MonoBehaviour
{
	bool isPlaying;
	FishingMinigame fishingMinigame;

	private void Start()
	{
		fishingMinigame = GetComponent<FishingMinigame>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			fishingMinigame.enabled = true;
		}
	}

	public void QuitGame()
	{
		fishingMinigame.enabled = false;
	}




}
