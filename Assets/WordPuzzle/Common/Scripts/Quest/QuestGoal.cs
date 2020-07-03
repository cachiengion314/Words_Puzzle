using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public GoalType goalType;
    public int reward;
    public int requiredAmount;
    public int amountResetup;
    int _currentAmountSpell;

    public bool isReached()
    {
        return (_currentAmountSpell >= requiredAmount);
    }

    void CountSpellingGoal()
    {
        if (!CPlayerPrefs.HasKey("Spelling_goal"))
        {
            CPlayerPrefs.SetInt("Spelling_goal", 0);
            _currentAmountSpell = 0;

        }
        else
        {
            _currentAmountSpell = CPlayerPrefs.GetInt("Spelling_goal");
            _currentAmountSpell++;
            CPlayerPrefs.SetInt("Spelling_goal", _currentAmountSpell);
        }
    }


}
public enum GoalType
{
    Spelling,
    LevelClear,
    Combos,
    ChappterClear,
    ExtraWord,
    Booster,
    LevelMisspelling
}
public enum ComboType
{
    amazing,
    awesome,
    excelent,
    good,
    great
}
public enum TaskType
{
    DAILY,
    ACHIEVEMENT
}
