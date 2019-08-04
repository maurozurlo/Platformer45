using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camSwitcher : MonoBehaviour
{
    public Camera mainCamera;
    public Camera skullCamera;
    public bool isActive;
    bool activeCam;
    
    void Update(){
        if(isActive){
            if(Input.GetKeyDown(KeyCode.C))
            if(!activeCam)
            ChangeCamera(mainCamera, skullCamera);           
            else
            ChangeCamera(skullCamera,mainCamera);    
        }
    }

    public void ChangeCamera(Camera cam1, Camera cam2){
        cam2.depth = 1;
        cam1.depth = -1;
        activeCam = !activeCam;
    }
}
