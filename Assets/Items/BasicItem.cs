using UnityEngine;

[System.Serializable]
public class BasicItem
{
    public string label;
    public int id;
    public int amount = 1;
    public Vector3 itemPos;

    public BasicItem(string label, int id, int amount, Vector3 pos){
        this.label = label;
        this.id = id;
        this.amount = amount;
        this.itemPos = pos;

    }
}
