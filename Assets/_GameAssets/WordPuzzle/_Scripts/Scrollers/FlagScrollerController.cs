using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;

public class FlagScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller enhancedScroller;

    public void InitFlagScroller()
    {
        enhancedScroller.Delegate = this;
        enhancedScroller.ReloadData();
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        return null;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return (DictionaryDialog.instance.flagItemPrefab.transform as RectTransform).sizeDelta.y;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return FlagTabController.instance.flagItemList.Count;
    }

    
}
