using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
	//UI
	public Camera npcCamera;
	public Text npcName;
	public Text npcText;
	public Button option1;
	public Button option2;
	//Current Dialogue
	public NpcDialogue dialogue;
	//GameObjects
	public GameObject NPC;
	public GameObject player;


	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		if (player == null)
			Debug.LogError("Player couldn't be found");
	}



	public void StartDialogue(GameObject npc, int nodeToStart)
	{
		NPC = npc;
		//Primero seteamos la camera
		npcCamera.depth = 2;
		npcCamera.transform.position = NPC.GetComponent<NPCInteraction>().cameraPos;
		Vector3 v3 = new Vector3(0, NPC.transform.rotation.eulerAngles.y - 180, 0);
		Quaternion qt = Quaternion.Euler(v3);
		npcCamera.transform.rotation = qt;

		if (dialogue != null)
		{
			DrawUI(nodeToStart);
		}
	}


	public void DrawUI(int mainNode)
	{
		if (!checkIfNodeExists(mainNode))
		{
			EndChat();
			return;
		}

		npcName.text = dialogue.npcName;
		Message node = GetNode(mainNode);
		npcText.text = node.text;
		// TODO: make this a foreach instead...
		// option buttons should get created on the fly
		SetupOption(option1, node.options[0]);
		SetupOption(option2, node.options[1]);

	}


	bool checkIfNodeExists(int mainNode)
	{
		if (mainNode < 0)
			return false;
		if (GetNode(mainNode) != null)
			return true;
		else
			return false;
	}

	public Message FirstMessage(int? currentQuest, bool questCompleted)
	{
		foreach (Message m in dialogue.dialogue)
		{
			if (m.onQuest == currentQuest && m.questCompleted == questCompleted) return m;
		}
		Debug.LogError("Node not found");
		return null;
	}

	public Message GetNode(int nodeId)
	{
		foreach (Message m in dialogue.dialogue)
		{
			if (m.id == nodeId) return m;
		}
		Debug.LogError("Node not found");
		return null;
	}

	public void SetupOption(Button option, Option values)
	{

		option.gameObject.SetActive(true);
		option.GetComponentInChildren<Text>().text = values.text;
		option.GetComponent<OptionUI>().action = values.action;
		option.GetComponent<OptionUI>().value = values.value;
	}

	public void HandleOption(OptionUI option)
	{
		switch (option.action)
		{
			case ActionType.goToNode:
				DrawUI((int)option.value);
				return;
			case ActionType.endChat:
				EndChat();
				return;
			case ActionType.startQuest:
				player.GetComponent<QuestManager>().StartQuest(option.value);
				EndChat();
				return;
			default:
				Debug.LogError("Unkown action for option");
				return;
		}
	}

	public void EndChat()
	{
		//Exit conversation
		npcCamera.depth = -2;
		NPC.GetComponent<NPCInteraction>().StartCoroutine("EndConversation");
	}
}
