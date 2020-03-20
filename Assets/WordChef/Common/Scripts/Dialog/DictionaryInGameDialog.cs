using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryInGameDialog : Dialog
{
    public static DictionaryInGameDialog instance;

    public SnapScrolling snapScrolling;
    public ScrollRect scrollRect;
    [Header("Content")] public RectTransform contentRectTransform;
    [Header("Other Object")] public GameObject itemPrefab;

    public TextMeshProUGUI wordNameText;

    public GameObject arrowLeftObject;
    public GameObject arrowRightObject;
    
    string meaning;
    string sourceDictionaries;
    string keyApi;
    string url;
    WordData wordData;
    
    public List<string> listWordInLevel;
    public Dictionary<string, MeanItemDictionary> listMeanItemObject = new Dictionary<string, MeanItemDictionary>();
    
    protected override void Awake()
    {
        instance = this;
    }
    void Start()
    {
        base.Start();
        snapScrolling.Init(itemPrefab.GetComponent<RectTransform>().sizeDelta.x);
        CheckHaveWords();
    }

    void Update()
    {
        SetArrowObject();
        SetWordNameText();
    }

    void SetArrowObject()
    {
        if(snapScrolling == null) return;
        if (snapScrolling.listItem.Count <= 1)
        {
            arrowLeftObject.SetActive(false);
            arrowRightObject.SetActive(false);
        }
        else
        {
            if (snapScrolling.selectItemID > 0 && snapScrolling.selectItemID < snapScrolling.listItem.Count - 1)
            {
                arrowLeftObject.SetActive(true);
                arrowRightObject.SetActive(true);
            }
            else
            {
                if (snapScrolling.selectItemID == 0)
                {
                    arrowLeftObject.SetActive(false);
                    arrowRightObject.SetActive(true);
                }
                if (snapScrolling.selectItemID == snapScrolling.listItem.Count - 1)
                {
                    arrowLeftObject.SetActive(true);
                    arrowRightObject.SetActive(false);
                }
            }
        }
    }

    void SetWordNameText()
    {
        if (listWordInLevel.Count > 0)
        {
            wordNameText.text = listWordInLevel[snapScrolling.selectItemID];
        }
    }

    void CheckHaveWords()
    {
        if (WordRegion.instance.listWordCorrect.Count > 0)
        {
            InstantiateMeanItem();
        }
        else
        {
            wordNameText.text = "No words";
        }
    }
    void InstantiateMeanItem()
    {
        for (int i = 0; i < WordRegion.instance.listWordInLevel.Count; i++)
        {
            for (int j = 0; j < WordRegion.instance.listWordCorrect.Count; j++)
            {
                string word = WordRegion.instance.listWordCorrect[j];
                if (WordRegion.instance.listWordInLevel[i] == word)
                {
                    GameObject item = Instantiate(itemPrefab, contentRectTransform);
                    MeanItemDictionary meanItemDictionary = item.GetComponent<MeanItemDictionary>();
                    meanItemDictionary.SetParentNestedScrollRect(scrollRect);
                    
                    listMeanItemObject.Add(word, meanItemDictionary);
                    listWordInLevel.Add(word);
                    snapScrolling.AddItemToList(item);
                    
                    if (!Dictionary.instance.CheckWExistInDictWordSaved(word))
                    {
                        Dictionary.instance.GetDataFromApi(word);
                        meanItemDictionary.SetMeanText("Loading...");
                    }
                    else
                    {
                        meanItemDictionary.SetMeanText(Dictionary.instance.dictWordSaved[word]);
                    }
                }
            }
        }
    }

    public void SetDataForMeanItemGetAPI(string word, string meanText)
    {
        listMeanItemObject[word].SetMeanText(meanText);
    }

    public void ArrowPageButton(bool isNext)
    {
        if (isNext)
        {
            snapScrolling.selectItemID++;
        }
        else
        {
            snapScrolling.selectItemID--;
        }
    }
}
