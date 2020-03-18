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
    public string SAVE_FOLDER;
    public Dictionary<string, string> dictWordSaved;
    void Awake()
    {
        if (instance == null)
            instance = this;

        SAVE_FOLDER = Application.persistentDataPath + "/Saves/";
        Debug.Log(Application.persistentDataPath);
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
       
    }
    private void Start()
    {
        dictWordSaved = new Dictionary<string, string>();
        if (dictWordSaved != null)
            load();
    }
    //void Update()
    //{
    //    if (dictWordSaved != null)
    //        load();
    //}

    public void SaveWord(string _name, string _mean)
    {
        if (!dictWordSaved.ContainsKey(_name))
        {
            WordSave word = new WordSave
            {
                name = _name,
                mean = _mean
            };
            string json = JsonConvert.SerializeObject(word) + "|";
            if (!File.Exists(SAVE_FOLDER + "saveword.txt"))
                File.WriteAllText(SAVE_FOLDER + "saveword.txt", json);
            else
            {
                File.AppendAllText(SAVE_FOLDER + "saveword.txt", json);
            }
        }
        load();
      

    }
    public void load()
    {
        Debug.Log("loading words..");
        if(File.Exists(SAVE_FOLDER + "saveword.txt"))
        {
            string text = File.ReadAllText(SAVE_FOLDER + "saveword.txt");
            List<string> listWordSaved = text.Split('|').OfType<string>().ToList<string>();
            listWordSaved.RemoveAt(listWordSaved.Count-1);
            WordSave wordSave;
            foreach (string word in listWordSaved)
            {
                wordSave = JsonConvert.DeserializeObject<WordSave>(word);
                if (!CheckWExistInDictWordSaved(wordSave.name))
                {
                    dictWordSaved.Add(wordSave.name, wordSave.mean);
                }
                
            }
        }
            
    }
    public bool CheckWExistInDictWordSaved(string word)
    {
        if (dictWordSaved.ContainsKey(word))
            return true;
        return false;
    }

}


public class WordSave
{
    public string name;
    public string mean;
}



