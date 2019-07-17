using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public float waitTime = 1f;
    public Vector3 cameraPos;
    Vector3 startPlayerPos;
    //GameObjects
    public GameObject dialogueUI;
    GameObject cameraPivot;
    GameObject player;
    //Esto lo dejamos para cuando podamos cargar dialogos en XML
    public List<DialogueManager> thisCharsDialogues;
    //Quests que este NPC puede dar
    public int[] QuestsThisNPCCanGive;

    void Start()
    {
        //dialogueUI = GameObject.Find("DialogueUI");
        if (dialogueUI == null)
            Debug.LogError("Dialogue UI not found");
        foreach (Transform item in transform)
        {
            if(item.name == "cameraPivot"){
                cameraPivot = item.gameObject;
                cameraPos = cameraPivot.transform.position;
            }
        }
        if (cameraPivot == null)
        {
            Debug.LogError("CameraPivot not found");
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Start conversation
            player = other.gameObject;
            //Aca chequear primero si el personaje tiene una Quest, sino
            if(player.GetComponent<QuestManager>().currentQuest != null){
                //Tenemos una quest, chequear si es una de este NPC en particular
                if(checkIfThisIsAQuestThisNPCCanGive(player.GetComponent<QuestManager>().currentQuestID))
                {
                    //Cheuqear estado de la quest y responder en consecuencia
                    StartConversation(1,player.GetComponent<QuestManager>().checkQuestStatus());
                }else{
                    StartConversation(1,0);
                }
                //Es una quest de este personaje, iniciar la conversacion 
            }else{
                StartConversation(0,0);
            }
        }
    }


    void StartConversation(int dialogueNumber, int nodeToStart)
    {
        //Ask if the player is locked
        if (!player.GetComponent<PlayerCharacter>().isLocked())
        {
            //Move Player
            startPlayerPos = player.transform.position;
            player.transform.position = new Vector3(player.transform.position.x,player.transform.position.y -10,player.transform.position.z);
            WaitForIdle();

            //Start UI
            dialogueUI.SetActive(true);
            //En realidad acá habria que setear el DM (Que vendria de cada NPC... pero por ahora no)
            dialogueUI.GetComponent<DialogueUI>().dm = thisCharsDialogues[dialogueNumber];
            dialogueUI.GetComponent<DialogueUI>().StartDialogue(this.gameObject,nodeToStart);
            //Cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public IEnumerator WaitForIdle(){
        //Lock the player in 5 seconds
        yield return new WaitForSeconds(5);
        player.GetComponent<PlayerCharacter>().Lock();
        player.GetComponent<Animator>().SetTrigger("Idle");
    }

    public IEnumerator EndConversation()
    {
        //Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //Esperamos
        player.GetComponent<Animator>().SetTrigger("ReturnToNormal");
        //Movemos al player
        player.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z - 5);
        //Desbloqueamos al player
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        dialogueUI.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        player.GetComponent<PlayerCharacter>().Unlock();
    }

    public bool checkIfThisIsAQuestThisNPCCanGive(int currentQuest){
        foreach (int item in QuestsThisNPCCanGive)
        {
            if(item == currentQuest)
            return true;
        }
        return false;
    }
}
