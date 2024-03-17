using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I18nManager : MonoBehaviour
{
    public enum Language
    {
        en,
        sp
    }

    public Language currentLanguage;

    public static I18nManager control;
    CSVReader csvReader;

	private void Awake()
	{
        if (!control)
        {
            control = this;
            DontDestroyOnLoad(gameObject);
            csvReader = GetComponent<CSVReader>();
        }
        else
        {
            DestroyImmediate(gameObject);
        }
	}

    public string GetValue(string id, string fallback)
    {
        try
        {
            string text = csvReader.GetValueFromId(currentLanguage.ToString(), id);
            return text.Trim().Length == 0 ? fallback : text;
        }
        catch
        {
            return fallback;
        }
    }
}
