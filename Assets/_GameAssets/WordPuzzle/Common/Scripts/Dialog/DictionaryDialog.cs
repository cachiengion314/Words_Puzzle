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
using System;
using PlayFab;
using Microsoft.SqlServer.Server;

public class DictionaryDialog : Dialog
{
    [SerializeField] private DictionaryScrollerController _dictionaryScrollerController;
    [SerializeField] private FlagScrollerController _flagScrollerController;
    [Space]
    public Color colorOn;
    public Color colorOff;
    [Space]
    public GameObject flagTab;
    public Transform rootFlagList;
    public List<FlagItemController> flagList = new List<FlagItemController>();
    //public GameObject flagTabScrollViewContent;
    //public RectTransform scrollFlag;
    public GameObject flagItemPrefab;
    public GameObject FlagBtn;

    public Text titleFlagTabTxt;
    private readonly string TITLE_CONTENT = "Unlock flags to discover the world";

    public TextMeshProUGUI visualHoneyTxt;
    public TextMeshProUGUI honeyTxt;
    public FlagMeanDialog flagMeanDialog;
    public UnLockTheFlagDialog unlockTheFlagDialog;
    [Space]
    public GameObject vocabularyTab;
    public GameObject vocabularyBtn;

    [Space]
    public GameObject buttonWord;
    //public GameObject groupWord;
    public GameObject noInternet;
    public ListGroupWord listGroupWord;
    //public Transform content;
    public static DictionaryDialog instance;
    public Text numWordPassedText;
    public List<string> listWordPassed;

    [HideInInspector] public ListGroupWord currListWord;
    //string wordValid;
    //List<WordData> listMeanWord = new List<WordData>();

    Dictionary<string, string> wordDiction = new Dictionary<string, string>();
    Dictionary<string, List<string>> groupWordDiction = new Dictionary<string, List<string>>();
    Dictionary<string, List<string>> dataGroupWordDiction = new Dictionary<string, List<string>>();
    char[] keys;
    List<string> defaultValue = new List<string>();

    [HideInInspector] public List<ListGroupWord> groupWords = new List<ListGroupWord>();
    HomeController homecontroller;

    public MeanDialog meanDialog;

    public GameObject shadowPanel;
    public GameObject OverLayDialog;
    //static readonly string SAVE_FOLDER = Application.dataPath + "/saves/";
    //Dictionary dict;

    [HideInInspector]
    public static string wordPassed;

    public Dictionary<string, List<string>> Itemsdictionary
    {
        get
        {
            return groupWordDiction;
        }
    }

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

