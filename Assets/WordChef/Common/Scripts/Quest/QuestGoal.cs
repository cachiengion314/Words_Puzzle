using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public GoalType goalType;
    public int reward;
    public int requiredAmount;
    int currentAmount;
    
    
    bool isReached()
    {        
       
        return (currentAmount >= requiredAmount);
    }

    void CountSpellingGoal()
    {
        if (!CPlayerPrefs.HasKey("Spelling_goal"))
        {
            CPlayerPrefs.SetInt("Spelling_goal", 0);
            currentAmount = 0;

        } else
        {
            currentAmount = CPlayerPrefs.GetInt("Spelling_goal");
            currentAmount++;
            CPlayerPrefs.SetInt("Spelling_goal", currentAmount);   
        }
    } 

        
}
public enum GoalType
{
    Spelling,
    LevelClear,
    Combos
}
public enum ComboType
{
    amazing,
    awesome,
    excelent,
    good,
    great
}
