using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameControl : MonoBehaviour
{
    public static gameControl control;
    public List<BasicItem> itemPersistance;
    public struct CompletedQuest{
        public int id;

        public CompletedQuest(int newID){
            id = newID;
        }
    }
    public List<int> completedQuests;

    public GameObject[] itemPrefabs;
    
    void Awake()
    {
        if (control == null)
            control = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        saveItemPositions();
    }


    public void restartLevel()
    {
        destroyAllItems();
        restoreItemPositions();
    }


    ///ITEMS
    void saveItemPositions()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Item"))
        {
            BasicItem itemToClone = item.GetComponent<vPickupItem>().thisItem;
            BasicItem itemToSave = new BasicItem(itemToClone.label,itemToClone.id,itemToClone.amount,itemToClone.itemPos);
            itemPersistance.Add(itemToSave);
        }
    }

    void restoreItemPositions()
    {
        foreach (BasicItem item in itemPersistance)
        {
            GameObject _item = Instantiate(itemPrefabs[item.id], item.itemPos, Quaternion.identity) as GameObject;
            _item.GetComponent<vPickupItem>().thisItem = item;
        }
    }

    void destroyAllItems()
    {
        GameObject[] itemInstances = GameObject.FindGameObjectsWithTag("Item");
        for (int i = 0; i < itemInstances.Length; i++)
        {
            Destroy(itemInstances[i]);
        }
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
}
