﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerCharacter>().BeKilledInstantly();
            other.GetComponent<AnimHandler>().TriggerAnim("drown");
        }
    }
}
