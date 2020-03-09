using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Superpow;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;


public class DictionaryDialog : Dialog
{
    public bool includeRelated;
    public bool useCanonical;
    public bool includeTags;
    public string limit;
    public Text textWordName;
    public Text textMean;
    public GameObject buttonClone;
    public Transform groupWord;

    //string wordValid;
    string sourceDictionaries;
    string keyApi;
    string url;
    public static Dictionary<string, List<WordData>> wordsDict = new Dictionary<string, List<WordData>>();
    List<WordData> listMeanWord = new List<WordData>();
    WordData wordData;
    
    private int passWorld, passSubWorld, passLevel;
    private GameLevel gameLevel;
    string [] listWordPassed;
    string wordPassed;

    protected void Start()
    {
        base.Start();
        GetWordPassed();
    }



    public void GetWordPassed()
    {

        passWorld = Prefs.unlockedWorld - 1 < 0 ? 0: Prefs.unlockedWorld-1 ;
        passSubWorld = Prefs.unlockedSubWorld-1 < 0? 0 : Prefs.unlockedSubWorld - 1;
        passLevel = Prefs.unlockedLevel-1;
        
        if (passLevel >=0)
        {
            for(int i=0; i<=passWorld; i++)
            {
                for(int j=0; j<= passSubWorld; j++)
                {
                    for(int k=0; k<=passLevel; k++)
                    {
                        gameLevel = Utils.Load(i, j, k);
                        wordPassed += gameLevel.answers;
                    }
                }
            }
        }
        Debug.Log(wordPassed);
        if (wordPassed!=null)
            listWordPassed = wordPassed.Split('|');
    }

    public void GetDataFromApi(string word)
    {
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
        JArray arrayJson = JArray.Parse(text);
        for (int i = 0; i < arrayJson.Count; i++)
        {
            wordData = JsonConvert.DeserializeObject<WordData>(arrayJson[i].ToString());
            listMeanWord.Add(wordData);
        }

        if(!wordsDict.ContainsKey(word))
            wordsDict.Add(word, listMeanWord);
    }

    public void ShowWordData()
    {
        //GetDataFromApi();
        foreach (KeyValuePair<string, List<WordData>> word in wordsDict)
        {
            textWordName.text = word.Key;
            foreach (WordData wordMean in word.Value)
            {
                textMean.text += "(" + wordMean.partOfSpeech + ") " + wordMean.text.Replace("<xref>", "").Replace("</xref>", "") + "\n";
            }
        }
    }

    public void CloneButtonWord()
    {
        int numGroupWord;
        if (listWordPassed.Length < 3)
        {
            
        }
    }

}

