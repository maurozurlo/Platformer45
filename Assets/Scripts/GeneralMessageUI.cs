using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GeneralMessageUI : MonoBehaviour
{
    public Text generalMessage;
    void Start()
    {
        if(generalMessage == null)
        Debug.LogError("a General Messaging component is missing");
    }

    public void DisplayMessage(string message, float fadeAfterSeconds){
        generalMessage.text = message;
        if(fadeAfterSeconds > 0)
        StartCoroutine(HideMessageWithDelay(fadeAfterSeconds));
    }

    IEnumerator HideMessageWithDelay(float fadeAfterSeconds){
        yield return new WaitForSeconds(fadeAfterSeconds);
        HideMessageImmediatly();
    }

    public void HideMessageImmediatly(){
        generalMessage.text = string.Empty;
    }
}
