using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OptionUI : MonoBehaviour
{
	public int value;
	public ActionType action;
	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(SendAction);
	}

	void SendAction() {
		DialogueUI.control.HandleOption(this);
	}
	


    
}
