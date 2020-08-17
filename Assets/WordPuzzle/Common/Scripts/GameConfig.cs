using UnityEngine;
using System;

[System.Serializable]
public class GameConfig
{
    public GameParameters gameParameters;
    public UnityAdsId unityAdsId;
    public FacebookAdsId facebookAdsId;
    public Admob admob;

    [Header("")]
    public int adPeriod;
    public int rewardedVideoPeriod;
    public int rewardedVideoAmount;
    public string androidPackageID;
    public string iosAppID;
    public string macAppID;
    public string facebookPageID;

    [Header("")]
    public int fontSizeInDiskSelectLevel;
    public int fontSizeInDiskMainScene;
    public int fontSizeInCellMainScene;
    [Header("")]
    public bool isWordRightToLeft = false;
}

[System.Serializable]
public class Admob
{
    [HideInInspector] public string rewardedLevel;
    [HideInInspector] public string rewardedChapter;
    [HideInInspector] public string rewardedBonusBox;
    [HideInInspector] public string rewardedFreeBoosters;
    [HideInInspector] public string rewardedFreeStars;
    [HideInInspector] public string rewardedFreeLetter;
    [HideInInspector] public string interstitialLevel;
    [HideInInspector] public string bannerLevel;
}
[System.Serializable]
public class UnityAdsId
{
    [HideInInspector] public string interstitialLevel = "levelTransition";

    [HideInInspector] public string rewardedChapter = "chapterClear";
    [HideInInspector] public string rewardedLevel = "levelClear";
    [HideInInspector] public string rewardedBonusBox = "bonusBox";
    [HideInInspector] public string rewardedFreeLetter = "freeLetter";
    [HideInInspector] public string rewardedFreeStars = "freeStars";
    [HideInInspector] public string rewardedFreeBoosters = "freeBoosters";
}

[System.Serializable]
public class FacebookAdsId
{
    [HideInInspector] public string intersititial = "670089180215126_670102936880417";

    [HideInInspector] public string rewardedBonusBox = "670089180215126_670109416879769";
    [HideInInspector] public string rewardedChapterClear = "670089180215126_670109660213078";
    [HideInInspector] public string rewardedLevelClear = "670089180215126_670109563546421";
    [HideInInspector] public string rewardedFreeBoosters = "670089180215126_670106246880086";
    [HideInInspector] public string rewardedFreeStars = "670089180215126_670103063547071";
    [HideInInspector] public string rewardedFreeLetter = "670089180215126_670089596881751";
}

[System.Serializable]
public class GameParameters
{
    public int rewardedBeeAmount = 2;
    public int maxBank = 2000;
    public int minBank = 720;
    public int rewardMultipleHintDaily = 2;
    public int rewardHintDaily = 5;
}
