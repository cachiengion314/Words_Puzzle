using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;

public class ListGroupWord : EnhancedScrollerCellView
{
    public LayoutElement layoutElement;
    public Transform groupWord;
    public Text textWordsNull;
    public Text firstButtonText;
    public Text numberWordText;
    [HideInInspector] public bool statusGroupWord = false;
    [HideInInspector] public int numberWord;

    [HideInInspector] public LayoutElement thisLayoutElement;

    private void Start()
    {
        thisLayoutElement = GetComponent<LayoutElement>();
        statusGroupWord = false;
    }

    public void OnButtonClick()
    {
        DictionaryDialog.instance.currListWord = this;
        //if (!statusGroupWord && numberWord > 0)
        //{
        //    Sound.instance.Play(Sound.Others.PopupOpen);
        //    DialogController.instance.ShowDialog(DialogType.OpenWordDictionary, DialogShow.STACK_DONT_HIDEN);
        //}
        //else
        //{
        CheckOpenListWord();
        //}
    }

    public void CheckOpenListWord()
    {
        CloseAllGroupWord();
        if (transform.GetChild(1).childCount > 0)
        {
            transform.GetChild(1).gameObject.SetActive(statusGroupWord);
        }
        else
        {
            transform.GetChild(2).gameObject.SetActive(statusGroupWord);
        }
        TweenControl.GetInstance().DelayCall(transform, 0.1f,()=> {
            if (groupWord.gameObject.activeInHierarchy || textWordsNull.gameObject.activeInHierarchy)
            {
                thisLayoutElement.minHeight = (textWordsNull.gameObject.activeInHierarchy ? textWordsNull.rectTransform.sizeDelta.y : 0) +
                    (groupWord.gameObject.activeInHierarchy ? (groupWord as RectTransform).sizeDelta.y : 0) + layoutElement.minHeight;
            }
            else
            {
                thisLayoutElement.minHeight = layoutElement.minHeight;
            }
        });
    }

    public override void RefreshCellView()
    {
        base.RefreshCellView();
        ResetState();
    }

    public void ResetState()
    {
        statusGroupWord = false;
        groupWord.gameObject.SetActive(false);
        textWordsNull.gameObject.SetActive(false);
        thisLayoutElement.minHeight = layoutElement.minHeight;
    }

    private void CloseAllGroupWord()
    {
        for (int i = 0; i < DictionaryDialog.instance.groupWords.Count; i++)
        {
            var groupWord = DictionaryDialog.instance.groupWords[i];
            if (groupWord != this)
            {
                groupWord.thisLayoutElement.minHeight = groupWord.layoutElement.minHeight;
                groupWord.transform.GetChild(1).gameObject.SetActive(false);
                groupWord.transform.GetChild(2).gameObject.SetActive(false);
                groupWord.statusGroupWord = false;
            }
        }
        statusGroupWord = !statusGroupWord;
    }

    public void ClearAllChildGroupWord()
    {
        for (int i = 0; i < groupWord.childCount; i++)
        {
            Destroy(groupWord.GetChild(i).gameObject);
        }
    }
}
