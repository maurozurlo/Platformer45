﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameControl : MonoBehaviour
{
    public static gameControl control;
    public int amountOfLives = 2;
    public int savePoint;
    public int questQuantity = 10;
    public int sceneIndex = 1;

    public List<BasicItem> inventory;
    GameObject player;

    public struct CompletedQuest{
        public int id;

        public CompletedQuest(int newID){
            id = newID;
        }
    }
    public List<int> completedQuests;
    
    void Awake()
    {
        if (control == null)
            control = this;
        DontDestroyOnLoad(this.gameObject);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    //Quests
    public void AddCompletedQuest(int questID){
        completedQuests.Add(questID);
    }

    public bool checkIfQuestIsCompleted(int questID){
        if(completedQuests.Count > 0){
         foreach (int item in completedQuests)
            {
                 if(item == questID)
                 return true;
             }
         }
        
        return false;
    }

    //Save

    public void Save(int slot){
        SaveLoadManager.SavePlayer(control,slot);
    }

    public void Load(PlayerData data){
        //Asignar cosas
        amountOfLives = data.amountOfLives;
        completedQuests = data.questsCompleted;
        inventory = data.inventory;
        savePoint = data.savePoint;
    }

    public string returnPercentageCompleted(int completedQuests){
        float a = completedQuests * 100 / questQuantity;
        return a + "%";
    }

    public GameObject GetPlayerReference()
    {
        if (player) return player;
        Debug.LogError("Player was requested but not found");
        return null;
    }
}
