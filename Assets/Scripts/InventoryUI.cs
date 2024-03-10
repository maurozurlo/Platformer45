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
            itemsDetail += " " + CheckIfPlural(item.label, item.amount) + ": " + item.amount.ToString();
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
