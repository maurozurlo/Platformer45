using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public UnityEngine.UI.Text inventoryUI;
    public List<BasicItem> crafteableItems = new List<BasicItem>();
    bool itemsCanBeMerged;

    public void AddItem(BasicItem newItem)
    {
        bool alreadyHaveItem = false;
        foreach (BasicItem item in gameControl.control.inventory)
        {
            if (item.id == newItem.id)
            {
                item.amount += newItem.amount;
                alreadyHaveItem = true;
            }
        }
        if (!alreadyHaveItem)
        {
            BasicItem clone = Instantiate(newItem);
            gameControl.control.inventory.Add(clone);
        }

        // Check for mergeable items
        itemsCanBeMerged = CheckForMergeableItems();
        //Redibujar interfaz
        inventoryUI.GetComponent<InventoryUI>().DrawUI(itemsCanBeMerged);
    }

    bool CheckForMergeableItems()
    {
        List<BasicItem> mergeableItems = new List<BasicItem>();

        // First, find items that can potentially be merged
        foreach (BasicItem item in gameControl.control.inventory)
        {
            if (item.canBeCombinedWithItems.Length >= 1)
            {
                mergeableItems.Add(item);
            }
        }

        // Now check if these mergeable items can actually be merged
        foreach (BasicItem item in mergeableItems)
        {
            bool canBeMerged = false;

            foreach (int mergeItemId in item.canBeCombinedWithItems)
            {
                // Find the item in inventory that can be merged with 'item'
                BasicItem mergeItem = gameControl.control.inventory.Find(invItem => invItem.id == mergeItemId && invItem.amount >= 1);

                if (mergeItem != null)
                {
                    canBeMerged = true;
                    break; // No need to check further if one merge item is found
                }
            }

            if (canBeMerged)
            {
                return true;
            }
        }
        return false;
    }

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.J))
        {
            MergeItems();
        }
	}

    void MergeItems()
    {
        // Preprocess the inventory for efficient lookups
        Dictionary<int, BasicItem> inventoryLookup = new Dictionary<int, BasicItem>();
        foreach (BasicItem invItem in gameControl.control.inventory)
        {
            inventoryLookup[invItem.id] = invItem;
        }

        foreach (BasicItem item in crafteableItems)
        {
            int itemsPlayerHas = 0;

            foreach (QuestItem itemNeeded in item.canBeMadeFromItems)
            {
                // Check if the required item exists in the inventory lookup
                if (inventoryLookup.TryGetValue(itemNeeded.itemId, out BasicItem requiredItem) && requiredItem.amount >= itemNeeded.amount)
                {
                    itemsPlayerHas++;
                }
                else
                {
                    // Required item not found or not enough quantity, break out of the loop
                    break;
                }
            }

            // If all required items are found in sufficient quantity
            if (itemsPlayerHas == item.canBeMadeFromItems.Length)
            {
                // We can make this item
                BasicItem clone = Instantiate(item);
                foreach (QuestItem requiredItem in item.canBeMadeFromItems)
                {
                    RemoveItem(requiredItem.itemId, requiredItem.amount);
                }
                AddItem(clone);
            }
        }
    }

    public void RemoveItem(int itemID, int amount)
    {
        //Primero chequear si tenemos el item en cuestion
        int idToRemove = GetInventoryListIndexByID(itemID);
        
        //Si encontramos el item a eliminar...
        if (idToRemove != -1)
        {
            if (gameControl.control.inventory[idToRemove].amount >= amount)
            {
                //Si tenemos más de lo que vamos a eliminar, mantenemos el item, pero bajamos la cantidad
                gameControl.control.inventory[idToRemove].amount -= amount;

                if (gameControl.control.inventory[idToRemove].amount == 0)
                {
                    //Tambien remover item, si el amount quedo en 0
                    gameControl.control.inventory.RemoveAt(idToRemove);
                }
            }
        }

        itemsCanBeMerged = CheckForMergeableItems();
        //Redibujar interfaz
        inventoryUI.GetComponent<InventoryUI>().DrawUI(itemsCanBeMerged);
        
    }

    public void RemoveAllItems(){
        gameControl.control.inventory.Clear();
        //Redibujar interfaz
        inventoryUI.GetComponent<InventoryUI>().DrawUI(false);
        
    }

    public int GetInventoryListIndexByID(int id){
        //Buscamos el item
        for (int i = 0; i < gameControl.control.inventory.Count; i++)
        {
            if (gameControl.control.inventory[i].id == id)
                return i;
        }
        //Si no se encontro, devuelve -1
        return -1;
    }
}
