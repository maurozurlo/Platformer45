using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public int currentQuestID = -1;
    public Quest currentQuest;

    //SFX
    public AudioClip missionCompleted;
    public AudioClip missionFailed;

    // Singleton
    public static QuestManager control;


    private void Awake()
    {
        if (!control)
        {
            control = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }
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
            foreach (Quest questItem in questDb)
            {
                if (questItem.QuestID == quest)
                {
                    currentQuest = questItem;
                    QuestUI.text = "Active quest: " + questItem.QuestName;
                }
            }
            
        }
    }

    public void EndQuest(){
        //Chequear si se necesitaban items
        if(currentQuest.QuestType == Quest.questType.itemHunt){
            //Sacar items
            foreach (QuestItem item in currentQuest.itemHuntDB)
            {
               player.GetComponent<PlayerInventory>().removeItem(item.itemId,item.amount);
            }
        }
        // Give player rewards
        foreach (BasicItem item in currentQuest.rewards)
        {
            player.GetComponent<PlayerInventory>().AddItem(item);
        }
        gameControl.control.AddCompletedQuest(currentQuestID);
        currentQuest = null;
        currentQuestID = -1;
        QuestUI.text = "Active quest: None";


        // Completed quest message
        player.GetComponent<GeneralMessageUI>().DisplayMessage("Quest completed",6,"top");
        player.GetComponent<PlayerCharacter>().sfx.GetComponent<GeneralSFX>().playSound(missionCompleted);
    }

    

    public bool IsQuestCompleted(){
        List<QuestItem> currentQuestItems = currentQuest.itemHuntDB;
            foreach (QuestItem item in currentQuestItems)
            {
                BasicItem playerItem = ItemInPlayerInventory(item.itemId);
                if (playerItem == null) return false; // Player doesn't have the item;
                if (playerItem.amount < item.amount) return false; // Player doesn't have enough
            }
        return true;
    }



    BasicItem ItemInPlayerInventory(int id)
    {
        foreach (BasicItem item in gameControl.control.inventory)
        {
            if (item.id == id) return item;
        }
        return null;
    }

    public string GetDialogueId(int[] npcQuests)
    {
        int _quest = currentQuestID;
        bool hasCompletedQuestNow = false;
        // 1. Check if player is currently on a quest
        if (currentQuestID != -1)
        {
            // There's a quest in process
            // Check Quest status
            if (!IsQuestCompleted())
            {
                string ongoing_id = DialogueUI.GetDialogId(DialogueUI.DialogType.value_ongoing, _quest);
                bool dialogExists = DialogueUI.control.DialogExists(ongoing_id);
                return dialogExists ? ongoing_id : DialogueUI.GetDialogId(DialogueUI.DialogType.common_ongoing);
            }
            // Complete Quest
            EndQuest();
            hasCompletedQuestNow = true;
        }
        // 2. Check if player has completed any of this NPC's quests
        List<int> completedQuests = gameControl.control.completedQuests;
        bool hasCompletedQuestsPreviously = completedQuests.Any(quest => npcQuests.Contains(quest));
        // 3. Player hasn't completed any of this NPCs quests, return intro
        if (!hasCompletedQuestsPreviously)
        {
            return DialogueUI.GetDialogId(DialogueUI.DialogType.common_intro);
        }
        List<int> NPCsCompletedQuests = completedQuests.Intersect(npcQuests).ToList();
        // 4. All quests are completed
        if (NPCsCompletedQuests.Count == npcQuests.Length)
        {
            return hasCompletedQuestNow ? 
                DialogueUI.GetDialogId(DialogueUI.DialogType.common_completed_all_first) 
                : DialogueUI.GetDialogId(DialogueUI.DialogType.common_completed_all);
        }
        // 5. Some quests are completed, but not all
        if (NPCsCompletedQuests.Count < npcQuests.Length)
        {
            int lastId = NPCsCompletedQuests.Last();
            return DialogueUI.GetDialogId(DialogueUI.DialogType.value_completed_some, lastId);
        }
        // 6. Why the fuck are we here
        Debug.LogError("Shouldnt have gotten here...");
        return null;
    }

    public class QuestCheck
    {
        public int id;
        public QuestStatus status;
    }

    public enum QuestStatus {
        ongoing,
        completed,
        notInQuest
    }

}
