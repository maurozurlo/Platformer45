using UnityEngine;
using System.Collections;

public class vPickupItem : MonoBehaviour
{
    AudioSource _audioSource;
    public AudioClip _audioClip;
    public BasicItem thisItem;
    public float speed = 40;
    //public GameObject _particle;


    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        thisItem.itemPos = this.transform.position;
    }

    void Update(){
        this.transform.Rotate(0, speed * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerInventory>().AddItem(thisItem);
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)            
                r.enabled = false;            

            _audioSource.PlayOneShot(_audioClip);
            Destroy(gameObject, _audioClip.length);
        }
    }
}