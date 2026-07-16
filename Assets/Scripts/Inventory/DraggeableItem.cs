using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggeableItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	[SerializeField] private Canvas canvas;
	private RectTransform rect;
	private CanvasGroup canvasGroup;
	private int childIndex;
	private Transform parent;
	Vector2 originalPos;
	InventoryUI inventoryUI;

	bool canBeDragged;

	[SerializeField] private BasicItem item;

	Image slotUI;

	Color defaultColor = Color.white;
	Color selectedColor = new Color(.8f, .8f, .8f);

	private void Awake()
	{
		rect = GetComponent<RectTransform>();
		originalPos = rect.anchoredPosition;
		canvasGroup = GetComponent<CanvasGroup>();
		childIndex = transform.GetSiblingIndex();
		parent = transform.parent;
		slotUI = parent.GetComponent<Image>();
	}

	void Start()
	{
		inventoryUI = InventoryUI.control;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (!canBeDragged) return;
		canvasGroup.blocksRaycasts = true;
		canvasGroup.alpha = 1f;

		GameObject target = eventData.pointerEnter;
		if (target != null && target.CompareTag("MergeSlot"))
		{
			// Place this item into the merge slot.
			transform.SetParent(target.transform);
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
			UnselectItem();
			// Selection highlight follows the item into the merge slot.
			Image mergeSlotImg = target.GetComponent<Image>();
			if (mergeSlotImg != null) slotUI = mergeSlotImg;
		}
		else
		{
			ResetToSlot();
		}
	}

	// Returns this item's UI element to its original grid slot. Called on a failed
	// drag and by InventoryUI.DrawUI, so every redraw starts from a clean grid and
	// items left in merge slots don't corrupt the index-based layout.
	public void ResetToSlot()
	{
		transform.SetParent(parent);
		rect.anchoredPosition = originalPos;
		transform.SetSiblingIndex(childIndex);
		if (parent != null)
		{
			Image parentImg = parent.GetComponent<Image>();
			if (parentImg != null) slotUI = parentImg;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
    {
		if (!item) return;
		inventoryUI.HandleSelectItem(gameObject);
		slotUI.color = selectedColor;
		canBeDragged = true;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!canBeDragged) return;
		if (!item) return;
		canvasGroup.alpha = .8f;
		canvasGroup.blocksRaycasts = false;
		transform.SetParent(canvas.transform);
		transform.SetAsLastSibling();

	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!canBeDragged) return;
		rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
	}

	public void SetItem(BasicItem newItem) {
		item = newItem;
	}

	public BasicItem GetItem()
	{
		return item;
	}

	public void UnselectItem()
	{
		slotUI.color = defaultColor;
		canBeDragged = false;
	}

}
