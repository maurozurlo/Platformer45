using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_ButtonCode : Puzzle_Main
{
	public override void GetInput(Puzzle_Trigger thisTrigger)
	{
		if (solved) return;
		int currentPos = currentInput.Length;
		string inputChar = thisTrigger.gameObject.name;
		if (currentPos > code.Length)
		{
			ClearInput();
			return;
		}
		string val = code[currentPos].ToString();
		if (val != inputChar)
		{
			ClearInput();
			return;
		}
		currentInput += inputChar;
		thisTrigger.CorrectInput(false);

		if (currentInput.Length == code.Length)
		{
			SolvePuzzle();
		}
	}
}
