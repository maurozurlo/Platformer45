using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public GameObject mainCanvas;
    public GameObject optionCanvas;
    public GameObject loadGameCanvas;
    public Sprite[] soundImg;
    public Image soundUI;
    bool loadSave, options;


    public void LoadSave(){
        loadSave = !loadSave;
        loadGameCanvas.SetActive(loadSave);
        mainCanvas.SetActive(!loadSave);
    }

    public void Options(){
        options = !options;
        optionCanvas.SetActive(options);
        mainCanvas.SetActive(!options);
    }

    public void StartGame(){
        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void SoundFX(){
        globalControl.control.activateSound = !globalControl.control.activateSound;
        if(globalControl.control.activateSound)
        soundUI.sprite = soundImg[0];
        else
        soundUI.sprite = soundImg[1];
    }
    
}
