using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	[HideInInspector]
	public Animator anim;

	[HideInInspector]
	public enum STATES { IDLE, SNEAK, WALK, RUN, JUMPING, CHAT, ATTACKING, LOCKED, DEAD };


	// set per default any character as idle
	public STATES state = STATES.IDLE;

	public string label = "Default Character";

	public float health = 100.0f;
	public float stamina = 100.0f;

	// is the character a player ?
	public bool isPlayer = false;

	public GameObject sfx;

	// Start is called before the first frame update
	void Awake()
	{
		if (GetComponent<Animator>() != null)
		{
			anim = GetComponent<Animator>();
		}
	}

	public STATES GetMoveState()
	{
		float input = anim.GetFloat("InputVertical");
		if (input <= 0f) return STATES.IDLE;
		if (input <= 0.50f) return STATES.SNEAK;
		if (input <= 1f) return STATES.WALK;
		return STATES.RUN;

	}

	public bool IsDead()
	{
		return health <= 0.0f || state == STATES.DEAD;
	}

	public bool isLocked()
	{
		return state == STATES.LOCKED;
	}

	public void Lock()
	{
		if (state == STATES.LOCKED) return;
		state = STATES.LOCKED;

		//disable user inputs
		Invector.CharacterController.vThirdPersonInput ccI = this.GetComponent<Invector.CharacterController.vThirdPersonInput>();
		ccI.enabled = false;

		anim.SetFloat("InputVertical", 0);
		anim.SetFloat("InputHorizontal", 0);
		anim.SetFloat("VerticalVelocity", 0);


		//disable ThirsPerson Controller
		Invector.CharacterController.vThirdPersonController cc = this.GetComponent<Invector.CharacterController.vThirdPersonController>();
		cc.enabled = false;

		//remove players velocity
		Rigidbody rb = this.GetComponent<Rigidbody>();
		rb.velocity = Vector3.zero;

		//set sprint to false (just in case the player sprints)
		cc.Sprint(false);
	}

	public void Unlock()
	{
		if (state != STATES.LOCKED) return;
		state = STATES.IDLE;

		Invector.CharacterController.vThirdPersonInput ccI = this.GetComponent<Invector.CharacterController.vThirdPersonInput>();
		ccI.enabled = true;

		Invector.CharacterController.vThirdPersonController cc = this.GetComponent<Invector.CharacterController.vThirdPersonController>();
		cc.enabled = true;

	}

	public void TakeDamage()
	{

	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			if (state == STATES.DEAD && gameControl.control.amountOfLives > 0)
			{
				gameControl.control.amountOfLives--;
				respawnPlayer();
			}
			else if (state == STATES.DEAD && gameControl.control.amountOfLives == 0)
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(0, UnityEngine.SceneManagement.LoadSceneMode.Single);
			}
		}
	}



	public void respawnPlayer()
	{
		this.GetComponent<PlayerInventory>().removeAllItems();
		state = STATES.LOCKED;
		Unlock();
		Rigidbody rb = this.GetComponent<Rigidbody>();
		rb.useGravity = true;
		rb.WakeUp();
		this.GetComponent<AnimHandler>().resetCamera();
		this.GetComponent<GeneralMessageUI>().hideMessageImmediatly();
		levelManager.control.spawnPlayerOnSavePoint(gameControl.control.savePoint);
	}

	public void beKilledInstantly()
	{
		Rigidbody rb = this.GetComponent<Rigidbody>();
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.useGravity = false;
		rb.Sleep();
		Lock();
		health = 0;
		this.state = STATES.DEAD;
		if (gameControl.control.amountOfLives > 0)
			this.GetComponent<GeneralMessageUI>().DisplayMessage("TE RE MORISTE PA \n APRETA \"R\" PARA VOLVER A JUGAR", 0f, "bottom");
		else if (gameControl.control.amountOfLives == 0)
		{
			this.GetComponent<GeneralMessageUI>().DisplayMessage("GAME OVER VIEJA, \n APRETA \"R\" PARA VOLVER AL MENU", 0f, "bottom");
		}
		levelManager.control.restartLevel();
	}
}
