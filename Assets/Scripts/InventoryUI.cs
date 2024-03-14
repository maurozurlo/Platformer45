using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject player;
    Text textComponent;

	private void Start()
	{
        textComponent = GetComponent<Text>();
        DrawUI(false);
    }

	// Update is called once per frame
	public void DrawUI(bool itemsCanBeMerged)
    {
        List<BasicItem> inventory = gameControl.control.inventory;
        string itemsDetail = string.Empty;
        int totalItems = 0;

        I18nManager t = I18nManager.control;
        foreach (BasicItem item in inventory){
            string itemName = t.GetValue($"item_{item.id}_item_name", item.label);
            itemsDetail += " " + CheckIfPlural(itemName, item.amount) + ": " + item.amount.ToString();
            totalItems += item.amount;
        }
        if(itemsDetail != ""){
            itemsDetail = ", " + itemsDetail;
        }

        string items = t.GetValue("ui_items", "Objetos: ");
        string canMergeItems = (itemsCanBeMerged ? t.GetValue("ui_items_combination", "Algunos objetos se pueden combinar") : "");
        textComponent.text = items + totalItems.ToString() + itemsDetail + canMergeItems;
    }

    string CheckIfPlural(string label, int amount){
        if(amount >= 2)
            return label + "s";
        else
            return label;
    }
    
}
