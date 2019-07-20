using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestManager : MonoBehaviour
{
    //UI
    public Text QuestUI;

    public List<Quest> questDb;
    public GameObject player;

    //currentQuest
    public int currentQuestID;
    public Quest currentQuest;

    //SFX
    public AudioClip missionCompleted;
    public AudioClip missionFailed;

    
    private void Start() {
        
        player = GameObject.FindGameObjectWithTag("Player");
        if(player == null){
            Debug.LogError("Player couldn't be found");
        }
    }

    public void StartQuest(int quest){
        //El default sin quest es -1
        if(currentQuest == null){
            //Seteamos la quest
            currentQuestID = quest;
            currentQuest = questDb[quest];
            QuestUI.text = "Active quest: " + questDb[quest].QuestName;
        }
    }

    public void EndQuest(int quest){
        //Chequear si se necesitaban items
        if(questDb[quest].QuestType == Quest.questType.itemHunt){
            //Sacar items
            foreach (QuestItem item in questDb[quest].itemHuntDB)
            {
               player.GetComponent<PlayerInventory>().removeItem(item.itemId,item.amount);
            }
        }
        questDb[quest].isCompleted = true;
        currentQuest = null;
        currentQuestID = -1;
        QuestUI.text = "Active quest: None";
        //Avisar al pibe que ya está
        this.GetComponent<GeneralMessageUI>().DisplayMessage("LA RE HICISTE AMEO",6,"top");
        this.GetComponent<PlayerCharacter>().sfx.GetComponent<GeneralSFX>().playSound(missionCompleted);
        //Guardar que la quest se completo
        gameControl.control.AddCompletedQuest(quest);
    }

    public int checkQuestStatus(){
        //Status info
        //1: Player doesn't have the item
        //2: Player doesn't have required amount
        //3: Player has everything, quest completed

        //Chequear si el usuario tiene los items que se necesitan
        //Cambiar esto...
        List<QuestItem> currentQuestItems = questDb[currentQuestID].itemHuntDB;

            foreach (BasicItem item in gameControl.control.inventory)
            {
                //Por ahora solo chequeamos el primer item de la BD de la Quest, dps habria que chequear todos
                if(item.id == questDb[currentQuestID].itemHuntDB[0].itemId)
                {
                    //Chequear si tambien tiene la cantidad necesaria
                    if (item.amount >= questDb[currentQuestID].itemHuntDB[0].amount)
                    {
                        //Quest completed
                        EndQuest(currentQuestID);
                        return 3;
                    }else{
                        //Player no tenia la cantidad necesaria
                        return 2;
                    }
                }
            }
            //Player no tenia el item
            return 1;
    }

}
