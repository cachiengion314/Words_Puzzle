using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class WordDoneByPlayerToggle : MonoBehaviour
{
    public Image wordNormal;
    public Image wordDone;
    public TextMeshProUGUI txtWord;

    private Toggle thisToggle;
    private void Awake()
    {
        thisToggle = GetComponent<Toggle>();
    }
    private void OnDestroy()
    {
        LevelWordFeedbackDialog.toggleList.Clear();
    }
    public void RegisterWord()
    {
        if (thisToggle.isOn)
        {
            if (!LevelWordFeedbackDialog.toggleList.Contains(thisToggle))
                LevelWordFeedbackDialog.toggleList.Add(thisToggle);
        }
        else
        {
            if (LevelWordFeedbackDialog.toggleList.Count > 0)
                LevelWordFeedbackDialog.toggleList.Remove(thisToggle);
        }

    }
}
