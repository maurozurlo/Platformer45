using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelManager : MonoBehaviour
{
    public static levelManager control;
    public List<BasicItem> itemPersistance;
    public List<SavePoint> savePoints;
    public GameObject[] itemPrefabs;
    public GameObject player;
    
    void Awake()
    {
        if (control == null)
            control = this;
        saveSavePointPositions();
    }

    void Start()
    {
        //saveItemPositions();
        spawnPlayerOnSavePoint(gameControl.control.savePoint);
    }

    public void restartLevel()
    {
        destroyAllItems();
        restoreItemPositions();
    }

    ///ITEMS
    void saveItemPositions()
    {
        Debug.LogError("NOT IMPLEMENTED");
        /* foreach (var item in GameObject.FindGameObjectsWithTag("Item"))
        {
            BasicItem itemToClone = item.GetComponent<vPickupItem>().thisItem;
            BasicItem itemToSave = new BasicItem(itemToClone.label, itemToClone.id, itemToClone.amount, itemToClone.itemPos, false);
            itemPersistance.Add(itemToSave);
        } */
    }

    void restoreItemPositions()
    {
        //Debug.LogError("NOT IMPLEMENTED");
        /*
        foreach (BasicItem item in itemPersistance)
        {
            GameObject _item = Instantiate(itemPrefabs[item.id], item.itemPos, Quaternion.identity) as GameObject;
            _item.GetComponent<vPickupItem>().thisItem = item;
        }*/
    }

    void destroyAllItems()
    {
        //Debug.LogError("NOT IMPLEMENTED");
        /*
        GameObject[] itemInstances = GameObject.FindGameObjectsWithTag("Item");
        for (int i = 0; i < itemInstances.Length; i++)
        {
            Destroy(itemInstances[i]);
        }*/
    }

    ///ITEMS
    void saveSavePointPositions()
    {   
        foreach (var item in GameObject.FindGameObjectsWithTag("SavePoint"))
        {
            savePoints.Add(item.GetComponent<SavePoint>());
        }
    }

    public void spawnPlayerOnSavePoint(int savePointID){
        Vector3 spawnPoint = Vector3.zero;

        for (int i = 0; i < savePoints.Count; i++)
        {
            if(savePoints[i].savePointID == savePointID)
            spawnPoint = savePoints[i].transform.position;
        }
        if (savePointID != -1 && spawnPoint != Vector3.zero)
        player.transform.position = spawnPoint;
    }

}
