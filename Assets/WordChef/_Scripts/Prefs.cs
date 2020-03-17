using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Prefs {

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
                int value = CPlayerPrefs.GetInt("Spelling_goal");
                GameState.countSpell = value;
            }
            return GameState.countSpell;
        }
        set { CPlayerPrefs.SetInt("Spelling_goal", value); GameState.countSpell = value; }
    }
    public static int countLevel
    {
        get
        {
            if (GameState.countLevel == -1)
            {
                int value = CPlayerPrefs.GetInt("Level_Amount");
                GameState.countLevel = value;
            }
            return GameState.countLevel;
        }
        set { CPlayerPrefs.SetInt("Level_Amount", value); GameState.countLevel = value; }
    }

    public static int countChapter
    {
        get
        {
            if (GameState.countChapter == -1)
            {
                int value = CPlayerPrefs.GetInt("Chapter_Amount");
                GameState.countChapter = value;
            }
            return GameState.countChapter;
        }
        set { CPlayerPrefs.SetInt("Chapter_Amount", value); GameState.countChapter = value; }
    }

    public static int countGood
    {
        get
        {
            if (GameState.goodCount == -1)
            {
                int value = CPlayerPrefs.GetInt("Good_Amount");
                GameState.goodCount = value;
            }
            return GameState.goodCount;
        }
        set { CPlayerPrefs.SetInt("Good_Amount", value); GameState.goodCount = value; }
    }
    public static int countGreat
    {
        get
        {
            if (GameState.greatCount == -1)
            {
                int value = CPlayerPrefs.GetInt("Great_Amount");
                GameState.greatCount = value;
            }
            return GameState.greatCount;
        }
        set { CPlayerPrefs.SetInt("Great_Amount", value); GameState.greatCount = value; }
    }
    public static int countAmazing
    {
        get
        {
            if (GameState.amazingCount == -1)
            {
                int value = CPlayerPrefs.GetInt("Amazing_Amount");
                GameState.amazingCount = value;
            }
            return GameState.amazingCount;
        }
        set { CPlayerPrefs.SetInt("Amazing_Amount", value); GameState.amazingCount = value; }
    }
    public static int countAwesome
    {
        get
        {
            if (GameState.awesomeCount == -1)
            {
                int value = CPlayerPrefs.GetInt("Awesome_Amount");
                GameState.awesomeCount = value;
            }
            return GameState.awesomeCount;
        }
        set { CPlayerPrefs.SetInt("Awesome_Amount", value); GameState.awesomeCount = value; }
    }
    public static int countExcellent
    {
        get
        {
            if (GameState.excelentCount == -1)
            {
                int value = CPlayerPrefs.GetInt("Excellent_Amount");
                GameState.excelentCount = value;
            }
            return GameState.excelentCount;
        }
        set { CPlayerPrefs.SetInt("Excellent_Amount", value); GameState.excelentCount = value; }
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
        return  GameState.currentWorld == unlockedWorld &&
                GameState.currentSubWorld == unlockedSubWorld && 
                GameState.currentLevel == unlockedLevel;
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
