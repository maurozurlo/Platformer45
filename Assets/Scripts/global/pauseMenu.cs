using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    public static bool gameIsPaused;
    public GameObject _pauseMenu;
    CursorLockMode clm;
    bool cursorVisible;

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(!gameIsPaused){
                Pause();
            }else
            {
                Resume();
            }
        }    
    }

    public void Resume(){
        //Devolver cursor a como estaba antes
        Cursor.lockState = clm;
        Cursor.visible = cursorVisible;

        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    void Pause(){
        //Guardar cursor actual
        clm = Cursor.lockState;
        cursorVisible = Cursor.visible;

        //Activar cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //
        _pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void GoBackToMenu(){
        Time.timeScale = 1;
        gameIsPaused = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
