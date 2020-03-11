using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Linq;
using TMPro;
using System.IO;


public class Dictionary: MonoBehaviour
{
    public static Dictionary instance;
    public static string SAVE_FOLDER;
    void Awake()
    {
        if (instance == null)
            instance = this;
        SAVE_FOLDER = Application.dataPath + "/Saves/";
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public void SaveWord(string _name, string _mean)
    {
        Debug.Log("save: " + _name);
        Debug.Log("save: " + _mean);
        //List<string> listJson = new List<string>();
        WordSave word = new WordSave
        {
            name = _name,
            mean = _mean
        };
        string json = JsonConvert.SerializeObject(word) + "|";
        if (!File.Exists(SAVE_FOLDER + "saveword.txt"))
            File.WriteAllText (SAVE_FOLDER+ "saveword.txt", json);
        else
        {
            File.AppendAllText(SAVE_FOLDER + "saveword.txt", json);
        }

    }
}


public class WordSave
{
    public string name;
    public string mean;
}



