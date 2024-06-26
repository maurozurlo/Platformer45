﻿using UnityEngine;


[System.Serializable]
public class NpcDialogue
{
    public string npcName;
    public int[] quests;
    public Dialogue[] dialogue;
}

[System.Serializable]
public class Dialogue
{
    public string id;
    public Node[] nodes;
}

[System.Serializable]
public class Node
{
    public int id;
    public string text;
    public Option[] options;
    public bool questCompleted;
}

[System.Serializable]
public class Option
{
    public string text;
    public string _action; // Until I figure out something better
    public ActionType action;
    public int value;
}

[System.Serializable]
public enum ActionType
{
    endChat,
    startQuest,
    goToNode,
    rewardAndEndChat
}