using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;
using System.Linq;

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
            groupWord.gameObject.SetActive(statusGroupWord);
        }
        else
        {
            textWordsNull.gameObject.SetActive(statusGroupWord);
        }
        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
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
        GameObject buttonWordClone;
        var _dictionaryDialog = DictionaryDialog.instance;
        var item = _dictionaryDialog.Itemsdictionary.ToList()[dataIndex];
        ClearAllChildGroupWord();
        foreach (var word in item.Value)
        {
            buttonWordClone = Instantiate(_dictionaryDialog.buttonWord, groupWord);
            buttonWordClone.transform.GetChild(0).GetComponent<Text>().text = word;
        }

        if (item.Value.Count > 0)
        {
            if (item.Value.Count == 1)
            {
                numberWordText.text = item.Value.Count + " word";
            }
            else
            {
                numberWordText.text = item.Value.Count + " words";
            }
            numberWord = item.Value.Count;
        }
        else
            numberWordText.text = "";

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
                groupWord.ResetState();
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
