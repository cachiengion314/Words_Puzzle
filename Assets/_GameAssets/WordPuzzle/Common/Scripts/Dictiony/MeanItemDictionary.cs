using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeanItemDictionary : MonoBehaviour
{
    public NestedScrollRect _nestedScrollRect;

    public TextMeshProUGUI meanText;

    private void Start()
    {
        CheckTheme();
    }

    private void CheckTheme()
    {
        if(MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            meanText.color = currTheme.fontData.colorContentDialog;
        }
    }

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
