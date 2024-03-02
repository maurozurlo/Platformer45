using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector;

public class NPCInteraction : MonoBehaviour
{
    public float waitTime = 1f;
    public Vector3 cameraPos;
    Vector3 startPlayerPos;
    //GameObjects
    public GameObject dialogueUI;
    GameObject cameraPivot;
    GameObject player;
    PlayerCharacter playerCharacter;
    //Esto lo dejamos para cuando podamos cargar dialogos en XML
    public List<DialogueManager> thisCharsDialogues;
    //Quests que este NPC puede dar
    public int QuestThisNPCCanGive;
    Invector.CharacterController.vThirdPersonInput playerControl;

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
            playerControl = other.gameObject.GetComponent<Invector.CharacterController.vThirdPersonInput>();
            playerCharacter = other.gameObject.GetComponent<PlayerCharacter>();
            //Aca chequear primero si el personaje tiene una Quest, sino
            if(player.GetComponent<QuestManager>().currentQuest != null){
                //Tenemos una quest, chequear si es una de este NPC en particular
                if(QuestThisNPCCanGive == player.GetComponent<QuestManager>().currentQuestID)
                {
                    //Cheuqear estado de la quest y responder en consecuencia
                    StartConversation(1,player.GetComponent<QuestManager>().checkQuestStatus());
                }else{
                    //Es una quest de este personaje, iniciar la conversacion 
                    StartConversation(1,0);
                }
            }else{
                if(gameControl.control.checkIfQuestIsCompleted(QuestThisNPCCanGive)){
                    StartConversation(2,0);
                }else{
                    StartConversation(0,0);
                }
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
            playerControl.ShowHidePlayer(false);
            playerCharacter.Lock();
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
        player.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z - 2);
        playerControl.ShowHidePlayer(true);
        dialogueUI.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        //Desbloqueamos al player
        playerCharacter.Unlock();
    }
}
