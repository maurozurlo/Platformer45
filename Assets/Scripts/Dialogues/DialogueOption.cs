using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    public string Text;
    public int DestinationNode = -1;
    public int DestinationQuest = -1;
}
