using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GeneralMessageUI : MonoBehaviour
{
    public Text generalMessage;
    public Image generalMessageBG;

    // Start is called before the first frame update
    void Start()
    {
        if(generalMessage == null || generalMessageBG == null)
        Debug.LogError("a General Messaging component is missing");
    }

    public void DisplayMessage(string message, float fadeAfterSeconds, string position){
        if (position == "top")
        {
            generalMessageBG.rectTransform.SetAnchor(AnchorPresets.TopCenter);
            generalMessageBG.rectTransform.anchoredPosition = new Vector3(0,-75,0);

        }else if (position == "bottom"){
            generalMessageBG.rectTransform.SetAnchor(AnchorPresets.BottomCenter);
            generalMessageBG.rectTransform.anchoredPosition = new Vector3(0,75,0);
        }

        generalMessage.text = message;
        generalMessageBG.gameObject.SetActive(true);        
        if(fadeAfterSeconds > 0)
        StartCoroutine(hideMessageWithDelay(fadeAfterSeconds));
    }

    IEnumerator hideMessageWithDelay(float fadeAfterSeconds){
        yield return new WaitForSeconds(fadeAfterSeconds);
        hideMessageImmediatly();
    }

    public void hideMessageImmediatly(){
        generalMessageBG.gameObject.SetActive(false);
        generalMessage.text = string.Empty;
    }
}
