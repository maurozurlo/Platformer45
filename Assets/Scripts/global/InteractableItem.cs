using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public WORLD_ITEM ItemType;
    public bool ShouldLockPlayerInPlace;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ShouldLockPlayerInPlace)
            {
                other.GetComponent<PlayerCharacter>().LockPlayerInPlace();
            }
            other.GetComponent<PlayerWorldItemInteraction>().TriggerInteraction(ItemType);
        }
    }
}
