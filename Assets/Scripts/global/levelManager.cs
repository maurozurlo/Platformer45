using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelManager : MonoBehaviour
{
    public static levelManager control;
    public List<BasicItem> itemPersistance;
    public List<savePoint> savePoints;
    public GameObject[] itemPrefabs;
    
    void Awake()
    {
        if (control == null)
            control = this;
        saveSavePointPositions();
    }

    void Start()
    {
        saveItemPositions();
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
        foreach (var item in GameObject.FindGameObjectsWithTag("Item"))
        {
            BasicItem itemToClone = item.GetComponent<vPickupItem>().thisItem;
            BasicItem itemToSave = new BasicItem(itemToClone.label, itemToClone.id, itemToClone.amount, itemToClone.itemPos, false);
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

    ///ITEMS
    void saveSavePointPositions()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("SavePoint"))
        {
            savePoints.Add(item.GetComponent<savePoint>());
        }
    }

    public void spawnPlayerOnSavePoint(int savePointID){
        Vector3 spawnPoint = Vector3.zero;

        for (int i = 0; i < savePoints.Count; i++)
        {
            if(savePoints[i].savePointID == savePointID)
            spawnPoint = savePoints[i].transform.position;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(savePointID != -1 && spawnPoint != Vector3.zero)
        player.transform.position = spawnPoint;
    }

}
