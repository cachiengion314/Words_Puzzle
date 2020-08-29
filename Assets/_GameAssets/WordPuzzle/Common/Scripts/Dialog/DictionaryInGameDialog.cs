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
    public GameObject noInternet;

    string meaning;
    string sourceDictionaries;
    string keyApi;
    string url;
    WordData wordData;

    public List<string> listWordInLevel;
    public Dictionary<string, MeanItemDictionary> listMeanItemObject = new Dictionary<string, MeanItemDictionary>();

    [Header("THEME UI CHANGE")]
    [SerializeField] private Image _board;
    [SerializeField] private Image _arrowLeft;
    [SerializeField] private Image _arrowRight;

    protected override void Awake()
    {
        instance = this;
        snapScrolling.onScroll = OnScrollItem;
    }

    void OnScrollItem()
    {
        ShowNointernet(WordRegion.instance.listWordCorrect[snapScrolling.selectItemID]);
    }

    void Start()
    {
        base.Start();
        noInternet.SetActive(false);
        snapScrolling.Init(itemPrefab.GetComponent<RectTransform>().sizeDelta.x);
        CheckHaveWords();
        CheckTheme();
    }

    void Update()
    {
        SetArrowObject();
        SetWordNameText();
    }

    private void CheckTheme()
    {
        if (MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            if (_board != null)
                _board.sprite = currTheme.uiData.meanWordData.board;
            _arrowLeft.sprite = currTheme.uiData.meanWordData.arrowLeft;
            _arrowRight.sprite = currTheme.uiData.meanWordData.arrowRight;

            _arrowLeft.SetNativeSize();
            _arrowRight.SetNativeSize();

            wordNameText.color = currTheme.fontData.colorContentDialog;
        }
    }

    void SetArrowObject()
    {
        if (snapScrolling == null) return;
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
        //for (int i = 0; i < WordRegion.instance.listWordInLevel.Count; i++)
        //{
        for (int j = 0; j < WordRegion.instance.listWordCorrect.Count; j++)
        {
            string word = WordRegion.instance.listWordCorrect[j];
            if (WordRegion.instance.listWordInLevel.Contains(word))
            {
                GameObject item = Instantiate(itemPrefab, contentRectTransform);
                MeanItemDictionary meanItemDictionary = item.GetComponent<MeanItemDictionary>();
                meanItemDictionary.SetParentNestedScrollRect(scrollRect);

                if (!listMeanItemObject.ContainsKey(word))
                    listMeanItemObject.Add(word, meanItemDictionary);
                listWordInLevel.Add(word);
                snapScrolling.AddItemToList(item);

                if (!Dictionary.instance.CheckWExistInDictWordSaved(word))
                {
                    CUtils.CheckConnection(this, (result) =>
                    {
                        if (result == 0)
                        {
                            //Dictionary.instance.GetDataFromApi(word);
                            StartCoroutine(Dictionary.instance.GetDataFromApiDelay(word));
                            meanItemDictionary.SetMeanText("Loading...");
                        }
                        else
                        {
                            noInternet.SetActive(true);
                        }
                    });
                }
                else
                {
                    meanItemDictionary.SetMeanText(Dictionary.instance.dictWordSaved[word]);
                    noInternet.SetActive(false);
                }
            }
        }
        //}
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
        ShowNointernet(WordRegion.instance.listWordCorrect[snapScrolling.selectItemID]);
    }

    public void ShowMeanWordByID(int ID)
    {
        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
            snapScrolling.selectItemID = ID;
        });
    }

    public void GetIndexByWord(string word)
    {
        for (int i = 0; i < WordRegion.instance.listWordCorrect.Count; i++)
        {
            int index = i;
            if (word.ToLower() == WordRegion.instance.listWordCorrect[index])
            {
                TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
                {
                    snapScrolling.selectItemID = index;
                    ShowNointernet(word);
                });
                break;
            }
        }
    }

    private void ShowNointernet(string word)
    {
        if (!Dictionary.instance.CheckWExistInDictWordSaved(word))
        {
            CUtils.CheckConnection(this, (result) =>
            {
                if (result == 0)
                {
                    noInternet.SetActive(false);
                }
                else
                {
                    noInternet.SetActive(true);
                }
            });
        }
        else
        {
            noInternet.SetActive(false);
        }
    }

    void OnDestroy()
    {
        TweenControl.GetInstance().KillDelayCall(transform);
    }
}
