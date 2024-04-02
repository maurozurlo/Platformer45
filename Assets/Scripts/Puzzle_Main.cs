using System.Collections.Generic;
using UnityEngine;

public enum PuzzleInputOutcome {
right, wrong
}

public class Puzzle_Main : MonoBehaviour
{
    public string code = "0123456";
    public string currentInput = "";
    public bool solved;
    public List<Puzzle_Trigger> triggers = new List<Puzzle_Trigger>();
    private AudioSource AS;
    public AudioClip right;
    public AudioClip wrong;


    private void Awake()
    {
        AS = GetComponent<AudioSource>();

        foreach (Puzzle_Trigger trigger in GetComponentsInChildren<Puzzle_Trigger>())
        {
            triggers.Add(trigger);
        }
    }


    public virtual void GetInput(Puzzle_Trigger thisTrigger)
    {
        // Override me
    }

    public void PlaySound(PuzzleInputOutcome outcome)
    {
        if (AS.isPlaying) return;
        AS.PlayOneShot(outcome == PuzzleInputOutcome.right ? right : wrong);
    }

    public void SolvePuzzle()
    {
        Debug.Log("You cracked the code");
        solved = true;
        triggers.ForEach(delegate (Puzzle_Trigger trigger)
        {
            trigger.CorrectInput(true);
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
