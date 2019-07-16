using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimHandler : MonoBehaviour
{
    Animator anim;
    public Camera killCamera;

    private void Start() {
     anim = this.GetComponent<Animator>();   
    }
    
    public void TriggerAnim(string animation){
        anim.SetTrigger(animation);
        killCamera.depth = 3;
    }
}
