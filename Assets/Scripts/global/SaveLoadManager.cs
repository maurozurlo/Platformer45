using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadManager
{
    public static void SavePlayer(gameControl playerData, int slot)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "player" + slot.ToString() + ".sav", FileMode.Create);

        PlayerData data = new PlayerData(playerData);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer(int slot)
    {
        string path = Application.persistentDataPath + "player" + slot.ToString() + ".sav";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = bf.Deserialize(stream) as PlayerData;

            stream.Close();
            return data;
        }
        return null;
    }

    public static void DeleteSave(int slot){
        string path = Application.persistentDataPath + "player" + slot.ToString() + ".sav";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static bool checkIfSaveExists(int slot){
        string path = Application.persistentDataPath + "player" + slot.ToString() + ".sav";
        if (File.Exists(path))
        {
            return true;
        }
        return false;
    }


}

[Serializable]
public class PlayerData
{
    public int savePoint;
    public List<int> questsCompleted;
    public List<BasicItem> inventory;
    public List<BasicItem> currentItemsOnMap;
    public int amountOfLives;
    //Otras cosas seguramente...

    public PlayerData(gameControl gControl)
    {
        savePoint = gControl.savePoint;
        questsCompleted = gControl.completedQuests;
        inventory = gControl.inventory;
        amountOfLives = gControl.amountOfLives;
        //currentItemsOnMap;
    }
}

