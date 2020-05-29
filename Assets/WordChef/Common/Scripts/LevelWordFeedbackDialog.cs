using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelWordFeedbackDialog : FeedbackDialog
{
    public List<string> correctWordsDoneByPlayerList = new List<string>();
    public List<Transform> textTransformPosList = new List<Transform>();
    public Transform textBackgroundPrefab;
    public Button wordDoneByPlayerPrefab;
    protected override void Start()
    {
        base.Start();
        foreach (Transform transform in textBackgroundPrefab)
        {
            textTransformPosList.Add(transform);
        }
        for (int i = 0; i < correctWordsDoneByPlayerList.Count; i++)
        {
            TextMeshProUGUI text = Instantiate(wordDoneByPlayerPrefab, textTransformPosList[i].position, Quaternion.identity).GetComponent<TextMeshProUGUI>();
            text.text = correctWordsDoneByPlayerList[i].ToString();
        }
    }
    public void WordsCorrectDoneByPlayer()
    {
        for (int j = 0; j < WordRegion.instance.listWordCorrect.Count; j++)
        {
            string word = WordRegion.instance.listWordCorrect[j];
            correctWordsDoneByPlayerList.Add(word);
        }
    }
    public new void OnCloseClick()
    {
        Close();
    }
}
