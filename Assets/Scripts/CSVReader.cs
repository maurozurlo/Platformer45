using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class CSVReader : MonoBehaviour
{
    public List<string> filePaths = new List<string>();
    private Dictionary<string, Dictionary<string, string>> csvData = new Dictionary<string, Dictionary<string, string>>(); 

    public void ReadCSVs()
    {
        csvData.Clear();

        foreach (string filePath in filePaths)
        {
            try
            {
                TextAsset csv = Resources.Load<TextAsset>(filePath);
                string fileName = filePath.Split(new string[] { "Dialogue/" }, StringSplitOptions.None)[1];
                csvData[fileName] = new Dictionary<string, string>();
                string[] lines = csv.text.Split(new string[] { "\n" }, StringSplitOptions.None );

                foreach (string line in lines)
                {
                    string[] parts = line.Split(new char[] { ',' }, 2);
                    if (parts.Length == 2)
                    {
                        csvData[fileName][parts[0]] = parts[1];
                    }
                }
            }
            catch
            {
                Debug.LogError("File not found: " + filePath);
            }
        }
    }

    public string GetValueFromId(string fileName, string id)
    {
        if (csvData.ContainsKey(fileName))
        {
            Dictionary<string, string> fileData = csvData[fileName];
            if (fileData.ContainsKey(id))
            {
                return fileData[id];
            }
            else
            {
                Debug.LogWarning("ID not found in CSV data: " + id);
            }
        }
        else
        {
            Debug.LogWarning("File not found in CSV data: " + fileName);
        }

        return string.Empty;
    }

	public void Awake()
	{
        ReadCSVs();
	}
}