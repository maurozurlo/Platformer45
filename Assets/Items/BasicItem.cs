using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class BasicItem : ScriptableObject
{
    public string label;
    public int id;
    public int amount = 1;
    public Vector3 itemPos;
    public bool hasBeenPickUp;
    public int[] canBeCombinedWithItems;
    public QuestItem[] canBeMadeFromItems;

    public BasicItem(string label, int id, int amount, Vector3 itemPos, bool hasBeenPickUp, int[] canBeCombinedWithItems){
        this.label = label;
        this.id = id;
        this.amount = amount;
        this.itemPos = itemPos;
        this.hasBeenPickUp = hasBeenPickUp;
        this.canBeCombinedWithItems = canBeCombinedWithItems;
    }
}
