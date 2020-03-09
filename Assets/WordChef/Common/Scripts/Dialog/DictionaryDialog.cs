using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Superpow;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Linq;


public class DictionaryDialog : Dialog
{
    public bool includeRelated;
    public bool useCanonical;
    public bool includeTags;
    public string limit;
    public Text textWordName;
    public Text textMean;
    public GameObject buttonWord;
    public GameObject groupWord;
    public Transform content;
    public static Dictionary<string, List<WordData>> wordsDict = new Dictionary<string, List<WordData>>();


    //string wordValid;
    string meaning;
    string sourceDictionaries;
    string keyApi;
    string url;
    List<WordData> listMeanWord = new List<WordData>();
    WordData wordData;
    
    
    private int passWorld, passSubWorld, passLevel;
    private GameLevel gameLevel;
    List<string> listWordPassed;
    string wordPassed;
    

    protected void Start()
    {
        base.Start();
        GetWordPassed();
        if(listWordPassed!=null)
            CloneGroupWord();
    }



    public void GetWordPassed()
    {

        passWorld = Prefs.unlockedWorld - 1 < 0 ? 0: Prefs.unlockedWorld-1 ;
        passSubWorld = Prefs.unlockedSubWorld-1 < 0? 0 : Prefs.unlockedSubWorld - 1;
        passLevel = Prefs.unlockedLevel-1;
        Debug.Log(passWorld.ToString() + passSubWorld.ToString() + passLevel.ToString());
        Debug.Log(Prefs.unlockedWorld.ToString() + Prefs.unlockedWorld.ToString() + Prefs.unlockedLevel.ToString());


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
            listWordPassed = wordPassed.Split('|').OfType<string>().ToList<string>();
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
        JArray arrayJson = JArray.Parse(text);
        for (int i = 0; i < arrayJson.Count; i++)
        {
            wordData = JsonConvert.DeserializeObject<WordData>(arrayJson[i].ToString());

            //listMeanWord.Add(wordData);

            meaning += "(" + wordData.partOfSpeech + ") " + wordData.text.Replace("<xref>", "").Replace("</xref>", "") + "\n";
        }

        //if(!wordsDict.ContainsKey(word))
        //    wordsDict.Add(word, listMeanWord);
        Debug.Log(word);
        MeanDialog.wordName = word;
        MeanDialog.wordMean = meaning.ToString();
        Debug.Log(MeanDialog.wordName);
        Debug.Log(MeanDialog.wordMean);

    }

    public void ShowWordData()
    {
        //GetDataFromApi();
        //foreach (KeyValuePair<string, List<WordData>> word in wordsDict)
        //{
        //    textWordName.text = word.Key;
        //    foreach (WordData wordMean in word.Value)
        //    {
        //        textMean.text += "(" + wordMean.partOfSpeech + ") " + wordMean.text.Replace("<xref>", "").Replace("</xref>", "") + "\n";
        //    }
        //}
        
    }

    public void CloneGroupWord()
    {
        int numGroupWord;
        int numWord = listWordPassed!=null?listWordPassed.Count:0;
        if ( listWordPassed.Count < 3)
        {
            numGroupWord = 1;
        }
        else
        {
            if ( listWordPassed.Count % 3 == 0)
            {
                numGroupWord = (int)( listWordPassed.Count / 3);
            }
            else
            {
                numGroupWord= (int)( listWordPassed.Count / 3+1);
            }
        }
        Debug.Log("numGroupWord: " + numGroupWord);
        for (int i=0; i<numGroupWord; i++)
        {
            GameObject groupWordClone;
            groupWordClone = GameObject.Instantiate(groupWord,content.transform);
            CloneButtonWord(groupWordClone, ref listWordPassed);


        }
    }

    public void CloneButtonWord(GameObject groupWord, ref List<string> listWordPassed)
    {
        GameObject buttonWordClone;
        if (listWordPassed.Count >= 3)
        {
            for(int j=0; j<3; j++)
            {
                buttonWordClone = GameObject.Instantiate(buttonWord, groupWord.transform);
                buttonWordClone.transform.GetChild(0).GetComponent<Text>().text = listWordPassed[0];
                listWordPassed.RemoveAt(0);
                Debug.Log("listWordPassed.Count: " + listWordPassed.Count);
                if (listWordPassed.Count <2)
                {
                    CloneButtonWord(groupWord, ref listWordPassed);
                    break;
                }
            }
        }
        else
        {
            for (int j = 0; j < listWordPassed.Count-1; j++)
            {
                Debug.Log("yesssss");
                buttonWordClone = GameObject.Instantiate(buttonWord, groupWord.transform);
                buttonWordClone.transform.GetChild(0).GetComponent<Text>().text = listWordPassed[0];
                listWordPassed.RemoveAt(0);
            }
        }
    }

}

