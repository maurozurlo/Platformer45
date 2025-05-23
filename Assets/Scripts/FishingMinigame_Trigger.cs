﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMinigame_Trigger : MonoBehaviour
{
	bool isPlaying;
	public GameObject spawnPoint; // TODO will be the point the player spawns to after minigame
	public GameObject playerSpawnPoint;
	public GameObject cameraPosition;
	public List<BasicItem> prizes = new List<BasicItem>(); // TODO: list of possible prizes, will need to implement some sort of weighted table
	MeshRenderer[] mrs;

	private void Start()
	{
		mrs = GetComponentsInChildren<MeshRenderer>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !isPlaying)
		{
			isPlaying = true;
			foreach (MeshRenderer mr in mrs)
			{
				mr.enabled = false;
			}
			
			// TODO: Add prizes weighted table
			BasicItem item = prizes[Random.Range(0, prizes.Count)];
			FishingMinigame.control.StartGame(spawnPoint.transform.position, playerSpawnPoint, cameraPosition, item);
		}
	}

	public void StopPlaying()
	{
		isPlaying = false;
		foreach (MeshRenderer mr in mrs)
		{
			mr.enabled = true;
		}
	}

}
