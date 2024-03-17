using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipText : MonoBehaviour
{
    private Text tooltip;
    public string id;
    public string fallback;
    // Start is called before the first frame update
    void Start()
    {
        tooltip = GetComponent<Text>();
        tooltip.text = I18nManager.control.GetValue(id, fallback);
    }
}
