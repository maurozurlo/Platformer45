using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAnim : MonoBehaviour
{
    BoneThrower bt;

	private void Start()
	{
        bt = GetComponentInParent<BoneThrower>();
	}
	// Start is called before the first frame update
	void StartThrow()
    {
        //bt.startTransitionTime = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;

    }

    void EndThrow()
    {
        
        //Debug.Log("Got to end throw");
    }
}
