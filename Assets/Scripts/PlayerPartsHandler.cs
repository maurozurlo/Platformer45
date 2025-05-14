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
    bool isTransition = false;

    PlayerCharacter player;

    public GameObject playerHandR;
    public GameObject playerHandL;

    private void Start() {
        headStartPos = skull.transform.localPosition;    
        headStartRotation = skull.transform.localRotation;
        player = GetComponent<PlayerCharacter>();
    }
    // Update is called once per frame
    void Update()
    {
        if (player.state == PlayerCharacter.STATES.LOCKED) return;
//        if (isTransition) return;

        if(Input.GetKeyDown(KeyCode.E)){
            if(!isPlayerNotComplete){
                DetachHead();
            }else{
                AttachHead();
            }
        }
    }

    void DetachHead(){
        isTransition = true;
        //Lockear player
        GetComponent<PlayerCharacter>().Lock();
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
        StartCoroutine(nameof(DetachHeadRoutine));
    }
    void AttachHead(){
        //Lockear player
        GetComponent<PlayerCharacter>().Lock();
        //desactivar roll head
        skull.GetComponent<RollHead>().enabled = false;
        //Desactivar camara
        CamSwitcher _camSwitcher = GetComponent<CamSwitcher>();
        _camSwitcher.SwitchTo(CamSwitcher.CameraType.Main);
        _camSwitcher.LockSwitching(true);
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
        GetComponent<PlayerCharacter>().Unlock();
        isPlayerNotComplete = false;
    }

    IEnumerator DetachHeadRoutine(){
        yield return new WaitForSeconds(1);
        CamSwitcher _camSwitcher = GetComponent<CamSwitcher>();
        _camSwitcher.SwitchTo(CamSwitcher.CameraType.Skull);
        _camSwitcher.LockSwitching(false);
        //Activar camSwitcher
        skull.GetComponent<RollHead>().enabled = true;
        //Activar rollHead;
        this.GetComponent<PlayerCharacter>().Unlock();
        //Desbloquear player
                //Sacar el parent de skull
        skull.transform.parent = null;
        isPlayerNotComplete = true;
        isTransition = false;
    }
}
