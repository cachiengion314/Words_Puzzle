using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Create Themes", order = 1)]
public class ThemesData : ScriptableObject
{
    public string nameTheme = "Woody";
    public bool showAnsScale;
    public float sizeTextPreviewSpacing = 200f;
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
    public int fontSizeMaxCell = 44;
    public int fontSizeMaxNumStar = 36;
    public Color colorLetter;
    public Color colorCell;
    public Color colorTextHeader;
    public Color colorTextNumStar;
    public Color colorNotify;
}

[Serializable]
public class UIData
{
    public Sprite iconStarFly;
    [Header("Play")]
    public bool showShadow = true;
    public bool levelTitleCenter = false;
    //public bool showLeaf = true;
    public bool showIconLevelTitle = true;
    public bool boardTextPreviewCenter;
    public Vector3 scaleTextPreview;
    public Vector3 posTextPreview;
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
    public Sprite bgTutorialLine;
    public Sprite iconHoney;
    public Sprite bgHoney;
    [Header("Home")]
    public bool showShadowHome = true;
    public Sprite bgHome;
    public Sprite shadowCurrencyHome;
    [Header("Dialog")]
    public Sprite bgBoardDialog;
    public Sprite imageTitleDialog;
    public Sprite btnCloseDialog;
    public ObjectivesData objectivesData;
}

[Serializable]
public class AnimData
{
    public string skinAnim;
    public string boneFx;
}

[Serializable]
public class ObjectivesData
{
    public Sprite foreGround;
    public Sprite btnDailyOn;
    public Sprite iconDailyOn;
    public Sprite iconDailyOff;
    public Sprite btnAchiveOn;
    public Sprite iconAchiveOn;
    public Sprite iconAchiveOff;
    public Sprite bgQuestDaily;
    public Sprite bgQuestAchive;
    public Sprite progressMask;
    public Sprite imageProgress;
    public Sprite btnGo;
    public Sprite btnReward;
    public Sprite iconComplete;
    public Sprite iconStar;
    [Space]
    public Sprite spelling;
    public Sprite levelClear;
    public Sprite chappterClear;
    public Sprite extraWord;
    public Sprite booster;
    public Sprite levelMisspelling;
    public Sprite amazing;
    public Sprite awesome;
    public Sprite excelent;
    public Sprite good;
    public Sprite great;
}