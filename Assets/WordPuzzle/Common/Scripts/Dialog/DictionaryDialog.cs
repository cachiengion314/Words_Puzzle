﻿using System.Collections;
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
using System;
using PlayFab;

public class DictionaryDialog : Dialog
{
    public GameObject flagTab;
    public GameObject flagTabScrollViewContent;
    public GameObject flagItemPrefab;
    [Space]
    public GameObject buttonWord;
    public GameObject groupWord;
    public GameObject noInternet;
    public ListGroupWord listGroupWord;
    public Transform content;
    public static DictionaryDialog instance;
    public TextMeshProUGUI numWordPassedText;

    [HideInInspector] public ListGroupWord currListWord;
    //string wordValid;
    //List<WordData> listMeanWord = new List<WordData>();

    Dictionary<string, string> wordDiction = new Dictionary<string, string>();
    Dictionary<string, List<string>> groupWordDiction = new Dictionary<string, List<string>>();
    Dictionary<string, List<string>> dataGroupWordDiction = new Dictionary<string, List<string>>();
    char[] keys;
    List<string> defaultValue = new List<string>();
    List<string> listWordPassed;
    [HideInInspector] public List<ListGroupWord> groupWords = new List<ListGroupWord>();
    HomeController homecontroller;

    public MeanDialog meanDialog;

    public GameObject shadowPanel;
    //static readonly string SAVE_FOLDER = Application.dataPath + "/saves/";
    //Dictionary dict;

    [HideInInspector]
    public static string wordPassed;

    protected override void Awake()
    {
        if (instance == null)
            instance = this;
        if (HomeController.instance != null)
            homecontroller = HomeController.instance;
        keys = Enumerable.Range('a', 'z' - 'a' + 1).Select(i => (Char)i).ToArray();
        foreach (char key in keys)
        {
            groupWordDiction.Add(key.ToString().ToUpper(), defaultValue);
        }
        GetWordPassed();

        flagTab.gameObject.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        if (listWordPassed != null)
            CloneListGroupWord();
        numWordPassedText.text = "You have collected " + listWordPassed.Count + " words";
        
        // Instantiate the flag tab
        for (int i = 0; i < FacebookController.instance.flagItemList.Count; i++)
        {
            FlagItemController flagItem = Instantiate(flagItemPrefab, flagTabScrollViewContent.transform).GetComponent<FlagItemController>();
            flagItem.flagImage = FacebookController.instance.flagItemList[i].flagImage;
            flagItem.flagName = FacebookController.instance.flagItemList[i].flagName;
        }
    }
    public void OnClickFlagTab()
    {
        flagTab.gameObject.SetActive(true);
    }
    public void OnClickVocabularyTab()
    {
        flagTab.gameObject.SetActive(false);
    }
    public void GetWordPassed()
    {
        wordPassed = CPlayerPrefs.GetString("WordLevelSave");
        if (PlayFabClientAPI.IsClientLoggedIn())
            wordPassed = FacebookController.instance.user.wordPassed != null ? FacebookController.instance.user.wordPassed : "";
        Debug.Log(wordPassed);
        if (wordPassed != null)
        {
            listWordPassed = wordPassed.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            listWordPassed.Sort();
            listWordPassed = listWordPassed.Distinct().ToList();
            if (listWordPassed.Count > 0)
            {
                foreach (string word in listWordPassed)
                {
                    char[] charWord = word.ToCharArray();
                    wordDiction.Add(word, charWord[0].ToString().ToUpper());
                }

                dataGroupWordDiction = wordDiction.GroupBy(r => r.Value).ToDictionary(t => t.Key, t => t.Select(r => r.Key).ToList());

                foreach (var item in dataGroupWordDiction)
                {
                    //Debug.Log(item.Key);
                    groupWordDiction[item.Key] = item.Value;
                }
            }
        }
    }
    public void CloneListGroupWord()
    {
        ListGroupWord listGroupWordClone;
        GameObject buttonWordClone;
        groupWordDiction.Distinct();
        foreach (var item in groupWordDiction)
        {
            listGroupWordClone = Instantiate(listGroupWord, content.transform);
            groupWords.Add(listGroupWordClone/*.GetComponent<ListGroupWord>()*/);
            listGroupWordClone.firstButtonText.text = item.Key + ".";
            if (item.Value.Count > 0)
            {
                if (item.Value.Count == 1)
                {
                    listGroupWordClone.numberWordText.text = item.Value.Count + " word";
                }
                else
                {
                    listGroupWordClone.numberWordText.text = item.Value.Count + " words";
                }
                listGroupWordClone.numberWord = item.Value.Count;
            }
            foreach (var word in item.Value)
            {
                //Debug.Log(item.Key + ": " + word);
                buttonWordClone = Instantiate(buttonWord, listGroupWordClone.groupWord);
                buttonWordClone.transform.GetChild(0).GetComponent<Text>().text = word;
            }
        }
    }

    public void OnPlayClick()
    {
        if (homecontroller != null)
            homecontroller.OnClick(0);
        Close();
    }

    public void ShowMeanDialog()
    {
        shadowPanel.SetActive(true);
        TweenControl.GetInstance().ScaleFromZero(meanDialog.gameObject, 0.3f);
    }

    public void HideMeanDialog()
    {
        TweenControl.GetInstance().ScaleFromOne(meanDialog.gameObject, 0.3f, () =>
        {
            shadowPanel.SetActive(false);
        });
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




