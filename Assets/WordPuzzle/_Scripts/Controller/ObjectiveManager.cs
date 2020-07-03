using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;
    [SerializeField] private GameObject icon;
    public ObjectiveData objectiveData;

    public GameObject Icon
    {
        get
        {
            return icon;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        LoadDataAchive();
    }

    void Start()
    {
        CheckTaskComplete();
    }

    public void CheckTaskComplete()
    {
        if ((Prefs.countLevelDaily >= objectiveData.dailyDatas[0] && !CPlayerPrefs.GetBool("Completed_Daily_" + 0, false))||
            (Prefs.countAmazingDaily >= objectiveData.dailyDatas[1] && !CPlayerPrefs.GetBool("Completed_Daily_" + 1, false)) ||
            (Prefs.countSpellDaily >= objectiveData.dailyDatas[2] && !CPlayerPrefs.GetBool("Completed_Daily_" + 2, false)) ||
            (Prefs.countLevel >= objectiveData.achievementsDatas[0]) ||
            (Prefs.countGreat >= objectiveData.achievementsDatas[1]) ||
            (Prefs.countAmazing >= objectiveData.achievementsDatas[2]) ||
            (Prefs.countAwesome >= objectiveData.achievementsDatas[3]) ||
            (Prefs.countExcellent >= objectiveData.achievementsDatas[4]) ||
            (Prefs.countSpell >= objectiveData.achievementsDatas[5]) ||
            (Prefs.countExtra >= objectiveData.achievementsDatas[6]) ||
            (Prefs.countBooster >= objectiveData.achievementsDatas[7]) ||
            (Prefs.countLevelMisspelling >= objectiveData.achievementsDatas[8]))
            ShowIcon(true);
        else
            ShowIcon(false);
    }

    public bool CheckTaskAchie()
    {
        if ((Prefs.countLevel >= objectiveData.achievementsDatas[0]) ||
            (Prefs.countGreat >= objectiveData.achievementsDatas[1]) ||
            (Prefs.countAmazing >= objectiveData.achievementsDatas[2]) ||
            (Prefs.countAwesome >= objectiveData.achievementsDatas[3]) ||
            (Prefs.countExcellent >= objectiveData.achievementsDatas[4]) ||
            (Prefs.countSpell >= objectiveData.achievementsDatas[5]) ||
            (Prefs.countExtra >= objectiveData.achievementsDatas[6]) ||
            (Prefs.countBooster >= objectiveData.achievementsDatas[7]) ||
            (Prefs.countLevelMisspelling >= objectiveData.achievementsDatas[8]))
            return true;
        else
            return false;
    }

    public void ResetupAchie(int index, int value)
    {
        objectiveData.achievementsDatas[index] = value;
        CPlayerPrefs.SetInt("OBJECTIVE_ACHIVE_" + index, value);
    }

    private void ShowIcon(bool show)
    {
        icon.SetActive(show);
    }

    private void LoadDataAchive()
    {
        for (int i = 0; i < objectiveData.achievementsDatas.Count; i++)
        {
            var index = i;
            var task = objectiveData.achievementsDatas[index];
            var result = CPlayerPrefs.GetInt("OBJECTIVE_ACHIVE_" + index, task);
            task = result;
        }
    }
}

[Serializable]
public class ObjectiveData
{
    public List<int> dailyDatas;
    public List<int> achievementsDatas;
}