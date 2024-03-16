using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
	public string id;
	public string fallback;
	Text thisText;

	private void Start()
	{
		thisText = GetComponent<Text>();
		UpdateText();
	}

	public void UpdateText()
	{
		thisText.text = I18nManager.control.GetValue(id, fallback);
	}
}
