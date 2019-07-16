using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    //UI
    public Camera npcCamera;
    public Text npcName;
    public Text npcText;
    public Button option1;
    public Button option2;
    //Current Dialogue
    public DialogueManager dm;
    //GameObjects
    public GameObject NPC;
    public GameObject player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            Debug.LogError("Player couldn't be found");
    }



    public void StartDialogue(GameObject npc, int nodeToStart)
    {
        NPC = npc;
        //Primero seteamos la camera
        npcCamera.depth = 2;
        npcCamera.transform.position = NPC.GetComponent<NPCInteraction>().cameraPos;
        Vector3 v3 = new Vector3(0, NPC.transform.rotation.eulerAngles.y - 180, 0);
        Quaternion qt = Quaternion.Euler(v3);
        npcCamera.transform.rotation = qt;

        if (dm != null)
        {
            DrawUI(nodeToStart);
        }
    }
    // Start is called before the first frame update
    public void GoToNextNode(int optionButton)
    {
        Button btnPressed;
        if (optionButton == 1)
            btnPressed = option1;
        else
            btnPressed = option2;

        if (btnPressed.GetComponent<OptionUI>().goToQuest != -1)
            //Iniciar quest
            player.GetComponent<QuestManager>().StartQuest(btnPressed.GetComponent<OptionUI>().goToQuest);

        int nextNode = btnPressed.GetComponent<OptionUI>().goToNode;

        DrawUI(nextNode);
    }

    public void DrawUI(int mainNode)
    {
        if (checkIfNodeExists(mainNode))
        {
            //Lo que dice el NPC
            npcName.text = dm.npcName;
            npcText.text = dm.Nodes[mainNode].Text;
            
            //La opcion 1
            if(dm.Nodes[mainNode].Options[0].DestinationNode == -2)
            option1.gameObject.SetActive(false);
            else
            {
            option1.gameObject.SetActive(true);
            option1.GetComponentInChildren<Text>().text = dm.Nodes[mainNode].Options[0].Text;
            option1.GetComponent<OptionUI>().goToNode = dm.Nodes[mainNode].Options[0].DestinationNode;
            option1.GetComponent<OptionUI>().goToQuest = dm.Nodes[mainNode].Options[0].DestinationQuest;
            }
            //La opcion 2
            option2.GetComponentInChildren<Text>().text = dm.Nodes[mainNode].Options[1].Text;
            option2.GetComponent<OptionUI>().goToNode = dm.Nodes[mainNode].Options[1].DestinationNode;
            option2.GetComponent<OptionUI>().goToQuest = dm.Nodes[mainNode].Options[1].DestinationQuest;    

        }
        else
        {
            //Exit conversation
            npcCamera.depth = -2;
            NPC.GetComponent<NPCInteraction>().StartCoroutine("EndConversation");
        }
    }

    bool checkIfNodeExists(int mainNode)
    {
        if (mainNode < 0)
            return false;
        if (dm.Nodes[mainNode] != null)
            return true;
        else
            return false;
    }
}
