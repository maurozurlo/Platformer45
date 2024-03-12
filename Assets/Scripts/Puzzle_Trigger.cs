using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Trigger : MonoBehaviour
{
	Puzzle_Main puzzle;
	

	private void Awake()
	{
		puzzle = GetComponentInParent<Puzzle_Main>();
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
			ChangeColor(Color.green);
		}
		else
		{
			StartCoroutine("Highlight", Color.green);
		}
	}

	public void WrongInput(bool stay)
	{
		if (stay)
		{
			StopAllCoroutines();
			ChangeColor(Color.red);
		}
		else
		{
			StartCoroutine("Highlight", Color.red);
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
