using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Trigger : MonoBehaviour
{
	Puzzle_Main puzzle;
	Animator anim;
	

	private void Awake()
	{
		puzzle = GetComponentInParent<Puzzle_Main>();
		anim = GetComponent<Animator>();
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
			puzzle.PlaySound(PuzzleInputOutcome.right);

		}
		else
		{
			anim.SetTrigger("right");
			puzzle.PlaySound(PuzzleInputOutcome.right);
			//StartCoroutine("Highlight", Color.green);
		}
	}

	public void WrongInput(bool stay)
	{
		if (stay)
		{
			StopAllCoroutines();
			anim.SetTrigger("wrong");
			puzzle.PlaySound(PuzzleInputOutcome.wrong);
			//ChangeColor(Color.red);
		}
		else
		{
			anim.SetTrigger("wrong");
			puzzle.PlaySound(PuzzleInputOutcome.wrong);
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
