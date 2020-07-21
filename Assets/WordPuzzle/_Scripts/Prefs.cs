using Superpow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Prefs
{

    public static int unlockedWorld
    {
        get
        {
            if (GameState.unlockedWorld == -1)
            {
                int value = CPlayerPrefs.GetInt("unlocked_world");
                GameState.unlockedWorld = value;
            }
            return GameState.unlockedWorld;
        }
        set { CPlayerPrefs.SetInt("unlocked_world", value); GameState.unlockedWorld = value; }
    }

    public static int unlockedSubWorld
    {
        get
        {
            if (GameState.unlockedSubWord == -1)
            {
                int value = CPlayerPrefs.GetInt("unlocked_sub_world");
                GameState.unlockedSubWord = value;
            }
            return GameState.unlockedSubWord;
        }
        set { CPlayerPrefs.SetInt("unlocked_sub_world", value); GameState.unlockedSubWord = value; }
    }

    public static int unlockedLevel
    {
        get
        {
            if (GameState.unlockedLevel == -1)
            {
                int value = CPlayerPrefs.GetInt("unlocked_level");
                GameState.unlockedLevel = value;
            }
            return GameState.unlockedLevel;
        }
        set { CPlayerPrefs.SetInt("unlocked_level", value); GameState.unlockedLevel = value; }
    }

    #region Objective Task

    public static int countSpell
    {
        get
        {
            if (GameState.countSpell == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.SPELLING);
                GameState.countSpell = value;
            }
            return GameState.countSpell;
        }
        set { CPlayerPrefs.SetInt(Const.SPELLING, value); GameState.countSpell = value; }
    }
    public static int countLevel
    {
        get
        {
            if (GameState.countLevel == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.LEVEL_CLEAR);
                GameState.countLevel = value;
            }
            return GameState.countLevel;
        }
        set { CPlayerPrefs.SetInt(Const.LEVEL_CLEAR, value); GameState.countLevel = value; }
    }

    public static int countChapter
    {
        get
        {
            if (GameState.countChapter == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.CHAPTER_CLEAR);
                GameState.countChapter = value;
            }
            return GameState.countChapter;
        }
        set { CPlayerPrefs.SetInt(Const.CHAPTER_CLEAR, value); GameState.countChapter = value; }
    }

    public static int countExtra
    {
        get
        {
            if (GameState.countExtra == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.EXTRA_WORD);
                GameState.countExtra = value;
            }
            return GameState.countExtra;
        }
        set { CPlayerPrefs.SetInt(Const.EXTRA_WORD, value); GameState.countExtra = value; }
    }

    public static int countBooster
    {
        get
        {
            if (GameState.countBooster == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.BOOSTER);
                GameState.countBooster = value;
            }
            return GameState.countBooster;
        }
        set { CPlayerPrefs.SetInt(Const.BOOSTER, value); GameState.countBooster = value; }
    }

    public static int countLevelMisspelling
    {
        get
        {
            if (GameState.countLevelMisspelling == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.LEVEL_MISSPELLING);
                GameState.countLevelMisspelling = value;
            }
            return GameState.countLevelMisspelling;
        }
        set { CPlayerPrefs.SetInt(Const.LEVEL_MISSPELLING, value); GameState.countLevelMisspelling = value; }
    }

    public static int countGood
    {
        get
        {
            if (GameState.goodCount == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.GOOD_COMBO);
                GameState.goodCount = value;
            }
            return GameState.goodCount;
        }
        set { CPlayerPrefs.SetInt(Const.GOOD_COMBO, value); GameState.goodCount = value; }
    }
    public static int countGreat
    {
        get
        {
            if (GameState.greatCount == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.GREAT_COMBO);
                GameState.greatCount = value;
            }
            return GameState.greatCount;
        }
        set { CPlayerPrefs.SetInt(Const.GREAT_COMBO, value); GameState.greatCount = value; }
    }
    public static int countAmazing
    {
        get
        {
            if (GameState.amazingCount == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.AMAZING_COMBO);
                GameState.amazingCount = value;
            }
            return GameState.amazingCount;
        }
        set { CPlayerPrefs.SetInt(Const.AMAZING_COMBO, value); GameState.amazingCount = value; }
    }
    public static int countAwesome
    {
        get
        {
            if (GameState.awesomeCount == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.AWESOME_COMBO);
                GameState.awesomeCount = value;
            }
            return GameState.awesomeCount;
        }
        set { CPlayerPrefs.SetInt(Const.AWESOME_COMBO, value); GameState.awesomeCount = value; }
    }
    public static int countExcellent
    {
        get
        {
            if (GameState.excelentCount == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.EXCELLENT_COMBO);
                GameState.excelentCount = value;
            }
            return GameState.excelentCount;
        }
        set { CPlayerPrefs.SetInt(Const.EXCELLENT_COMBO, value); GameState.excelentCount = value; }
    }

    #endregion

    #region DailyTask
    public static int countSpellDaily
    {
        get
        {
            if (GameState.countSpellDaily == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.SPELLING_DAILY);
                GameState.countSpellDaily = value;
            }
            return GameState.countSpellDaily;
        }
        set { CPlayerPrefs.SetInt(Const.SPELLING_DAILY, value); GameState.countSpellDaily = value; }
    }
    public static int countLevelDaily
    {
        get
        {
            if (GameState.countLevelDaily == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.LEVEL_CLEAR_DAILY);
                GameState.countLevelDaily = value;
            }
            return GameState.countLevelDaily;
        }
        set { CPlayerPrefs.SetInt(Const.LEVEL_CLEAR_DAILY, value); GameState.countLevelDaily = value; }
    }

    public static int countChapterDaily
    {
        get
        {
            if (GameState.countChapterDaily == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.CHAPTER_CLEAR_DAILY);
                GameState.countChapterDaily = value;
            }
            return GameState.countChapterDaily;
        }
        set { CPlayerPrefs.SetInt(Const.CHAPTER_CLEAR_DAILY, value); GameState.countChapterDaily = value; }
    }

    public static int countExtraDaily
    {
        get
        {
            if (GameState.countExtraDaily == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.EXTRA_WORD_DAILY);
                GameState.countExtraDaily = value;
            }
            return GameState.countExtraDaily;
        }
        set { CPlayerPrefs.SetInt(Const.EXTRA_WORD_DAILY, value); GameState.countExtraDaily = value; }
    }

    public static int countBoosterDaily
    {
        get
        {
            if (GameState.countBoosterDaily == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.BOOSTER_DAILY);
                GameState.countBoosterDaily = value;
            }
            return GameState.countBoosterDaily;
        }
        set { CPlayerPrefs.SetInt(Const.BOOSTER_DAILY, value); GameState.countBoosterDaily = value; }
    }

    public static int countLevelMisspellingDaily
    {
        get
        {
            if (GameState.countLevelMisspellingDaily == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.LEVEL_MISSPELLING_DAILY);
                GameState.countLevelMisspellingDaily = value;
            }
            return GameState.countLevelMisspellingDaily;
        }
        set { CPlayerPrefs.SetInt(Const.LEVEL_MISSPELLING_DAILY, value); GameState.countLevelMisspellingDaily = value; }
    }

    public static int countGoodDaily
    {
        get
        {
            if (GameState.goodCountDaily == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.GOOD_COMBO_DAILY);
                GameState.goodCountDaily = value;
            }
            return GameState.goodCountDaily;
        }
        set { CPlayerPrefs.SetInt(Const.GOOD_COMBO_DAILY, value); GameState.goodCountDaily = value; }
    }
    public static int countGreatDaily
    {
        get
        {
            if (GameState.greatCountDaily == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.GREAT_COMBO_DAILY);
                GameState.greatCountDaily = value;
            }
            return GameState.greatCountDaily;
        }
        set { CPlayerPrefs.SetInt(Const.GREAT_COMBO_DAILY, value); GameState.greatCountDaily = value; }
    }
    public static int countAmazingDaily
    {
        get
        {
            if (GameState.amazingCountDaily == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.AMAZING_COMBO_DAILY);
                GameState.amazingCountDaily = value;
            }
            return GameState.amazingCountDaily;
        }
        set { CPlayerPrefs.SetInt(Const.AMAZING_COMBO_DAILY, value); GameState.amazingCountDaily = value; }
    }
    public static int countAwesomeDaily
    {
        get
        {
            if (GameState.awesomeCountDaily == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.AWESOME_COMBO_DAILY);
                GameState.awesomeCountDaily = value;
            }
            return GameState.awesomeCountDaily;
        }
        set { CPlayerPrefs.SetInt(Const.AWESOME_COMBO_DAILY, value); GameState.awesomeCountDaily = value; }
    }
    public static int countExcellentDaily
    {
        get
        {
            if (GameState.excelentCountDaily == -1)
            {
                int value = CPlayerPrefs.GetInt(Const.EXCELLENT_COMBO_DAILY);
                GameState.excelentCountDaily = value;
            }
            return GameState.excelentCountDaily;
        }
        set { CPlayerPrefs.SetInt(Const.EXCELLENT_COMBO_DAILY, value); GameState.excelentCountDaily = value; }
    }
    #endregion

    public static List<int> GetPanWordIndexes(int world, int subWorld, int level)
    {
        string data = PlayerPrefs.GetString("pan_word_indexes_v2_" + world + "_" + subWorld + "_" + level);
        return CUtils.BuildListFromString<int>(data);
    }

    public static void SetPanWordIndexes(int world, int subWorld, int level, int[] indexes)
    {
        string data = CUtils.BuildStringFromCollection(indexes);
        PlayerPrefs.SetString("pan_word_indexes_v2_" + world + "_" + subWorld + "_" + level, data);
    }

    public static bool IsLastLevel()
    {
        return GameState.currentWorld > unlockedWorld &&
                GameState.currentSubWorld > unlockedSubWorld &&
                GameState.currentLevel > unlockedLevel;
    }

    public static bool IsSaveLevelProgress()
    {
        var numlevels = Utils.GetNumLevels(Prefs.unlockedWorld, Prefs.unlockedSubWorld);
        var currlevel = (Prefs.unlockedLevel + numlevels * Prefs.unlockedSubWorld + MainController.instance.gameData.words[0].subWords.Count * numlevels * Prefs.unlockedWorld) + 1;
        var lastWord = MainController.instance.gameData.words[MainController.instance.gameData.words.Count - 1];
        var lastLevel = lastWord.subWords[lastWord.subWords.Count - 1].gameLevels[lastWord.subWords[lastWord.subWords.Count - 1].gameLevels.Count - 1];

        return GameState.currentWorld >= unlockedWorld &&
            GameState.currentSubWorld >= unlockedSubWorld &&
            GameState.currentLevel >= unlockedLevel && currlevel > lastLevel.level;
    }

    public static void SetExtraWords(int world, int subWorld, int level, string[] extraWords)
    {
        CryptoPlayerPrefsX.SetStringArray("extra_words_" + world + "_" + subWorld + "_" + level, extraWords);
    }

    public static string[] GetExtraWords(int world, int subWorld, int level)
    {
        return CryptoPlayerPrefsX.GetStringArray("extra_words_" + world + "_" + subWorld + "_" + level);
    }

    public static int extraProgress
    {
        get { return CPlayerPrefs.GetInt("extra_progress", 0); }
        set { CPlayerPrefs.SetInt("extra_progress", value); }
    }

    public static int extraTarget
    {
        get { return CPlayerPrefs.GetInt("extra_target", 5); }
        set { CPlayerPrefs.SetInt("extra_target", value); }
    }

    public static int totalExtraAdded
    {
        get { return CPlayerPrefs.GetInt("total_extra_added", 0); }
        set { CPlayerPrefs.SetInt("total_extra_added", value); }
    }

    public static string[] levelProgress
    {
        get { return CryptoPlayerPrefsX.GetStringArray("level_progress"); }
        set { CryptoPlayerPrefsX.SetStringArray("level_progress", value); }
    }

    public static string[] answersProgress
    {
        get { return CryptoPlayerPrefsX.GetStringArray("answer_progress"); }
        set { CryptoPlayerPrefsX.SetStringArray("answer_progress", value); }
    }

    public static bool isNoti1Enabled
    {
        get { return PlayerPrefs.GetInt("is_noti_1_enabled") == 1; }
        set { PlayerPrefs.SetInt("is_noti_1_enabled", value ? 1 : 0); }
    }

    public static bool isNoti2Enabled
    {
        get { return PlayerPrefs.GetInt("is_noti_2_enabled") == 1; }
        set { PlayerPrefs.SetInt("is_noti_2_enabled", value ? 1 : 0); }
    }

    public static int noti3Ruby
    {
        get { return PlayerPrefs.GetInt("noti_3_ruby"); }
        set { PlayerPrefs.SetInt("noti_3_ruby", value); }
    }

    public static int noti4Ruby
    {
        get { return PlayerPrefs.GetInt("noti_4_ruby"); }
        set { PlayerPrefs.SetInt("noti_4_ruby", value); }
    }

    public static int noti5Ruby
    {
        get { return PlayerPrefs.GetInt("noti_5_ruby"); }
        set { PlayerPrefs.SetInt("noti_5_ruby", value); }
    }

    public static int noti6Ruby
    {
        get { return PlayerPrefs.GetInt("noti_6_ruby"); }
        set { PlayerPrefs.SetInt("noti_6_ruby", value); }
    }

    public static int noti7Ruby
    {
        get { return PlayerPrefs.GetInt("noti_7_ruby"); }
        set { PlayerPrefs.SetInt("noti_7_ruby", value); }
    }

    public static void AddToNumHint(int world, int subWorld, int level)
    {
        int numHint = GetNumHint(world, subWorld, level);
        PlayerPrefs.SetInt("numhint_used_" + world + "_" + subWorld + "_" + level, numHint + 1);
    }

    public static int GetNumHint(int world, int subWorld, int level)
    {
        return PlayerPrefs.GetInt("numhint_used_" + world + "_" + subWorld + "_" + level);
    }
}
