using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

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
    public Color colorContentDialog;
    public Color colorWin;
    public Color colorWhenPlusStar;
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
    public Gradient colorLinerender;
    public Color colorParticle;
    public Color colorParticle2;
    [Header("Home")]
    public bool showShadowHome = true;
    public Sprite bgHome;
    public Sprite shadowCurrencyHome;
    [Header("Dialog")]
    public Sprite bgBoardDialog;
    public Sprite imageTitleDialog;
    public Sprite btnCloseDialog;
    public ObjectivesData objectivesData;
    public MenuData menuData;
    public FreestarData freestarData;
    public ComingSoonData comingSoonData;
    public HelpData helpData;
    public FeedbackData feedbackData;
    public MissingWordData missingWordData;
    public LevelWordData levelWordData;
    public ContactData contactData;
    public MeanWordData meanWordData;
    public RewardDialogData rewardDialogData;
    public SettingData settingData;
    public FreestarPlayData freestarPlayData;
    public CollectFreestarPlayData collectFreestarPlayData;
    public BonusBoxData bonusBoxData;
    public HowtoplayData howtoplayData;
    public DontLikeAdsData dontLikeAdsData;
    public BeehiveData beehiveData;
}

[Serializable]
public class AnimData
{
    public string skinAnim;
    public string boneFx;
}

[Serializable]
public class MenuData
{
    public Sprite iconBtnNormal;
    public Sprite iconBtnTask;
    public Sprite iconBtnHelpFeddback;
    public Sprite iconHome;
    public Sprite iconTheme;
    public Sprite iconSetting;
    public Sprite iconObjective;
    public Sprite iconFeedback;
    public Sprite iconHelp;
}

[Serializable]
public class FreestarData
{
    public Sprite btnWatch;
    public Sprite iconAds;
    public Sprite iconStar;
    [Space]
    public Color colorTextBtn;
}

[Serializable]
public class ComingSoonData
{
    public Sprite message;
    public Sprite btnClose;
    public Color colorTextBtn;
}

[Serializable]
public class HelpData
{
    public Sprite btnNomal;
    public Sprite btnFb;
    public Sprite btnMessager;
    public Sprite btnMail;
    public Sprite iconOther;
    public Sprite iconFb;
    public Sprite iconMessager;
    public Sprite iconMail;
}

[Serializable]
public class FeedbackData
{
    public Sprite bgOption;
    public Sprite btnMissingWord;
    public Sprite btnLevelWord;
    public Sprite btnContact;
    public Sprite iconMissingWord;
    public Sprite iconLevelWord;
    public Sprite iconContact;
}

[Serializable]
public class MissingWordData
{
    public Sprite boardMissingWord;
    public Sprite btnSend;
    public Color colorTextBtn;
    public Color colorTextFiel;
}

[Serializable]
public class LevelWordData
{
    public Sprite btnSend;
    public Sprite boardWord;
    public Color colorTextBtn;
    public Sprite wordNormal;
    public Sprite wordDone;
    public Color colorTextWordPfb;
}

[Serializable]
public class ContactData
{
    public Sprite boardContact;
    public Sprite btnSend;
    public Color colorTextBtn;
    public Color colorTextFiel;
}

[Serializable]
public class MeanWordData
{
    public Sprite board;
    public Sprite boardContentDialog;
    public Sprite arrowLeft;
    public Sprite arrowRight;
    public Sprite selectedPanigation;
    public Sprite disablePanigation;
}

[Serializable]
public class RewardDialogData
{
    public Sprite btnCollect;
    public Color colorBtn;
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
    public Sprite bgNote;
    public Sprite bgQuest;
    public Sprite bgQuestDaily;
    public Sprite bgQuestAchive;
    public Sprite progressMask;
    public Sprite imageProgress;
    public Sprite bgProgress;
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
    [Space]
    public Color colorTextProgress;
    public Color colorTextBtnPlay;
    public Color colorTextBtnCollect;
}

[Serializable]
public class SettingData
{
    public Sprite iconSound;
    public Sprite iconMusic;
    public Sprite iconEffect;
    public Sprite iconNotification;
    public Sprite iconOn;
    public Sprite iconOff;
    public Sprite iconArrow;
    public Sprite handle;
    public Sprite bgProgress;
    public Sprite fillProgress;
    public Sprite frameMask;
    public Sprite line;
    public Sprite line2;
    public Sprite btnFeedback;
    public Sprite btnRate;
    public Color colorTextBtnFeedback;
    public Color colorTextBtnRate;
}

[Serializable]
public class FreestarPlayData
{
    public Sprite iconGift;
    public Sprite iconStar;
    public Sprite iconHint;
    public Sprite iconSelectedHint;
    public Sprite iconMultipleHint;
    public Sprite iconAds;
    public Sprite btnWatch;
    public Color colorTextBtn;
}

[Serializable]
public class CollectFreestarPlayData
{
    public Sprite iconGift;
    public Sprite iconStar;
    public Sprite btnCollect;
    public Color colorTextBtn;
}

[Serializable]
public class HowtoplayData
{
    public Sprite arrowLeft;
    public Sprite arrowRight;
    public Sprite hand;

    public Sprite iconButton;
    public Sprite iconHint;
    public Sprite iconSelectedHint;
    public Sprite iconMultipleHint;
    public Sprite iconShuffle;
    public Sprite iconCellsPreview;
    public Sprite iconCandy;
    public Sprite iconCellsMultipleHint;
    public Sprite iconCellsBee;
    public Sprite iconBee;
    public Sprite iconBeeBtn;
    public Sprite iconCellsEmpty;
    public Sprite iconCellsOpen;
    public Sprite iconBgLetter;
    public Sprite boardOverlay;
    public Sprite boardPreview;
    public List<Sprite> imagesCenter;
    public List<Sprite> imagesOther;

}

[Serializable]
public class BonusBoxData
{
    public Sprite btnHtpl;
    public Sprite board;
    public Sprite boardTitle;
    public Sprite iconStar;
    public Sprite bgProgress;
    public Sprite progressBar;
    public Sprite circleProgress;
    public Sprite btnCollect;
    public Sprite btnReward;
    public Sprite iconCandyStar;
    public Sprite iconAds;
    public bool alignCenterIcon;
    public Color colorTextBoardTitle;
    public Color colorTextBtn;
    public Color colorTextBtnReward;
}

[Serializable]
public class DontLikeAdsData
{
    public Sprite itemTop;
    public Sprite btnPrice;
    public Color colorTextBtn;
    public List<Sprite> iconsNoAds;
}

[Serializable]
public class BeehiveData
{
    public List<Sprite> imgsBeehive;
}