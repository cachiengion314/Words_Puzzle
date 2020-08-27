using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using System.Linq;
using UnityEngine.UI;
using System;

public class DictionaryScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller enhancedScroller;
    private DictionaryDialog _dictionaryDialog;

    public void InitDictionaryScroller()
    {
        _dictionaryDialog = DictionaryDialog.instance;
        _dictionaryDialog.Itemsdictionary.Distinct();
        enhancedScroller.Delegate = this;
        enhancedScroller.ReloadData();
        enhancedScroller.cellViewReused = OnCellViewReused;
        //enhancedScroller.scrollerScrollingChanged = OnScroll;
        enhancedScroller.ScrollRect.content.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    private void OnCellViewReused(EnhancedScroller scroller, EnhancedScrollerCellView cellView)
    {
        cellView.RefreshCellView();
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        GameObject buttonWordClone;
        ListGroupWord cellView = scroller.GetCellView(DictionaryDialog.instance.listGroupWord) as ListGroupWord;
        var item = _dictionaryDialog.Itemsdictionary.ToList()[dataIndex];
        DictionaryDialog.instance.groupWords.Add(cellView);
        cellView.firstButtonText.text = item.Key + ".";

        cellView.ClearAllChildGroupWord();
        foreach (var word in item.Value)
        {
            buttonWordClone = Instantiate(_dictionaryDialog.buttonWord, cellView.groupWord);
            buttonWordClone.transform.GetChild(0).GetComponent<Text>().text = word;
        }

        if (item.Value.Count > 0)
        {
            if (item.Value.Count == 1)
            {
                cellView.numberWordText.text = item.Value.Count + " word";
            }
            else
            {
                cellView.numberWordText.text = item.Value.Count + " words";
            }
            cellView.numberWord = item.Value.Count;
        }
        else
            cellView.numberWordText.text = "";
        return cellView;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return DictionaryDialog.instance.listGroupWord.layoutElement.minHeight;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return DictionaryDialog.instance.Itemsdictionary.Count;
    }
}
