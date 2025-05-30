using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DroppableMergeSlot : MonoBehaviour, IDropHandler
{

	public void OnDrop(PointerEventData eventData)
	{
		/*var draggable = eventData.pointerDrag;
			draggable.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
			draggable.transform.SetParent(transform);/*/
	}
}
