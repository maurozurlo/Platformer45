using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSFX : MonoBehaviour
{
    AudioSource AS;

    private void Start() {
        AS = this.GetComponent<AudioSource>();    
    }

    public void playSound(AudioClip clip){
        AS.PlayOneShot(clip);
    }

    public void stopSound(){
        AS.Stop();
    }
}
