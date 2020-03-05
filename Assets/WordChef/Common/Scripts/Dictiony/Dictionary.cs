using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;



public class Dictionary : MonoBehaviour
{
    public string wordValid;
    public bool includeRelated;
    public string sourceDictionaries;
    public bool useCanonical;
    public bool includeTags;
    public string limit;
    public Text textWordName;
    public Text textMean;

    string keyApi;
    string url;
    Dictionary<string, List<WordData>> wordsDict= new Dictionary<string, List<WordData>>();
    List<WordData> listMeanWord = new List<WordData>();
    WordData wordData;

    private void Start()
    {
        sourceDictionaries = "wiktionary";
        keyApi = "l7bgsd9titsbw82vtpzparyfrmt9yg2hibbeihv5uex8e5maa";
        url = "https://api.wordnik.com/v4/word.json/" + wordValid
            + "/definitions?limit=" + limit
            + "&includeRelated=" + includeRelated
            + "&sourceDictionaries=" + sourceDictionaries
            + "&useCanonical=" + useCanonical
            + "&includeTags=" + includeTags
            + "&api_key=" + keyApi;
    }

    public void GetData()
    {
        char[] s = { '[', ']' };
        var client = new WebClient();
        var text = client.DownloadString(url);
        JArray arrayJson = JArray.Parse(text);
        for(int i=0; i < arrayJson.Count; i++)
        {
            wordData = JsonConvert.DeserializeObject<WordData>(arrayJson[i].ToString());
            listMeanWord.Add(wordData);
        }

        wordsDict.Add(wordValid,listMeanWord);
    }

    public void ShowWordData()
    {
        GetData();
          foreach (KeyValuePair<string, List<WordData>> word in wordsDict)
        {
            textWordName.text = word.Key;
            foreach(WordData wordMean in  word.Value)
            {
                textMean.text += wordMean.text + ", ";
            }
        }
    }
}

public class WordData 
{
    public string partOfSpeech;
    public string text;
    
}

