using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class DialogueManager : ScriptableObject
{
    [TextArea]
    public string Notes = "Destination node settings: \n 0-Infinite: Conversation node \n -1: Exit conversation \n -2: Hide first option button";
    public string npcName;
    public List<DialogueNode> Nodes;
}
