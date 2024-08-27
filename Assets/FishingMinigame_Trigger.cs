using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMinigame_Trigger : MonoBehaviour
{
	bool isPlaying;
	public GameObject spawnPoint; // TODO will be the point the player spawns to after minigame
	public GameObject playerSpawnPoint;
	public GameObject cameraPosition;
	public List<GameObject> prizes = new List<GameObject>(); // TODO: list of possible prizes, will need to implement some sort of weighted table

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !isPlaying)
		{
			isPlaying = true;
			FishingMinigame.control.StartGame(spawnPoint.transform.position, playerSpawnPoint, cameraPosition);
			// TODO: Add prizes
			// TODO: Add spawnPoint
		}
	}

	public void StopPlaying()
	{
		isPlaying = false;
	}

}
