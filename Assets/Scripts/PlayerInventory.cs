using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public UnityEngine.UI.Text inventoryUI;

    public void addItem(BasicItem thisItem)
    {
        bool add = false;

        foreach (BasicItem item in gameControl.control.inventory)
        {
            if (thisItem.id == item.id)
            {
                item.amount += thisItem.amount;
                add = true;
            }
        }

        if (!add)
        {
            gameControl.control.inventory.Add(thisItem);
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


        //Redibujar interfaz
        inventoryUI.GetComponent<InventoryUI>().DrawUI();
        
    }

    public void removeAllItems(){
        gameControl.control.inventory.Clear();
        //Redibujar interfaz
        inventoryUI.GetComponent<InventoryUI>().DrawUI();
        
    }

    public int getInventoryListIndexByID(int id){
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
