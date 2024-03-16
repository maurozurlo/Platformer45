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
    //SaveGames
    [Header("SaveGames")]
    public Image[] SGimgUI;
    public Text[] SGtitleUI;
    public Text[] SGdescUI;
    public GameObject[] SGdelSaveUI;
    //Modal
    [Header("Modal")]
    public GameObject modal;
    public Text modalText;
    public Button yesButton;
    public Button noButton;

    public Toggle[] toggles;


    public void Awake(){
        fillUpSaveSlots();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
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

    public void fillUpSaveSlots(){
        for (int i = 1; i <= 4; i++)
        {
            if(SaveLoadManager.checkIfSaveExists(i)){
            PlayerData data = SaveLoadManager.LoadPlayer(i);
            SGtitleUI[i-1].text =  "Lives: " + data.amountOfLives.ToString();
            SGdescUI[i-1].text = "Percentage completed: " + gameControl.control.returnPercentageCompleted(data.questsCompleted.Count);
            }
            SGimgUI[i-1].GetComponent<Button>().interactable = SaveLoadManager.checkIfSaveExists(i);
            SGdelSaveUI[i-1].GetComponent<Button>().interactable = SaveLoadManager.checkIfSaveExists(i);
        }
    }

    public void LoadSaveGame(int slot){
        PlayerData data = SaveLoadManager.LoadPlayer(slot);
        gameControl.control.Load(data);
        SceneManager.LoadScene(gameControl.control.sceneIndex);
    }

    public void openModal(int slot){
        modal.SetActive(true);
        string a = "¿Estas seguro de que queres eliminar la partida guardada en el espacio " + slot.ToString() + "?";
        modalText.text = a;
         Button btn = yesButton.GetComponent<Button>(); //or just drag-n-drop the button in the CustomButton field
         btn.GetComponent<ModalButton>().slot = slot;
         btn.onClick.AddListener(modalButton_onClick);  //subscribe to the onClick event
    }



     //Handle the onClick event
     void modalButton_onClick()
     {
         DeleteSaveSlot(yesButton.GetComponent<ModalButton>().slot);
         closeModal();
     }

    public void closeModal(){
        modal.SetActive(false);
        fillUpSaveSlots();
    }

    public void DeleteSaveSlot(int slot){
        SaveLoadManager.DeleteSave(slot);
    }

    public void ChangeLanguage(GameObject toggle)
    {
        I18nManager t = I18nManager.control;
        I18nManager.Language language = GetLang(toggle.GetComponentInChildren<Text>().text);
        if (language == I18nManager.control.currentLanguage) return;
        DisableToggles();
        toggle.GetComponent<Toggle>().isOn = true;
        t.currentLanguage = language;

        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject button in buttons)
        {
            MenuButton menuButton = button.GetComponent<MenuButton>();
            if (menuButton == null) return;
            button.GetComponent<MenuButton>().UpdateText();
        }
    }

    void DisableToggles()
    {
        foreach (Toggle toggle in toggles)
        {
            toggle.isOn = false;
        }
    }

    I18nManager.Language GetLang(string v)
    {
		switch (v)
		{
			case "Español":
				return I18nManager.Language.sp;
			case "English":
				return I18nManager.Language.en;
			default:
				break;
		}

		return I18nManager.Language.sp;
	}


}
