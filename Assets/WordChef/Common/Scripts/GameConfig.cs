using UnityEngine;
using System;

[System.Serializable]
public class GameConfig
{
    public GameParameters gameParameters;
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
    [Header("Banner")]
    public string androidBanner;
    public string iosBanner;
    [Header("Interstitial")]
    public string androidInterstitial;
    public string iosInterstitial;
    [Header("RewardedVideo")]
    public string androidRewarded;
    public string iosRewarded;
}
[System.Serializable]
public class FacebookAdsId
{
    public string intersititial = "670089180215126_670102936880417";

    public string rewardedBonusBox = "670089180215126_670109416879769";
    public string rewardedChapterClear = "670089180215126_670109660213078";
    public string rewardedLevelClear = "670089180215126_670109563546421";
    public string rewardedFreeBoosters = "670089180215126_670106246880086";
    public string rewardedFreeStars = "670089180215126_670103063547071";
    public string rewardedFreeLetter = "670089180215126_670089596881751";
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
