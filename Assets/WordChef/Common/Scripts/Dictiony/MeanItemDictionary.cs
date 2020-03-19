using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeanItemDictionary : MonoBehaviour
{
    public NestedScrollRect _nestedScrollRect;

    public Text meanText;
    public void SetParentNestedScrollRect(ScrollRect parent)
    {
        _nestedScrollRect.m_parentScrollRect = parent;
        _nestedScrollRect.InitParentScrollRect();
    }

    public void SetMeanText(string text)
    {
        meanText.text = text;
    }
}
