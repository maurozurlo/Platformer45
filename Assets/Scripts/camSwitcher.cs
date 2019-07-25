using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camSwitcher : MonoBehaviour
{
    public Camera mainCamera;
    public Camera skullCamera;
    public bool isActive;
    
    void Update(){
        if(isActive){
        if(Input.GetKeyDown(KeyCode.Keypad1)){
         ChangeCamera(mainCamera, skullCamera);           
        }
        if(Input.GetKeyDown(KeyCode.Keypad3)){
            ChangeCamera(skullCamera,mainCamera);    
        }
        }
    }

    public void ChangeCamera(Camera cam1, Camera cam2){
        cam2.depth = 10;
        cam1.depth = -10;
    }
}
