using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameControl : MonoBehaviour
{
    public static gameControl control;
    public List<BasicItem> itemPersistance;
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

    void saveItemPositions()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Item"))
        {
            itemPersistance.Add(item.GetComponent<vPickupItem>().thisItem);
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

    public void restartLevel()
    {
        destroyAllItems();
        restoreItemPositions();
    }


    void destroyAllItems()
    {
        GameObject[] itemInstances = GameObject.FindGameObjectsWithTag("Item");
        for (int i = 0; i < itemInstances.Length; i++)
        {
            Destroy(itemInstances[i]);
        }
    }
}
