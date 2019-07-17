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
        anim.StopPlayback();
        anim.SetBool("IsGrounded",true);
        anim.SetTrigger(animation);
        killCamera.depth = 3;
    }

    public void resetCamera(){
        killCamera.depth = -3;
    }
}
