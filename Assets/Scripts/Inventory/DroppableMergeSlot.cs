using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DroppableMergeSlot : MonoBehaviour, IDropHandler
{

	// Placement into the slot is handled by DraggeableItem.OnEndDrag (reparenting).
	// Combining only happens when the player clicks the COMBINAR button.
	public void OnDrop(PointerEventData eventData)
	{
	}
}
