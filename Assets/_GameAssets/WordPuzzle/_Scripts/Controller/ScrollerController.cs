using EnhancedUI.EnhancedScroller;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] private GameData _data;
    public EnhancedScroller enhancedScroller;
    public WorldItem itemPfb;
    public WorldController worldController;
    public int target;

    void Start()
    {
        worldController.worldItems = new List<WorldItem>();
        if (Prefs.unlockedWorld >= _data.words.Count)
            Prefs.unlockedWorld = _data.words.Count - 1;
        target = Prefs.unlockedSubWorld + Prefs.unlockedWorld * _data.words[0].subWords.Count;
        enhancedScroller.Delegate = this;
        enhancedScroller.ReloadData();
        enhancedScroller.JumpToDataIndex(target);
        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
            worldController.worldItems[target].OnButtonClick();
            SceneAnimate.Instance.ShowTip(false);
        });
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        WorldItem wordItem = scroller.GetCellView(itemPfb) as WorldItem;
        var indexWord = dataIndex / _data.words[0].subWords.Count;
        var indexSubWord = dataIndex % _data.words[0].subWords.Count;
        wordItem.worldController = worldController;
        wordItem.world = indexWord;
        wordItem.subWorld = indexSubWord;
        wordItem.totalSubword = _data.words[0].subWords.Count;
        wordItem.Setup();
        worldController.worldItems.Add(wordItem);
        return wordItem;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return itemPfb.bg.rectTransform.sizeDelta.y;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        var result = /*_data.words.Count * _data.words[0].subWords.Count*/worldController.TotalChapter;
        return result;
    }

}
