using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    AudioSource _audioSource;
    public AudioClip _audioClip;
    public GameObject _spawnPoint;


    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerCharacter>().Lock();
            other.transform.position = _spawnPoint.transform.position;
            _audioSource.PlayOneShot(_audioClip);
            other.GetComponent<PlayerCharacter>().Unlock();
        }
    }
}
