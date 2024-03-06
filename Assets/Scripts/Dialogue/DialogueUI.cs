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
	
	//Current Dialogue
	public NpcDialogue npcDialogue;
	public Node[] dialogueNodes;
	//GameObjects
	public GameObject NPC;
	public GameObject player;
	// Singleton
	public static DialogueUI control;

	// Options
	public GameObject optionContainer;
	public GameObject optionPrefab;
	public GameObject[] options;


	private void Awake()
	{
		if (!control)
		{
			control = this;
		}
		else
		{
			DestroyImmediate(this);
		}
	}
	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		if (player == null)
			Debug.LogError("Player couldn't be found");
	}



	public void StartDialogue(GameObject npc)
	{
		NPC = npc;
		//Primero seteamos la camera
		npcCamera.depth = 2;
		npcCamera.transform.position = NPC.GetComponent<NPCInteraction>().cameraPos;
		Vector3 v3 = new Vector3(0, NPC.transform.rotation.eulerAngles.y - 180, 0);
		Quaternion qt = Quaternion.Euler(v3);
		npcCamera.transform.rotation = qt;

		// Conseguimos el id del dialogue a mostrar
		string dialogToStart = QuestManager.control.GetDialogueId(npcDialogue.quests);
		dialogueNodes = GetNodesForDialog(dialogToStart);
		if (dialogueNodes != null)
		{
			DrawUI(0);
		}
	}

	Node[] GetNodesForDialog(string dialogToStart)
	{
		foreach (Dialogue dialogueItem in npcDialogue.dialogue)
		{
			if (dialogueItem.id == dialogToStart) return dialogueItem.nodes;
		}
		Debug.LogError($"Dialog id {dialogToStart} not found");
		return null;
	}


	public void DrawUI(int mainNode)
	{
		if (!checkIfNodeExists(mainNode))
		{
			EndChat();
			return;
		}

		npcName.text = npcDialogue.npcName;
		Node node = GetNode(mainNode);
		npcText.text = node.text;

		// Remove all children
		foreach (Transform child in optionContainer.transform)
		{
			Destroy(child.gameObject);
		}
		// Create options
		foreach (Option option in node.options) {
			GameObject op = Instantiate(optionPrefab, optionContainer.transform);
			SetupOption(op.GetComponent<Button>(), option);
		}
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

	public Node GetNode(int nodeId)
	{
		foreach (Node m in dialogueNodes)
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
				QuestManager.control.StartQuest(option.value);
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

	public enum DialogType {
		common_intro,
		common_completed_all,
		common_ongoing,
		value_ongoing,
		value_completed_some,		
	}

	public static string GetDialogId(DialogType type, int value = -1)
	{
		switch (type)
		{
			case DialogType.common_intro:
				return "common_intro";
			case DialogType.common_ongoing:
				return $"common_quest_ongoing";
			case DialogType.common_completed_all:
				return "common_completed_all";
			case DialogType.value_ongoing:
				return $"quest_{value}_ongoing";
			case DialogType.value_completed_some:
				return $"quest_{value}_completed_some";

			default:
				Debug.LogError("Unsupported dialog type");
				return "common_intro";
		}
	}

	public bool DialogExists(string id)
	{
		foreach (Dialogue dialogueItem in npcDialogue.dialogue)
		{
			if (dialogueItem.id == id) return true;
		}
		return false;
	}
}
