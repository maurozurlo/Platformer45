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
            mainCamera.depth = 10;
            skullCamera.depth = -10;
        }
        if(Input.GetKeyDown(KeyCode.Keypad3)){
            skullCamera.depth = 10;
            mainCamera.depth = -10;
        }
        }
    }
}
