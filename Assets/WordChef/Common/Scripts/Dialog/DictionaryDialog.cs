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
using UnityEngine.SceneManagement;


public class DictionaryDialog : Dialog
{
    public GameObject buttonWord;
    public GameObject groupWord;
    public GameObject listGroupWord;
    public Transform content;
    public static DictionaryDialog instance;
    public Text numWordPassedText;
    

    //string wordValid;
    //List<WordData> listMeanWord = new List<WordData>();
    
    Dictionary<string, string> wordDiction =new Dictionary<string, string>();
    Dictionary<string, List<string>> groupWordDiction=new Dictionary<string, List<string>>();
    Dictionary<string, List<string>> dataGroupWordDiction= new Dictionary<string, List<string>>();
    char[] keys;
    List<string> defaultValue = new List<string>();
    List<string> listWordPassed;
    GameObject homecontroller;

    public MeanDialog meanDialog;

    public GameObject shadowPanel;
    //static readonly string SAVE_FOLDER = Application.dataPath + "/saves/";
    //Dictionary dict;

    [HideInInspector]
    public static string wordPassed;



    protected override void Awake()
    {
        instance = this;
        homecontroller = GameObject.FindGameObjectWithTag("HomeController");
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
        numWordPassedText.text = "You have collected " + listWordPassed.Count + "/100000 words";

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
            listWordPassed = listWordPassed.Distinct().ToList();
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

   


    public void CloneListGroupWord()
    {
        GameObject listGroupWordClone;
        GameObject buttonWordClone;

        foreach ( KeyValuePair<string, List<string>> item in  groupWordDiction)
        {
            listGroupWordClone = GameObject.Instantiate(listGroupWord, content.transform);
            listGroupWordClone.transform.Find("Button").Find("FirstLetter").GetComponent<TextMeshProUGUI>().text = item.Key + ".";
            if (item.Value.Count > 0)
            {
                if (item.Value.Count == 1)
                {
                    listGroupWordClone.transform.Find("Button").Find("NumWord").GetComponent<TextMeshProUGUI>().text = item.Value.Count+" word";
                }
                else
                {
                    listGroupWordClone.transform.Find("Button").Find("NumWord").GetComponent<TextMeshProUGUI>().text = item.Value.Count+" words";
                }
            }
            foreach(var word in item.Value)
            {
                //Debug.Log(item.Key + ": " + word);
                buttonWordClone = GameObject.Instantiate(buttonWord, listGroupWordClone.GetComponent<ListGroupWord>().groupWord);
                buttonWordClone.transform.GetChild(0).GetComponent<Text>().text = word;
            }
        }
   
    }

    public void OnPlayClick()
    {
        homecontroller.GetComponent<HomeController>().OnClick(0);
        gameObject.GetComponent<Dialog>().Close();

    }

    public void ShowMeanDialog()
    {
        shadowPanel.SetActive(true);
        TweenControl.GetInstance().ScaleFromZero(meanDialog.gameObject, 0.3f);
    }

    public void HideMeanDialog()
    {
        shadowPanel.SetActive(false);
        TweenControl.GetInstance().ScaleFromOne(meanDialog.gameObject, 0.3f);
    }

    public void SetTextMeanDialog(string name, string wordMean)
    {
        meanDialog.wordName = name;
        meanDialog.wordMean = wordMean;
        meanDialog.showMean();
    }

    public void SetWordMeanText(string text)
    {
        meanDialog.wordMean = text;
    }
}




