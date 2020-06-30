using System;
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
    public bool fontScale = true;
    public TMP_FontAsset fontAsset;
    public Font fontNormal;
    public int fontSize;
    public int fontSizeMax = 44;
    public Color colorLetter;
    public Color colorCell;
    public Color colorTextHeader;
}

[Serializable]
public class UIData
{
    public bool showShadow = true;
    public bool showLeaf = true;
    public bool showIconLevelTitle = true;
    public Sprite background;
    public Sprite boardWordRegion;
    public Sprite header;
    public Sprite iconStar;
    public Sprite iconAdd;
    public Sprite bgCurrency;
    public Sprite bgLevelTitle;
    public Sprite btnDictionary;
    public Sprite iconDictionary;
    public Sprite imgGround;
    public Sprite imgBgTextPreview;
    public Sprite imgBgCellPreview;
    public Sprite btnSetting;
    public Sprite iconSetting;
    public Sprite iconCoinCell;
    public Sprite imgCell;
    public Sprite bgCell;
    public Sprite bgCellDone;
    public Sprite imgPedestal;
    public Sprite bgLetter;
    public Sprite numBooster;
    public Sprite priceBooster;
    public Sprite frameFxExist;
    public Sprite bgToast;
}

[Serializable]
public class AnimData
{
    public string skinAnim;
}