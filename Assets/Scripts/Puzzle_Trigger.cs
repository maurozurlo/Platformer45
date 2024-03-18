using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Trigger : MonoBehaviour
{
	Puzzle_Main puzzle;
	Animator anim;
	AudioSource AS;
	public AudioClip right;
	public AudioClip wrong;
	

	private void Awake()
	{
		puzzle = GetComponentInParent<Puzzle_Main>();
		anim = GetComponent<Animator>();
		AS = GetComponent<AudioSource>();
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			puzzle.GetInput(this);
		}
	}

	public void CorrectInput(bool stay)
	{
		if (stay)
		{
			StopAllCoroutines();
			//ChangeColor(Color.green);
			anim.SetTrigger("right");
			AS.PlayOneShot(right);

		}
		else
		{
			anim.SetTrigger("right");
			AS.PlayOneShot(right);
			//StartCoroutine("Highlight", Color.green);
		}
	}

	public void WrongInput(bool stay)
	{
		if (stay)
		{
			StopAllCoroutines();
			anim.SetTrigger("wrong");
			AS.PlayOneShot(wrong);
			//ChangeColor(Color.red);
		}
		else
		{
			anim.SetTrigger("wrong");
			AS.PlayOneShot(wrong);
			//StartCoroutine("Highlight", Color.red);
		}
	}

	IEnumerator Highlight(Color color)
	{
		ChangeColor(color);
		yield return new WaitForSeconds(1);
		ChangeColor(Color.white);
	}

	void ChangeColor(Color color)
	{
		GetComponent<MeshRenderer>().material.color = color;
	}

}
