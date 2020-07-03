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
    private List<RectTransform> textTransformPosList = new List<RectTransform>();
    public RectTransform textBackgroundPrefab;
    public RectTransform wordDoneByPlayerPrefab;
    public int currlevel;
    protected override void Awake()
    {
        base.Awake();

        MissingWordsFeedback._dataWordsRef = FirebaseDatabase.DefaultInstance
           .GetReference("feedbacks");
    }
    protected override void Start()
    {
        base.Start();
        var numlevels = Utils.GetNumLevels(GameState.currentWorld, GameState.currentSubWorld);
        currlevel = (GameState.currentLevel + numlevels * GameState.currentSubWorld + MainController.instance.gameData.words[0].subWords.Count * numlevels * GameState.currentWorld) + 1;

        WordsCorrectDoneByPlayer();
        foreach (RectTransform child in textBackgroundPrefab)
        {
            textTransformPosList.Add(child);
        }
        if (correctWordsDoneByPlayerList.Count > 0)
        {
            for (int i = 0; i < correctWordsDoneByPlayerList.Count; i++)
            {
                Toggle textToggle = Instantiate(wordDoneByPlayerPrefab).GetComponent<Toggle>();
                textToggle.isOn = false;
                textToggle.SetParent(textBackgroundPrefab);
                TextMeshProUGUI text = textToggle.GetComponentInChildren<TextMeshProUGUI>();
                textToggle.transform.localScale = Vector3.one;
                textToggle.transform.position = textTransformPosList[i].position;
                text.text = correctWordsDoneByPlayerList[i].ToString();
            }
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
