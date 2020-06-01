using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Common;

public class LevelWordFeedbackDialog : FeedbackDialog
{
    public static List<Toggle> toggleList = new List<Toggle>();

    private List<string> correctWordsDoneByPlayerList = new List<string>();
    private List<RectTransform> textTransformPosList = new List<RectTransform>();
    public RectTransform textBackgroundPrefab;
    public RectTransform wordDoneByPlayerPrefab;
    protected override void Awake()
    {
        base.Awake();

        MissingWordsFeedback._missingWordsRef = FirebaseDatabase.DefaultInstance
           .GetReference("Missing and Irrelevant Words");
    }
    protected override void Start()
    {
        base.Start();
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
        for (int i = 0; i < toggleList.Count; i++)
        {
            TextMeshProUGUI TMP = toggleList[i].GetComponentInChildren<TextMeshProUGUI>();

            string key = MissingWordsFeedback._missingWordsRef.Push().Key;
            MissingWordsFeedback.childUpdates["/Irrelevant words/" + key] = TMP.text;
            MissingWordsFeedback._missingWordsRef.UpdateChildrenAsync(MissingWordsFeedback.childUpdates);
        }
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
    public new void OnCloseClick()
    {
        Close();
    }
}
