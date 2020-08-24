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
    public string admob_level_transition = "ca-app-pub-3940256099942544/1033173712";

    public string admob_chapter_clear = "ca-app-pub-3940256099942544/5224354917";
    public string admob_level_clear = "ca-app-pub-3940256099942544/5224354917"; 
    public string admob_bonus_box = "ca-app-pub-3940256099942544/5224354917";
    public string admob_free_boosters = "ca-app-pub-3940256099942544/5224354917";
    public string admob_free_letter = "ca-app-pub-3940256099942544/5224354917";
    public string admob_free_stars = "ca-app-pub-3940256099942544/5224354917";

    public string admob_banner = "ca-app-pub-3212738706492790/6113697308";
}
[System.Serializable]
public class UnityAdsId
{
    public string interstitialLevel = "levelTransition";

    public string rewardedChapter = "chapterClear";
    public string rewardedLevel = "levelClear";
    public string rewardedBonusBox = "bonusBox";
    public string rewardedFreeLetter = "freeLetter";
    public string rewardedFreeStars = "freeStars";
    public string rewardedFreeBoosters = "freeBoosters";
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
