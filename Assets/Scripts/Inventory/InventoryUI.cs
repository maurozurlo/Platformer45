using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    bool isOpen;
    public static InventoryUI control;

    [Header("Visuals")]
    public Camera InventoryCamera;
    public GameObject InventoryCanvas;
    public GameObject InventoryCardUI;
    public GameObject selectedItemUI;
    public BasicItem selectedItemInUI;
    public List<DraggeableItem> draggeableItems = new List<DraggeableItem>();


    [Header("Buttons")]
    public Button equipButton, biteButton, dropButton;

    [Header("Buttons")]
    public TMPro.TMP_Text itemTitle, itemDescription;


    CursorLockMode clm;
    bool cursorVisible;

    [SerializeField] public bool debug;

    private I18nManager t;


    private void Awake()
	{
        if (control)
        {
            DestroyImmediate(this);
        }
        else
        {
            control = this;
        }
	}

	private void Start()
	{
        DrawUI(false);
        // DEBUG
        if (debug)
        {
            ShowHideInventory();
        }

        // disable btns
        t = I18nManager.control;
    }

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.I))
        {
            // TODO: seguramente mas logica aca para ver si puedo abrirlo, pero por ahora da igual
            ShowHideInventory();
        }
    }

	// Update is called once per frame
	public void DrawUI(bool itemsCanBeMerged)
    {
        List<BasicItem> inventory = gameControl.control.inventory.OrderBy(item => item.id) // consistent, non-localized sorting
        .ToList();
        string itemsDetail = string.Empty;
        int totalItems = 0;
        int idx = 0;
        
        I18nManager t = I18nManager.control;
        foreach (BasicItem item in inventory) {
            string itemName = t.GetValue($"item_{item.id}_item_name", item.label);
            itemsDetail += " " + CheckIfPlural(itemName, item.amount) + ": " + item.amount.ToString();
            totalItems += item.amount;
            GameObject itemUI = draggeableItems[idx].gameObject;
            draggeableItems[idx].SetItem(item);
            itemUI.GetComponent<Image>().enabled = true;
            // Amount UI
            TextMeshProUGUI amountUI = itemUI.GetComponentInChildren<TextMeshProUGUI>();
            if (item.sprite)
            {
                itemUI.GetComponent<Image>().sprite = item.sprite;
            }
            if (item.amount >= 2)
            {
                amountUI.text = item.amount.ToString();
            }
            else
            {
                amountUI.text = "";
            }
            idx++;
        }
        if(itemsDetail != ""){
            itemsDetail = ", " + itemsDetail;
        }

        string items = t.GetValue("ui_items", "Objetos: ");
        string canMergeItems = (itemsCanBeMerged ? t.GetValue("ui_items_combination", "Algunos objetos se pueden combinar") : "");
    }

    string CheckIfPlural(string label, int amount){
        if(amount >= 2)
            return label + "s";
        else
            return label;
    }

    void ShowHideInventory()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            InventoryCamera.depth = 99;
            //Guardar cursor actual
            clm = Cursor.lockState;
            cursorVisible = Cursor.visible;

            //Activar cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //InventoryCanvas.SetActive(true);
            PlayerCharacter.control.Lock();
        }
        else
        {
            InventoryCamera.depth = -1;
            //InventoryCanvas.SetActive(false);
            Cursor.lockState = clm;
            Cursor.visible = cursorVisible;
            PlayerCharacter.control.Unlock();
        }
    }

    public void HandleSelectItem(GameObject itemUI)
    {
        foreach (DraggeableItem item in draggeableItems)
        {
            item.UnselectItem();
        }
        selectedItemUI = itemUI;
        selectedItemInUI = itemUI.GetComponent<DraggeableItem>().GetItem();
        DrawItemPanelUI();
    }

    public void DrawItemPanelUI()
    {
        if (selectedItemUI == null || selectedItemInUI == null) return;

        string label = t.GetValue($"item_{selectedItemInUI.id}_item_name", selectedItemInUI.label);
        string desc = t.GetValue($"item_{selectedItemInUI.id}_item_desc", selectedItemInUI.description);
        string amountLabel = t.GetValue("global_amount", "Amount");

        itemTitle.text = label;
        itemDescription.text = $"{desc}\n{amountLabel}: {selectedItemInUI.amount}";
    }



}
