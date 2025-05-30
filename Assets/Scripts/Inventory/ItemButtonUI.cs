using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButtonUI : MonoBehaviour
{
    public BasicItem item;
    public bool isSelected;
    private DraggeableItem draggeable;
    void Start()
    {
        draggeable = GetComponentInChildren<DraggeableItem>();
    }

    public void OnClickHandler()
    {
        isSelected = true;
        draggeable.GetComponent<UnityEngine.UI.Image>().raycastTarget = true;
    }
}
