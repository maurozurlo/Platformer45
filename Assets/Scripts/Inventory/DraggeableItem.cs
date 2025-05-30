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
		GameObject target = eventData.pointerEnter;
		canvasGroup.alpha = 1f;
		

		if (target != null && target.CompareTag("MergeSlot"))
		{
			transform.SetParent(target.transform);
			rect.anchorMin = new Vector2(0, 0); // bottom-left
			rect.anchorMax = new Vector2(1, 1); // top-right
			rect.offsetMin = Vector2.zero; // left + bottom
			rect.offsetMax = Vector2.zero; // right + top
		}
		else
		{
			transform.SetParent(parent);
			rect.anchoredPosition = originalPos;
			transform.SetSiblingIndex(childIndex);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
    {
		inventoryUI.HandleSelectItem(gameObject);
		slotUI.color = selectedColor;
		canBeDragged = true;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!canBeDragged) return;
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

}
