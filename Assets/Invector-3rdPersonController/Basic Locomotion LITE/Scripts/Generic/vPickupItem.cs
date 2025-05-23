﻿using UnityEngine;
using System.Collections;

public class vPickupItem : MonoBehaviour
{
    AudioSource _audioSource;
    public AudioClip _audioClip;
    public BasicItem thisItem;
    public float speed = 40;
    bool pickedUp;
    //public GameObject _particle;


    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        thisItem.itemPos = transform.position;
    }

    void Update(){
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("PlayerSkull"))
        {
            if (pickedUp) return;
            pickedUp = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().AddItem(thisItem);
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)            
                r.enabled = false;            

            _audioSource.PlayOneShot(_audioClip);
            Destroy(gameObject, _audioClip.length);
        }
    }
}