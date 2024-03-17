using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSwitch : MonoBehaviour
{
    public PingPongPlatform platformToControl;
	// Start is called before the first frame update
	GeneralMessageUI messageUI;
	bool playerIsInRange;
	private string msg;

	private void Start()
	{
		messageUI = GameObject.FindGameObjectWithTag("Player").GetComponent<GeneralMessageUI>();
		msg = I18nManager.control.GetValue("ui_platform_switch", "Presionar T para mover plataforma");
	}
	private void Update()
	{
		if (playerIsInRange && Input.GetKeyDown(KeyCode.T))
		{
			platformToControl.MovePlatform();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			playerIsInRange = true;
			
			messageUI.DisplayMessage(msg, 0);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			playerIsInRange = false;
			messageUI.HideMessageImmediatly();
		}
	}
}
