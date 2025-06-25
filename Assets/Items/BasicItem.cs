using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class BasicItem : ScriptableObject
{
    [Header("Basic Info")]
    public string label;

    [TextArea]
    public string description;

    public int id;
    public int amount = 1;

    [Tooltip("The original position of the item in the world, if applicable.")]
    public Vector3 itemPos;

    public bool hasBeenPickedUp;

    [Header("Combination and Crafting")]
    [Tooltip("IDs of items this one can be combined with.")]
    public int[] canBeCombinedWithItems;

    [Tooltip("Items required to craft this one.")]
    public QuestItem[] canBeMadeFromItems;

    [Header("Visuals")]
    public Sprite sprite;

    [Header("Properties")]
    public bool isDroppable;
    public bool canBeEquippedInBody;
    public bool canBeEquippedInHead;

    // If you want to initialize at runtime, create a method instead:
    public void Initialize(string label, int id, int amount, Vector3 itemPos, bool hasBeenPickedUp, int[] combinableWith, string description)
    {
        this.label = label;
        this.id = id;
        this.amount = amount;
        this.itemPos = itemPos;
        this.hasBeenPickedUp = hasBeenPickedUp;
        this.canBeCombinedWithItems = combinableWith;
        this.description = description;
    }
}
