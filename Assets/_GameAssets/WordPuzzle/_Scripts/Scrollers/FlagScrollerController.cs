using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;

public class FlagScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller enhancedScroller;
    [SerializeField] private FlagScrollerCellView _flagItemPfb;

    public void InitFlagScroller()
    {
        enhancedScroller.Delegate = this;
        enhancedScroller.ReloadData();
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        FlagScrollerCellView flagScrollerCellView = scroller.GetCellView(_flagItemPfb) as FlagScrollerCellView;
        flagScrollerCellView.SpawnFlagToCell(dataIndex);
        return flagScrollerCellView;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return (_flagItemPfb.transform as RectTransform).sizeDelta.y;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return FlagTabController.instance.flagItemList.Count / 2;
    }

    public FlagScrollerCellView JumScrollToIndex(int index)
    {
        enhancedScroller.JumpToDataIndex(index);
        return enhancedScroller.GetCellViewAtDataIndex(index) as FlagScrollerCellView;
    }


}
