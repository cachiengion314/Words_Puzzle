﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Create Themes", order = 1)]
public class ThemesData : ScriptableObject
{
    public bool showAnsScale;
    public FontData fontData;
    public UIData uiData;
    public AnimData animData;
}

[Serializable]
public class FontData
{
    public TMP_FontAsset fontAsset;
    public Font fontNormal;
}

[Serializable]
public class UIData
{
    public bool showShadow = true;
    public Sprite background;
    public Sprite boardWordRegion;
    public Sprite header;
    public Sprite iconStar;
    public Sprite iconAdd;
    public Sprite bgCurrency;
    public Sprite bgLevelTitle;
    public Sprite btnDictionary;
    public Sprite iconDictionary;
    public Sprite btnSetting;
    public Sprite iconSetting;
    public Sprite iconCoinCell;
    public Sprite bgCell;
    public Sprite bgCellDone;
    public Sprite imgPedestal;
    public Sprite bgLetter;
}

[Serializable]
public class AnimData
{
    public string skinAnim;
}