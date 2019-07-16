using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class Quest : ScriptableObject
{
    public int QuestID = -1;
    public string QuestName = "Default Quest";
    public bool isCompleted;

    public enum questType {
        itemHunt, characterHunt
    }

    public List<QuestItem> itemHuntDB;

    public questType QuestType;
}
