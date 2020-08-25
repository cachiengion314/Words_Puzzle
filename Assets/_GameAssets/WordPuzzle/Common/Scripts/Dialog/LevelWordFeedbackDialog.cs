using Firebase.Database;
using Superpow;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Common;

public class LevelWordFeedbackDialog : Dialog
{
    public static List<Toggle> toggleList = new List<Toggle>();

    private List<string> correctWordsDoneByPlayerList = new List<string>();
 
    public RectTransform content;
    public RectTransform wordDoneByPlayerPrefab;
    public int currlevel;
    [Header("THEME UI CHANGE")]
    [SerializeField] private Image _btnSend;
    [SerializeField] private Image _boardWord;
    [SerializeField] private TextMeshProUGUI _txtTitle;
    [SerializeField] private TextMeshProUGUI _txtSend;

    protected override void Awake()
    {
        base.Awake();

        MissingWordsFeedback._dataWordsRef = FirebaseDatabase.DefaultInstance
           .GetReference("feedbacks");
    }
    protected override void Start()
    {
        base.Start();
        CheckTheme();
        var numlevels = Utils.GetNumLevels(GameState.currentWorld, GameState.currentSubWorld);
        currlevel = (GameState.currentLevel + numlevels * GameState.currentSubWorld + MainController.instance.gameData.words[0].subWords.Count * numlevels * GameState.currentWorld) + 1;

        WordsCorrectDoneByPlayer();
      
        if (correctWordsDoneByPlayerList.Count > 0)
        {
            for (int i = 0; i < correctWordsDoneByPlayerList.Count; i++)
            {
                Toggle textToggle = Instantiate(wordDoneByPlayerPrefab).GetComponent<Toggle>();
                textToggle.isOn = false;
                textToggle.SetParent(content);
                TextMeshProUGUI text = textToggle.GetComponentInChildren<TextMeshProUGUI>();
                textToggle.transform.localScale = Vector3.one;
             
                text.text = correctWordsDoneByPlayerList[i].ToString();
            }
        }
    }

    private void CheckTheme()
    {
        if (MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            _btnSend.sprite = currTheme.uiData.levelWordData.btnSend;
            _boardWord.sprite = currTheme.uiData.levelWordData.boardWord;

            //_boardWord.SetNativeSize();

            _txtTitle.color = currTheme.fontData.colorContentDialog;
            _txtSend.color = currTheme.uiData.levelWordData.colorTextBtn;

            var wordToggle = wordDoneByPlayerPrefab.GetComponent<WordDoneByPlayerToggle>();
            wordToggle.wordNormal.sprite = currTheme.uiData.levelWordData.wordNormal;
            wordToggle.wordDone.sprite = currTheme.uiData.levelWordData.wordDone;
            wordToggle.wordNormal.SetNativeSize();

            wordToggle.txtWord.color = currTheme.uiData.levelWordData.colorTextWordPfb;
        }
    }

    public void OnSendIrrelevantWords()
    {
        string longText = null;
        for (int i = 0; i < toggleList.Count; i++)
        {
            TextMeshProUGUI textMeshPro = toggleList[i].GetComponentInChildren<TextMeshProUGUI>();
            if (i < toggleList.Count - 1)
                longText += textMeshPro.text.ToString() + ",";
            else
                longText += textMeshPro.text.ToString();
        }
        if (longText == null || longText.Length == 0) { Close(); return; }

        string key = MissingWordsFeedback._dataWordsRef.Push().Key;
        Dictionary<string, object> infoDic = new Dictionary<string, object>
        {
            ["type"] = "irregular",
            ["results"] = longText,
            ["date"] = DateTime.Now.ToString("MM/dd/yyyy"),
            ["status"] = "open",
            ["level"] = currlevel
        };
        // Push information
        MissingWordsFeedback.childUpdates["/" + key] = infoDic;
        MissingWordsFeedback._dataWordsRef.UpdateChildrenAsync(MissingWordsFeedback.childUpdates);

        Close();
    }
    public void WordsCorrectDoneByPlayer()
    {
        if (WordRegion.instance != null && WordRegion.instance.listWordCorrect.Count > 0)
        {
            for (int i = 0; i < WordRegion.instance.listWordCorrect.Count; i++)
            {
                string word = WordRegion.instance.listWordCorrect[i];
                correctWordsDoneByPlayerList.Add(word);
            }
        }
    }
    public void OnCloseClick()
    {
        Close();
    }
}
