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
    Invector.CharacterController.vThirdPersonInput playerControl;
    public MessageReader messageReader;
    
    void Start()
    {
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
            // TODO: get message if not first time we bump into this character
            StartConversation();
        }
    }


    void StartConversation()
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
            NpcDialogue npcDialogue = messageReader.GetDialogue();
            dialogueUI.GetComponent<DialogueUI>().npcDialogue = npcDialogue;            
            dialogueUI.GetComponent<DialogueUI>().StartDialogue(this.gameObject);
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
        playerCharacter.Unlock();
        yield return new WaitForSeconds(waitTime);
        //Desbloqueamos al player
        
    }
}
