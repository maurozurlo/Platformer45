using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPartsHandler : MonoBehaviour
{

    public GameObject skull;
    public GameObject headBone;
    Vector3 headStartPos;
    Quaternion headStartRotation;

    public static bool isPlayerNotComplete;

    private void Start() {
        headStartPos = skull.transform.localPosition;    
        headStartRotation = skull.transform.localRotation;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            if(!isPlayerNotComplete){
                DetachHead();
            }else{
                AttachHead();
            }
        }
    }

    void DetachHead(){
        //Lockear player
        this.GetComponent<PlayerCharacter>().Lock();
        //Sacar el parent de skull
        skull.transform.parent = null;
        //Activar SphereCollider
        skull.GetComponent<SphereCollider>().enabled = true;
        //Activar RB de la Head
        Rigidbody rb = skull.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;
        //Dar pequeña force desde abajo a la head
        rb.AddForce(Vector3.up * 50);
        //Wait for seconds 1s
        StartCoroutine("DetachHeadRoutine");
    }
    void AttachHead(){
        //Lockear player
        this.GetComponent<PlayerCharacter>().Lock();
        //desactivar roll head
        skull.GetComponent<rollHead>().enabled = false;
        //Desactivar camara
        camSwitcher _camSwitcher = this.GetComponent<camSwitcher>();
        _camSwitcher.isActive = false;
        _camSwitcher.ChangeCamera(_camSwitcher.skullCamera,_camSwitcher.mainCamera);
        //DesActivar SphereCollider
        skull.GetComponent<SphereCollider>().enabled = false;
        //DesActivar RB de la Head
        Rigidbody rb = skull.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.useGravity = false;
        //Place head
        skull.transform.parent = headBone.transform;
        skull.transform.localPosition = headStartPos;
        skull.transform.localRotation = headStartRotation;
        this.GetComponent<PlayerCharacter>().Unlock();
        isPlayerNotComplete = false;
    }

    IEnumerator DetachHeadRoutine(){
        yield return new WaitForSeconds(1);
        this.GetComponent<camSwitcher>().isActive = true;
        //Activar camSwitcher
        skull.GetComponent<rollHead>().enabled = true;
        //Activar rollHead;
        this.GetComponent<PlayerCharacter>().Unlock();
        //Desbloquear player
                //Sacar el parent de skull
        skull.transform.parent = null;
        isPlayerNotComplete = true;
    }
}
