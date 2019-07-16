using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<BasicItem> playerItems;
    public UnityEngine.UI.Text inventoryUI;

    public void addItem(BasicItem thisItem)
    {
        bool add = false;

        foreach (BasicItem item in playerItems)
        {
            if (thisItem.id == item.id)
            {
                item.amount += thisItem.amount;
                add = true;
            }
        }

        if (!add)
        {
            playerItems.Add(thisItem);
        }
        //Redibujar interfaz
        inventoryUI.GetComponent<InventoryUI>().DrawUI();
    }

    public void removeItem(int itemID, int amount)
    {
        //Primero chequear si tenemos el item en cuestion
        int idToRemove = getInventoryListIndexByID(itemID);
        
        //Si encontramos el item a eliminar...
        if (idToRemove != -1)
        {
            if (playerItems[idToRemove].amount >= amount)
            {
                //Si tenemos más de lo que vamos a eliminar, mantenemos el item, pero bajamos la cantidad
                playerItems[idToRemove].amount -= amount;

                if (playerItems[idToRemove].amount == 0)
                {
                    //Tambien remover item, si el amount quedo en 0
                    playerItems.RemoveAt(idToRemove);
                }
            }
        }


        //Redibujar interfaz
        inventoryUI.GetComponent<InventoryUI>().DrawUI();
    }

    public int getInventoryListIndexByID(int id){
        //Buscamos el item
        for (int i = 0; i < playerItems.Count; i++)
        {
            if (playerItems[i].id == id)
                return i;
        }
        //Si no se encontro, devuelve -1
        return -1;
    }
}
