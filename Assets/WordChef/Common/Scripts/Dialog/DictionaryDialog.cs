using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Superpow;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Linq;
using TMPro;
using System.IO;


public class DictionaryDialog : Dialog
{
    public bool includeRelated;
    public bool useCanonical;
    public bool includeTags;
    public string limit;

    public GameObject buttonWord;
    public GameObject groupWord;
    public GameObject listGroupWord;
    public Transform content;
    public static DictionaryDialog instance;
    




    //string wordValid;
    //List<WordData> listMeanWord = new List<WordData>();
    string meaning;
    string sourceDictionaries;
    string keyApi;
    string url;
    WordData wordData;
    Dictionary<string, string> wordDiction =new Dictionary<string, string>();
    Dictionary<string, List<string>> groupWordDiction=new Dictionary<string, List<string>>();
    Dictionary<string, List<string>> dataGroupWordDiction= new Dictionary<string, List<string>>();
    char[] keys;
    List<string> defaultValue = new List<string>();
    List<string> listWordPassed;
    //static readonly string SAVE_FOLDER = Application.dataPath + "/saves/";
    //Dictionary dict;

    [HideInInspector]
    public static string wordPassed;


    protected override void Awake()
    {
        instance = this;
        
    }

    protected override void Start()
    {
        base.Start();
        keys = "ABCDFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        foreach (char key in keys)
        {
            groupWordDiction.Add(key.ToString(), defaultValue);
        }
        GetWordPassed();
        if (listWordPassed != null)
            CloneListGroupWord();
    }



    public void GetWordPassed()
    {
        wordPassed = CPlayerPrefs.GetString("WordLevelSave");
        Debug.Log(wordPassed);
        if (wordPassed != null)
        {
            listWordPassed = wordPassed.Split('|').OfType<string>().ToList<string>();
            listWordPassed.Sort();
            listWordPassed.RemoveAt(0);
            foreach(string word in listWordPassed)
            {
                char[] charWord = word.ToCharArray();
                wordDiction.Add(word, char.ToUpper(charWord[0]).ToString());
            }

             dataGroupWordDiction= wordDiction.GroupBy(r => r.Value).ToDictionary(t =>t.Key, t => t.Select(r => r.Key).ToList());

            foreach (var item in dataGroupWordDiction)
            {
                //Debug.Log(item.Key);
                groupWordDiction[item.Key] = item.Value;
            }
        }
    }

    public void GetDataFromApi(string word)
    {
        meaning = "";
        sourceDictionaries = "wiktionary";
        keyApi = "l7bgsd9titsbw82vtpzparyfrmt9yg2hibbeihv5uex8e5maa";
        url = "https://api.wordnik.com/v4/word.json/" + word
            + "/definitions?limit=" + limit
            + "&includeRelated=" + includeRelated
            + "&sourceDictionaries=" + sourceDictionaries
            + "&useCanonical=" + useCanonical
            + "&includeTags=" + includeTags
            + "&api_key=" + keyApi;
        
        var client = new WebClient();
        var text = client.DownloadString(url);
        if (text != null)
        {
            JArray arrayJson = JArray.Parse(text);
            for (int i = 0; i < arrayJson.Count; i++)
            {
                wordData = JsonConvert.DeserializeObject<WordData>(arrayJson[i].ToString());

                //listMeanWord.Add(wordData);

                meaning += "(" + wordData.partOfSpeech + ") " + wordData.text.Replace("<xref>", "").Replace("</xref>", "") + "\n";
            }
        }
        else
        {
            meaning = "Can't get data, please check your wifi";
        }
        //return meaning;
        Debug.Log(word);
        Debug.Log(meaning);
        MeanDialog.wordName = word;
        MeanDialog.wordMean = meaning.ToString();
        Dictionary.instance.SaveWord(word, meaning.ToString());
        //Debug.Log(MeanDialog.wordName);
        //Debug.Log(MeanDialog.wordMean);
    }


    public void CloneListGroupWord()
    {
        GameObject listGroupWordClone;
        GameObject buttonWordClone;

        foreach ( KeyValuePair<string, List<string>> item in  groupWordDiction)
        {
            listGroupWordClone = GameObject.Instantiate(listGroupWord, content.transform);
            listGroupWordClone.transform.Find("Button").Find("FirstLetter").GetComponent<TextMeshProUGUI>().text = item.Key;
            foreach(var word in item.Value)
            {
                //Debug.Log(item.Key + ": " + word);
                buttonWordClone = GameObject.Instantiate(buttonWord, listGroupWordClone.GetComponent<ListGroupWord>().groupWord);
                buttonWordClone.transform.GetChild(0).GetComponent<Text>().text = word;
            }
        }
   
    }


}




