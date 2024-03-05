using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

public class MessageReader : MonoBehaviour
{
	public string filePath;
	public Message[] messages;
	NpcDialogue dialogue;

	private void Start()
	{
		string jsonText = File.ReadAllText(filePath);
		NpcDialogue npcDialogue = JsonUtility.FromJson<NpcDialogue>(jsonText);
		dialogue = npcDialogue;
		messages = npcDialogue.dialogue;

		foreach (Message message in messages)
		{
			foreach (Option option in message.options)
			{
				Enum.TryParse(option._action, out ActionType parsedAction);
				option.action = parsedAction;
			}
		}
	}

	public NpcDialogue GetDialogue()
	{
		if (dialogue == null)
		{
			Debug.LogError("No dialogue found");
		}
		return dialogue;
	}
}