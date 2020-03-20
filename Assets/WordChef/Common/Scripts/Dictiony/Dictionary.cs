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
    private string SAVE_FOLDER;
    public Dictionary<string, string> dictWordSaved;
    
    public bool includeRelated;
    public bool useCanonical;
    public bool includeTags;
    public string limit;
    
    string sourceDictionaries = "wiktionary";
    string keyApi = "l7bgsd9titsbw82vtpzparyfrmt9yg2hibbeihv5uex8e5maa";
    string url;
    WordData wordData;
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
    
    public void GetDataFromApi(string word)
    {
        string meaning = "";
        //sourceDictionaries = "wiktionary";
        //keyApi = "l7bgsd9titsbw82vtpzparyfrmt9yg2hibbeihv5uex8e5maa";
        url = "https://api.wordnik.com/v4/word.json/" + word
                                                      + "/definitions?limit=" + limit
                                                      + "&includeRelated=" + includeRelated
                                                      + "&sourceDictionaries=" + sourceDictionaries
                                                      + "&useCanonical=" + useCanonical
                                                      + "&includeTags=" + includeTags
                                                      + "&api_key=" + keyApi;
        
        var client = new WebClient();
        var text = client.DownloadString(url);
        if (text != null || text != "")
        {
            JArray arrayJson = JArray.Parse(text);
            for (int i = 0; i < arrayJson.Count; i++)
            {
                wordData = JsonConvert.DeserializeObject<WordData>(arrayJson[i].ToString());

                //listMeanWord.Add(wordData);

                meaning +=(i+1)+ ". (" + wordData.partOfSpeech + ") " + wordData.text.Replace("<xref>", "").Replace("</xref>", "") + "\n";
            }
        }
        else
        {
            meaning = "Can't get data, please check your wifi";
        }
        
        //return meaning;
        //Debug.Log(word);
        //Debug.Log(meaning);
        //MeanDialog.wordName = word;
        //MeanDialog.wordMean = meaning.ToString();
        SaveWord(word, meaning.ToString());
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



