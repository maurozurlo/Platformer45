using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SKULL_STATE
{
	idle,
	on_fire
}
public class PlayerSkullManager : MonoBehaviour
{
	GameObject FireParticle;
	SKULL_STATE CurrentState;


	private void Start()
	{
		FireParticle = transform.Find("FireBall").gameObject; // TODO: maybe make this less hardcoded
	}

	public void ChangeState(SKULL_STATE state)
	{
		if (CurrentState != state)
		{
			CurrentState = state;
			switch (state)
			{
				case SKULL_STATE.on_fire:
					FireParticle.SetActive(true);
					break;
				case SKULL_STATE.idle:
				default:
					FireParticle.SetActive(false);
					break;
			}
		}
	}
}
