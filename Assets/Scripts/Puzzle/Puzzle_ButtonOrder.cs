using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_ButtonOrder : Puzzle_Main
{
    public override void GetInput(Puzzle_Trigger thisTrigger)
    {
        if (solved) return;
        string inputChar = thisTrigger.gameObject.name;
        if (currentInput.Contains(inputChar)) return;

        int currentPos = currentInput.Length;
        string val = code[currentPos].ToString();

        if (val != inputChar)
        {
            ClearInput();
            return;
        }
        currentInput += inputChar;
        thisTrigger.CorrectInput(true);

        if (currentInput.Length == code.Length)
        {
            SolvePuzzle();
        }
    }
}
