using System.Collections.Generic;
using UnityEngine;

public class PuzzleReceptor : MonoBehaviour
{
    public List<QuestItem> questItems = new List<QuestItem>();
    private PlayerInventory inventory;
    public PuzzleContainer puzzleRewardContainer;
    public GameObject openVisualFX;
    public AudioClip solvedClip;
    AudioSource AS;
    public bool solved;

    private void Start()
    {
        if (gameControl.control != null)
        {
            GameObject player = gameControl.control.GetPlayerReference();
            if (player != null)
            {
                player.TryGetComponent(out inventory);
            }
        }
        AS = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (solved) return;
        if (!other.CompareTag("Player") || inventory == null)
            return;

        if (HasRequiredItems())
        {
            solved = true;
            RemoveItemsFromInventory();
            ActivateReward();
        }
    }

    private bool HasRequiredItems()
    {
        return questItems.TrueForAll(item =>
        {
            int itemId = inventory.GetInventoryListIndexByID(item.itemId);
            return itemId != -1 && gameControl.control.inventory[itemId].amount >= item.amount;
        });
    }

    private void RemoveItemsFromInventory()
    {
        foreach (QuestItem item in questItems)
        {
            inventory.RemoveItem(item.itemId, item.amount);
        }
    }

    public void ActivateReward()
    {
        puzzleRewardContainer.GetComponent<PuzzleContainer>().OpenContainer();
        openVisualFX.SetActive(true);
        AS.PlayOneShot(solvedClip);
    }
}