        //flagTab.gameObject.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        InitFlagList();
        InitCollectionTab();
        CheckShowTutFlag();
    }

    private void CheckShowTutFlag()
    {
        if (!CPlayerPrefs.HasKey("HONEY_TUTORIAL") && !TutorialController.instance.isShowTut && FacebookController.instance.user.unlockedFlagWords.Count > 0)
        {
            TutorialController.instance.isBlockSwipe = true;
            var flagTarget = flagList.Find(flag => !flag.isLocked);
            var raycast = flagTarget.gameObject.AddComponent<GraphicRaycaster>();
            flagTarget.gameObject.AddComponent<Canvas>();
            var canvas = flagTarget.gameObject.GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingLayerName = "UI2";
            canvas.sortingOrder = 6;
            var indexTarget = flagList.IndexOf(flagTarget) / 2;
            _flagScrollerController.JumScrollToIndex(indexTarget);
            TweenControl.GetInstance().DelayCall(transform, 0.5f, () =>
            {
                TutorialController.instance.ShowPopFlagTut(flagTarget);
                //scrollFlag.GetComponent<ScrollRect>().content.anchoredPosition = new Vector3(flagTabScrollViewContent.transform.localPosition.x, resultContentPos);
                //TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
                //{
                //scrollFlag.GetComponent<ScrollRect>().content.anchoredPosition = new Vector3(flagTabScrollViewContent.transform.localPosition.x, resultContentPos);
                //});

            });
        }
    }

    private void InitCollectionTab()
    {
        if (listWordPassed != null)
            _dictionaryScrollerController.InitDictionaryScroller();
        numWordPassedText.text = "You have collected " + listWordPassed.Count + " words";

        _flagScrollerController.InitFlagScroller();
        WriteFlagTabTitleContent();

        if (HoneyFrameHomeScene.isClickOnThis || HoneyFrameMainScene.isClickOnThis)
        {
            OnClickFlagTab();
            HoneyFrameHomeScene.isClickOnThis = false;
            HoneyFrameMainScene.isClickOnThis = false;
        }
        else
        {
            OnClickVocabularyTab();
        }

        var unlockAll = CPlayerPrefs.GetBool("UNLOCK_ALL_FLAG", false);
        if (unlockAll)
            UnlockAllFlag();
    }
    public void WriteFlagTabTitleContent()
    {
        string title_content;
        int countUnlock = FlagTabController.instance.unlockedWordHashset.Count;
        if (countUnlock == 0)
        {
            title_content = TITLE_CONTENT;
        }
        else if (countUnlock == 1)
        {
            title_content = string.Format("You have unlocked {0} flag", 1);
        }
        else
        {
            title_content = string.Format("You have unlocked {0} flags", countUnlock); ;
        }
        titleFlagTabTxt.text = title_content;
    }
    private void InitFlagList()
    {
        // Instantiate the flag tab
        for (int i = 0; i < FlagTabController.instance.flagItemList.Count; i++)
        {
            FlagItemController flagItem = Instantiate(flagItemPrefab, rootFlagList).GetComponent<FlagItemController>();
            flagItem.gameObject.SetActive(false);
            flagItem.indexOfSmallFlagImage = FlagTabController.instance.flagItemList[i].flagSmallImageIndex;
            flagItem.indexOfBigFlagImage = FlagTabController.instance.flagItemList[i].flagBigImageIndex;
            flagItem.flagUnlockWord = FlagTabController.instance.flagItemList[i].flagUnlockWord;

            flagItem.flagName = FlagTabController.instance.flagItemList[i].flagName;
            flagItem.subRegion = FlagTabController.instance.flagItemList[i].subRegion;
            flagItem.capital = FlagTabController.instance.flagItemList[i].capital;
            flagItem.population = FlagTabController.instance.flagItemList[i].population;
            flagItem.area = FlagTabController.instance.flagItemList[i].area;

            string checkWord = flagItem.flagUnlockWord != string.Empty ? flagItem.flagUnlockWord : flagItem.flagName;
            if (FlagTabController.instance.unlockedWordHashset.Contains(checkWord.ToLower()))
            {
                flagItem.isLocked = false;
            }
            else
            {
                flagItem.isLocked = true;
            }

            flagItem.indexOfFlag = i;
            flagList.Add(flagItem);
        }
    }

    void SetTabActive(GameObject tab, GameObject tabBtn, bool status)
    {
        tab.SetActive(status);
        tabBtn.transform.Find("IconBtnOn").gameObject.SetActive(status);
        tabBtn.transform.Find("IconOn").gameObject.SetActive(status);
        tabBtn.transform.Find("IconOff").gameObject.SetActive(!status);
        if (status)
        {
            tabBtn.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = colorOn;

        }
        else
        {
            tabBtn.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = colorOff;
        }
    }
    public void OnClickFlagTab()
    {
        SetTabActive(flagTab, FlagBtn, true);
        SetTabActive(vocabularyTab, vocabularyBtn, false);
    }
    public void OnClickVocabularyTab()
    {
        SetTabActive(flagTab, FlagBtn, false);
        SetTabActive(vocabularyTab, vocabularyBtn, true);
    }
    public void GetWordPassed()
    {
        wordPassed = CPlayerPrefs.GetString("WordLevelSave");
        if (PlayFabClientAPI.IsClientLoggedIn())
            wordPassed = FacebookController.instance.user.wordPassed != null ? FacebookController.instance.user.wordPassed : "";
        LogController.Debug(wordPassed);
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
    //public void CloneListGroupWord()
    //{
    //    ListGroupWord listGroupWordClone;
    //    GameObject buttonWordClone;
    //    groupWordDiction.Distinct();
    //    foreach (var item in groupWordDiction)
    //    {
    //        listGroupWordClone = Instantiate(listGroupWord, content.transform);
    //        groupWords.Add(listGroupWordClone/*.GetComponent<ListGroupWord>()*/);
    //        listGroupWordClone.firstButtonText.text = item.Key + ".";
    //        if (item.Value.Count > 0)
    //        {
    //            if (item.Value.Count == 1)
    //            {
    //                listGroupWordClone.numberWordText.text = item.Value.Count + " word";
    //            }
    //            else
    //            {
    //                listGroupWordClone.numberWordText.text = item.Value.Count + " words";
    //            }
    //            listGroupWordClone.numberWord = item.Value.Count;
    //        }
    //        foreach (var word in item.Value)
    //        {
    //            //Debug.Log(item.Key + ": " + word);
    //            buttonWordClone = Instantiate(buttonWord, listGroupWordClone.groupWord);
    //            buttonWordClone.transform.GetChild(0).GetComponent<Text>().text = word;
    //        }
    //    }
    //}

    public void OnPlayClick()
    {
        if (homecontroller != null)
        {
            homecontroller.OnClick(0);
        }
        AudienceNetworkBanner.instance.LoadBanner();
        Close();
    }

    public void ShowMeanDialog()
    {
        shadowPanel.SetActive(true);
        OverLayDialog.SetActive(true);
        TweenControl.GetInstance().ScaleFromZero(meanDialog.gameObject, 0.3f);
    }

    public void HideMeanDialog()
    {
        TweenControl.GetInstance().ScaleFromOne(meanDialog.gameObject, 0.3f, () =>
        {
            shadowPanel.SetActive(false);
            OverLayDialog.SetActive(false);
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

    public override void Close()
    {
        if (MainController.instance != null)
        {
            AudienceNetworkBanner.instance.LoadBanner();
        }
        base.Close();
    }

    //==Test
    private void UnlockAllFlag()
    {
        foreach (var flag in flagList)
        {
            if (flag.isLocked)
            {
                flag.UnlockSuccess();
                if (flag.flagUnlockWord != string.Empty)
                {
                    FlagTabController.instance.AddToUnlockedWordDictionary(flag.flagUnlockWord);
                }
                else
                {
                    FlagTabController.instance.AddToUnlockedWordDictionary(flag.flagName);
                }
                WriteFlagTabTitleContent();
                FlagTabController.instance.SaveUnlockedWordData();
            }
        }
    }
    //==
}




