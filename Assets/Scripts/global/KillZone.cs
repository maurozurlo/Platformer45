using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public WORLD_ITEM KillZoneType = WORLD_ITEM.killing_fluid;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerCharacter>().BeKilledInstantly();
            other.GetComponent<PlayerWorldItemInteraction>().TriggerInteraction(KillZoneType);
        }
    }
}
