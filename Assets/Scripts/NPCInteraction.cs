using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector;

public class NPCInteraction : MonoBehaviour
{
    public float waitTime = 1f;
    public Vector3 cameraPos;
    //GameObjects
    public GameObject dialogueUI;
    GameObject cameraPivot;
    GameObject player;
    PlayerCharacter playerCharacter;
    Invector.CharacterController.vThirdPersonInput playerControl;
    public MessageReader messageReader;
    
    void Awake()
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
            StartConversationWrapper(player);
        }
    }

    public void StartConversationWrapper(GameObject other)
    {
        SetPlayer(other);
        StartConversation();
    }

    void SetPlayer(GameObject playerGO)
    {
        player = playerGO;
        playerControl = playerGO.GetComponent<Invector.CharacterController.vThirdPersonInput>();
        playerCharacter = playerGO.GetComponent<PlayerCharacter>();
    }


    void StartConversation()
    {
        //Ask if the player is locked
        if (!player.GetComponent<PlayerCharacter>().isLocked())
        {
            //Move Player
            player.transform.position = transform.position + (transform.forward * 3.5f);
            playerControl.ShowHidePlayer(false);
            playerCharacter.Lock();
            WaitForIdle();
            //Start UI
            dialogueUI.SetActive(true);
            NpcDialogue npcDialogue = messageReader.GetDialogue();
            dialogueUI.GetComponent<DialogueUI>().npcDialogue = npcDialogue;            
            dialogueUI.GetComponent<DialogueUI>().StartDialogue(gameObject);
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
        
        playerControl.ShowHidePlayer(true);
        dialogueUI.SetActive(false);
        playerCharacter.Unlock();
        yield return new WaitForSeconds(waitTime);
        //Desbloqueamos al player
        
    }
}
