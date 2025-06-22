using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    [Header("Buttons")]
    public Button equipButton, biteButton, dropButton;

    [Header("Buttons")]
    public TMPro.TMP_Text itemTitle, itemDescription;


    CursorLockMode clm;
    bool cursorVisible;

    [SerializeField] public bool debug;

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
        selectedItemUI = itemUI;
        selectedItemInUI = itemUI.GetComponent<DraggeableItem>().GetItem();
        DrawItemPanelUI();
    }

    public void DrawItemPanelUI()
    {
        if (!selectedItemUI) return;

        itemTitle.text = selectedItemInUI.label;
        // TODO: use localization
        itemDescription.text = selectedItemInUI.description + "\n Cantidad: " + selectedItemInUI.amount.ToString();
    }



}
