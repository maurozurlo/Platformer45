using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class BasicItem:ScriptableObject
{
    public string label;
    public int id;
    public int amount = 1;
    public Vector3 itemPos;
    public bool hasBeenPickUp;

    public BasicItem(string label, int id, int amount, Vector3 pos, bool hasBeenPickUp){
        this.label = label;
        this.id = id;
        this.amount = amount;
        this.itemPos = pos;
        this.hasBeenPickUp = hasBeenPickUp;
    }
}
