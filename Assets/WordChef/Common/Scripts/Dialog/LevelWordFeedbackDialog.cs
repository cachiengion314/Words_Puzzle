using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Common;

public class LevelWordFeedbackDialog : FeedbackDialog
{
    private List<string> correctWordsDoneByPlayerList = new List<string>();
    private List<RectTransform> textTransformPosList = new List<RectTransform>();
    public RectTransform textBackgroundPrefab;
    public RectTransform wordDoneByPlayerPrefab;
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
    public void WordsCorrectDoneByPlayer()
    {
        for (int i = 0; i < WordRegion.instance.listWordCorrect.Count; i++)
        {
            string word = WordRegion.instance.listWordCorrect[i];
            correctWordsDoneByPlayerList.Add(word);
        }
    }
    public new void OnCloseClick()
    {
        Close();
    }
}
