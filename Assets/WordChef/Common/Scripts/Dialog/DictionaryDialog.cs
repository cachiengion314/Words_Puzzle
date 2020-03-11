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

    


    //string wordValid;
    string meaning;
    string sourceDictionaries;
    string keyApi;
    string url;
    //List<WordData> listMeanWord = new List<WordData>();
    WordData wordData;
    Dictionary<string, string> wordDiction =new Dictionary<string, string>();
    Dictionary<string, List<string>> groupWordDiction=new Dictionary<string, List<string>>();
    Dictionary<string, List<string>> dataGroupWordDiction= new Dictionary<string, List<string>>();
    char[] keys;
    List<string> defaultValue = new List<string>();
    private int passWorld, passSubWorld, passLevel;
    private GameLevel gameLevel;
    List<string> listWordPassed;

    [HideInInspector]
    public static string wordPassed;
    

    protected void Start()
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
            //CloneGroupWord();
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
            //char[] testString = listWordPassed[0].ToCharArray();
            foreach(string word in listWordPassed)
            {
                char[] charWord = word.ToCharArray();
                wordDiction.Add(word, char.ToUpper(charWord[0]).ToString());
            }
       
             dataGroupWordDiction= wordDiction.GroupBy(r => r.Value).ToDictionary(t =>t.Key, t => t.Select(r => r.Key).ToList());

            //foreach (var data in dataGroupWordDiction)
            //{
            //    foreach(var word in data.Value)
            //    {
            //        Debug.Log(data.Key + ": "+word);
            //    }
            //}

            foreach (var item in dataGroupWordDiction)
            {
                Debug.Log(item.Key);
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
        JArray arrayJson = JArray.Parse(text);
        for (int i = 0; i < arrayJson.Count; i++)
        {
            wordData = JsonConvert.DeserializeObject<WordData>(arrayJson[i].ToString());

            //listMeanWord.Add(wordData);

            meaning += "(" + wordData.partOfSpeech + ") " + wordData.text.Replace("<xref>", "").Replace("</xref>", "") + "\n";
        }

        Debug.Log(word);
        MeanDialog.wordName = word;
        MeanDialog.wordMean = meaning.ToString();
        Debug.Log(MeanDialog.wordName);
        Debug.Log(MeanDialog.wordMean);

    }


    //public void CloneGroupWord()
    //{
    //    int numGroupWord;
    //    int numWord = listWordPassed!=null?listWordPassed.Count:0;
    //    if ( listWordPassed.Count < 3)
    //    {
    //        numGroupWord = 1;
    //    }
    //    else
    //    {
    //        if ( listWordPassed.Count % 3 == 0)
    //        {
    //            numGroupWord = (int)( listWordPassed.Count / 3);
    //        }
    //        else
    //        {
    //            numGroupWord= (int)( listWordPassed.Count / 3+1);
    //        }
    //    }
    //    //Debug.Log("numGroupWord: " + numGroupWord);
    //    for (int i=0; i<numGroupWord; i++)
    //    {
    //        GameObject groupWordClone;
    //        groupWordClone = GameObject.Instantiate(groupWord, content.transform);
    //        CloneButtonWord(groupWordClone, ref listWordPassed);


    //    }
    //}

    public void CloneButtonWord(GameObject groupWord,List<string> listWordPassed)
    {
        GameObject buttonWordClone;
        buttonWordClone = GameObject.Instantiate(buttonWord, groupWord.transform);
        buttonWordClone.transform.GetChild(0).GetComponent<Text>().text = listWordPassed[0];
        listWordPassed.RemoveAt(0);

        //if (listWordPassed.Count >= 3)
        //{
        //    for(int j=0; j<3; j++)
        //    {
        //        buttonWordClone = GameObject.Instantiate(buttonWord, groupWord.transform);
        //        buttonWordClone.transform.GetChild(0).GetComponent<Text>().text = listWordPassed[0];
        //        listWordPassed.RemoveAt(0);
        //        //Debug.Log("listWordPassed.Count: " + listWordPassed.Count);
        //        if (listWordPassed.Count <2)
        //        {
        //            CloneButtonWord(groupWord, ref listWordPassed);
        //            break;
        //        }
        //    }
        //}
        //else
        //{
        //    for (int j = 0; j < listWordPassed.Count-1; j++)
        //    {
        //        buttonWordClone = GameObject.Instantiate(buttonWord, groupWord.transform);
        //        buttonWordClone.transform.GetChild(0).GetComponent<Text>().text = listWordPassed[0];
        //        listWordPassed.RemoveAt(0);
        //    }
        //}

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
                Debug.Log(item.Key + ": " + word);
                buttonWordClone = GameObject.Instantiate(buttonWord, listGroupWordClone.GetComponent<ListGroupWord>().groupWord);
                buttonWordClone.transform.GetChild(0).GetComponent<Text>().text = word;
            }
        }
   
    }

}

