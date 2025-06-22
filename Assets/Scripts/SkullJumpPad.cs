using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullJumpPad : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("PlayerSkull"))
		{
			var rb = other.GetComponent<Rigidbody>();

			rb.AddForce(Vector3.up * 35, ForceMode.Impulse);

			other.GetComponent<PlayerSkullManager>().ChangeState(SKULL_STATE.on_fire);
		}
	}
}
