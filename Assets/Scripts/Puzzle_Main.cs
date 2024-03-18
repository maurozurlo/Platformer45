using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Main : MonoBehaviour
{
    public string code = "0123456";
    public string currentInput = "";
    public bool solved;
    public List<Puzzle_Trigger> triggers = new List<Puzzle_Trigger>();

    private void Awake()
    {
        foreach (Puzzle_Trigger trigger in GetComponentsInChildren<Puzzle_Trigger>())
        {
            triggers.Add(trigger);
        }
    }


    public virtual void GetInput(Puzzle_Trigger thisTrigger)
    {
        // Override me
    }

    public void SolvePuzzle()
    {
        Debug.Log("You cracked the code");
        solved = true;
        triggers.ForEach(delegate (Puzzle_Trigger trigger)
        {
            //trigger.CorrectInput(true);
        });
    }

    public void ClearInput()
    {
        currentInput = "";
        triggers.ForEach(delegate (Puzzle_Trigger trigger)
        {
            trigger.WrongInput(false);
        });
    }
}
