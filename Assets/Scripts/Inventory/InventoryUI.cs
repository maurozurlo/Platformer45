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

    // Cached on this same GameObject — used to actually show/hide the overlay.
    Canvas inventoryCanvas;
    GraphicRaycaster inventoryRaycaster;

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
        inventoryCanvas = GetComponent<Canvas>();
        inventoryRaycaster = GetComponent<GraphicRaycaster>();

        DrawUI(false);

        // Inventory starts closed/hidden. Opened with the I key (see Update).
        isOpen = false;
        SetInventoryVisible(false, false);

        // DEBUG
        if (debug)
        {
            ShowHideInventory();
        }

        // disable btns
        t = I18nManager.control;

        // Wire the COMBINAR button to craft from the current inventory.
        Transform mergeBtnTransform = transform.Find("Panel/Slots/MergeBtn");
        if (mergeBtnTransform != null)
        {
            Button mergeBtn = mergeBtnTransform.GetComponent<Button>();
            if (mergeBtn != null) mergeBtn.onClick.AddListener(OnCombinePressed);
        }
    }

    // Called by the COMBINAR button. Combines exactly the item in the top merge
    // slot with the item in the bottom merge slot, if they form a recipe.
    public void OnCombinePressed()
    {
        BasicItem itemA = GetItemInMergeSlot("Panel/Slots/Merge1");
        BasicItem itemB = GetItemInMergeSlot("Panel/Slots/Merge2");
        if (itemA == null || itemB == null) return; // need one item in each slot

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        PlayerInventory inv = player.GetComponent<PlayerInventory>();
        if (inv != null) inv.CombineTwo(itemA, itemB);
    }

    // Returns the item currently dragged into the given merge slot, or null.
    BasicItem GetItemInMergeSlot(string slotPath)
    {
        Transform slot = transform.Find(slotPath);
        if (slot == null) return null;
        DraggeableItem dragged = slot.GetComponentInChildren<DraggeableItem>();
        return dragged != null ? dragged.GetItem() : null;
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
        List<BasicItem> inventory = GameControl.control.inventory.OrderBy(item => item.id) // consistent, non-localized sorting
        .ToList();
        string itemsDetail = string.Empty;
        int totalItems = 0;
        int idx = 0;
        
        // Reset every slot back to the grid and clear it before refilling, so items
        // dragged into merge slots (or slots that are now empty) don't leave stale
        // icons/amounts behind.
        foreach (DraggeableItem slot in draggeableItems)
        {
            slot.ResetToSlot();
            Image slotImg = slot.GetComponent<Image>();
            if (slotImg != null) slotImg.enabled = false;
            slot.SetItem(null);
        }

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
        SetInventoryVisible(isOpen, true);
    }

    // Actually shows/hides the overlay. When the game runs as an overlay (not its own
    // scene) we must toggle the Canvas itself — the camera depth alone is not enough,
    // since a Screen Space - Overlay canvas renders regardless of any camera.
    void SetInventoryVisible(bool visible, bool affectCursorAndPlayer)
    {
        if (inventoryCanvas != null) inventoryCanvas.enabled = visible;
        if (inventoryRaycaster != null) inventoryRaycaster.enabled = visible;
        // Push the inventory camera behind the main camera when closed so its
        // background does not cover the game world.
        if (InventoryCamera != null) InventoryCamera.depth = visible ? 99 : -1;

        // Refresh the slots from the current inventory data every time we open,
        // so items picked up while the inventory was closed show up.
        if (visible) DrawUI(false);

        if (!affectCursorAndPlayer) return;

        if (visible)
        {
            //Guardar cursor actual
            clm = Cursor.lockState;
            cursorVisible = Cursor.visible;

            //Activar cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (PlayerCharacter.control != null) PlayerCharacter.control.Lock();
        }
        else
        {
            Cursor.lockState = clm;
            Cursor.visible = cursorVisible;
            if (PlayerCharacter.control != null) PlayerCharacter.control.Unlock();
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
