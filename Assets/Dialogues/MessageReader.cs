using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

public class MessageReader : MonoBehaviour
{
	public string filePath;
	NpcDialogue npcDialogue;

	private void Awake()
	{
		string jsonText = File.ReadAllText(filePath);
		NpcDialogue parsedNPCDialogue = JsonUtility.FromJson<NpcDialogue>(jsonText);
		npcDialogue = parsedNPCDialogue;

		foreach (Dialogue dialogueItem in npcDialogue.dialogue)
		{
			foreach (Node node in dialogueItem.nodes)
			{
				foreach (Option option in node.options)
				{
					Enum.TryParse(option._action, out ActionType parsedAction);
					option.action = parsedAction;
				}
			}
		}
		
	}

	public NpcDialogue GetDialogue()
	{
		if (npcDialogue == null)
		{
			Debug.LogError("No dialogue found");
		}
		return npcDialogue;
	}
}