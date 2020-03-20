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
            wordNameText.text = "Loading...";
            Invoke("InstantiateMeanItem", 1f);
        }
        else
        {
            wordNameText.text = "No words";
        }
    }
    void InstantiateMeanItem()
    {
        /*for (int i = 0; i < WordRegion.instance.listWordInLevel.Count; i++)
        {
            if (Dictionary.instance.CheckWExistInDictWordSaved(WordRegion.instance.listWordInLevel[i]))
            {
                GameObject item = Instantiate(itemPrefab, contentRectTransform);
                MeanItemDictionary meanItemDictionary = item.GetComponent<MeanItemDictionary>();
                meanItemDictionary.SetParentNestedScrollRect(scrollRect);
                meanItemDictionary.SetMeanText( Dictionary.instance.dictWordSaved[WordRegion.instance.listWordInLevel[i]]);

                listWordInLevel.Add(WordRegion.instance.listWordInLevel[i]);
                snapScrolling.AddItemToList(item);
            }
            else
            {
                for (int j = 0; j < WordRegion.instance.listWordCorrect.Count; j++)
                {
                    if (WordRegion.instance.listWordInLevel[i].Contains(WordRegion.instance.listWordCorrect[j]))
                    {
                        Dictionary.instance.GetDataFromApi(WordRegion.instance.listWordInLevel[i]);
                        if (Dictionary.instance.CheckWExistInDictWordSaved(WordRegion.instance.listWordInLevel[i]))
                        {
                            GameObject item = Instantiate(itemPrefab, contentRectTransform);
                            MeanItemDictionary meanItemDictionary = item.GetComponent<MeanItemDictionary>();
                            meanItemDictionary.SetParentNestedScrollRect(scrollRect);
                            meanItemDictionary.SetMeanText(Dictionary.instance.dictWordSaved[WordRegion.instance.listWordInLevel[i]]);
                        
                            listWordInLevel.Add(WordRegion.instance.listWordInLevel[i]);
                            snapScrolling.AddItemToList(item);
                        }
                    }
                }
            }
        }*/

        if (WordRegion.instance.listWordCorrect.Count > 0)
        {
            for (int j = 0; j < WordRegion.instance.listWordCorrect.Count; j++)
            {
                string word = WordRegion.instance.listWordCorrect[j];
                if (!Dictionary.instance.CheckWExistInDictWordSaved(word))
                {
                    Dictionary.instance.GetDataFromApi(word);
                    if (Dictionary.instance.CheckWExistInDictWordSaved(word))
                    {
                        GameObject item = Instantiate(itemPrefab, contentRectTransform);
                        MeanItemDictionary meanItemDictionary = item.GetComponent<MeanItemDictionary>();
                        meanItemDictionary.SetParentNestedScrollRect(scrollRect);
                        meanItemDictionary.SetMeanText(Dictionary.instance.dictWordSaved[word]);
                        
                        listWordInLevel.Add(word);
                        snapScrolling.AddItemToList(item);
                    }
                }
                else
                {
                    GameObject item = Instantiate(itemPrefab, contentRectTransform);
                    MeanItemDictionary meanItemDictionary = item.GetComponent<MeanItemDictionary>();
                    meanItemDictionary.SetParentNestedScrollRect(scrollRect);
                    meanItemDictionary.SetMeanText( Dictionary.instance.dictWordSaved[word]);

                    listWordInLevel.Add(word);
                    snapScrolling.AddItemToList(item);
                }
            }
        }
    }
}
