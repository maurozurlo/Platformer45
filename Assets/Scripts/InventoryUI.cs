using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject player;
    Text myText;

    private void Awake() {
        myText = this.GetComponent<Text>();
    }

    // Update is called once per frame
    public void DrawUI(bool itemsCanBeMerged)
    {
        List<BasicItem> inventory = gameControl.control.inventory;
        string itemsDetail = string.Empty;
        int totalItems = 0;

        foreach(BasicItem item in inventory){
            string itemName = I18nManager.control.GetValue($"item_{item.id}_item_name", item.label);
            itemsDetail += " " + CheckIfPlural(itemName, item.amount) + ": " + item.amount.ToString();
            totalItems += item.amount;
        }
        if(itemsDetail != ""){
            itemsDetail = ", " + itemsDetail;
        }


        myText.text = "Items: " + totalItems.ToString() + itemsDetail + (itemsCanBeMerged ? ". Algunos objetos se pueden combinar apretando J" : "");
    }

    string CheckIfPlural(string label, int amount){
        if(amount >= 2)
            return label + "s";
        else
            return label;
    }
    
}
