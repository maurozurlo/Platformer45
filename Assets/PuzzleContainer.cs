using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleContainer : MonoBehaviour
{
    Animation anim;
    AnimationClip openHalf, openFull;
    AudioSource AS;
    public AudioClip openHalfSFX, openFullSFX;

    GameObject item;

    public enum ContainerStates
    {
        idle, half, open
    }

    public ContainerStates state = ContainerStates.idle;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        AS = GetComponent<AudioSource>();
        openHalf = anim.GetClip("open_1");
        openFull = anim.GetClip("open_2");
    }

    

    public void OpenContainer()
    {
        switch (state)
        {
            case ContainerStates.idle:
                anim.clip = openHalf;
                state = ContainerStates.half;
                AS.PlayOneShot(openHalfSFX);
                break;
            case ContainerStates.half:
                anim.clip = openFull;
                state = ContainerStates.open;
                AS.PlayOneShot(openFullSFX);
                EnablePickup();
                break;
            case ContainerStates.open:
                Debug.LogError("Something weird happened in a container");
                break;
        }

        anim.Play();
        }

    void EnablePickup()
    {
        item = GetComponentInChildren<vPickupItem>().gameObject;
        item.GetComponent<SphereCollider>().isTrigger = true;
    }
}
