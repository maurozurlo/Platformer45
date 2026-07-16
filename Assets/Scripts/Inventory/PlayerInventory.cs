using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public List<BasicItem> crafteableItems = new List<BasicItem>();
    bool itemsCanBeMerged;

	public void AddItem(BasicItem newItem)
    {
        bool alreadyHaveItem = false;
        foreach (BasicItem item in GameControl.control.inventory)
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
            GameControl.control.inventory.Add(clone);
        }

        // Check for mergeable items
        itemsCanBeMerged = CheckForMergeableItems();
        //Redibujar interfaz siempre (antes solo se redibujaba si había merge posible,
        // por eso los items recogidos no aparecían en el inventario)
        if (inventoryUI != null)
        {
            inventoryUI.DrawUI(itemsCanBeMerged);
        }
    }

    bool CheckForMergeableItems()
    {
        List<BasicItem> mergeableItems = new List<BasicItem>();

        // First, find items that can potentially be merged
        foreach (BasicItem item in GameControl.control.inventory)
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
                BasicItem mergeItem = GameControl.control.inventory.Find(invItem => invItem.id == mergeItemId && invItem.amount >= 1);

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
            // TODO: esto se va a llamar desde InventoryUI
            MergeItems();
        }
	}

    public bool MergeItems()
    {
        bool craftedSomething = false;

        // Preprocess the inventory for efficient lookups
        Dictionary<int, BasicItem> inventoryLookup = new Dictionary<int, BasicItem>();
        foreach (BasicItem invItem in GameControl.control.inventory)
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
                craftedSomething = true;
            }
        }

        return craftedSomething;
    }

    // Combines exactly two items (the ones placed in the two merge slots) into
    // their recipe result, consuming both. Returns true if a valid recipe existed.
    public bool CombineTwo(BasicItem a, BasicItem b)
    {
        if (a == null || b == null) return false;

        foreach (BasicItem craftable in crafteableItems)
        {
            QuestItem[] recipe = craftable.canBeMadeFromItems;
            if (recipe == null || recipe.Length != 2) continue;

            bool matches =
                (recipe[0].itemId == a.id && recipe[1].itemId == b.id) ||
                (recipe[0].itemId == b.id && recipe[1].itemId == a.id);
            if (!matches) continue;

            RemoveItem(recipe[0].itemId, recipe[0].amount);
            RemoveItem(recipe[1].itemId, recipe[1].amount);
            AddItem(Instantiate(craftable));
            return true;
        }
        return false;
    }

    public void RemoveItem(int itemID, int amount)
    {
        //Primero chequear si tenemos el item en cuestion
        int idToRemove = GetInventoryListIndexByID(itemID);
        
        //Si encontramos el item a eliminar...
        if (idToRemove != -1)
        {
            if (GameControl.control.inventory[idToRemove].amount >= amount)
            {
                //Si tenemos más de lo que vamos a eliminar, mantenemos el item, pero bajamos la cantidad
                GameControl.control.inventory[idToRemove].amount -= amount;

                if (GameControl.control.inventory[idToRemove].amount == 0)
                {
                    //Tambien remover item, si el amount quedo en 0
                    GameControl.control.inventory.RemoveAt(idToRemove);
                }
            }
        }

        itemsCanBeMerged = CheckForMergeableItems();
        //Redibujar interfaz
        inventoryUI.DrawUI(itemsCanBeMerged);
        
    }

    public void RemoveAllItems(){
        GameControl.control.inventory.Clear();
        //Redibujar interfaz
        inventoryUI.DrawUI(false);
        
    }

    public int GetInventoryListIndexByID(int id){
        //Buscamos el item
        for (int i = 0; i < GameControl.control.inventory.Count; i++)
        {
            if (GameControl.control.inventory[i].id == id)
                return i;
        }
        //Si no se encontro, devuelve -1
        return -1;
    }

}
