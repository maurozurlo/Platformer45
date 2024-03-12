using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStartDialogue : MonoBehaviour
{
    
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<NPCInteraction>().StartConversationWrapper(player);
    }

}
