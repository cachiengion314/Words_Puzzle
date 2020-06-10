using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;

    public ObjectiveData objectiveData;

    void Awake()
    {
        if (instance != null)
            instance = this;
    }

    public void CheckTaskComplete()
    {
        if (Prefs.countLevelDaily >= objectiveData.dailyDatas[0])
            Debug.Log("Daily 0 complete !");
    }
}

[Serializable]
public class ObjectiveData
{
    public List<int> dailyDatas;
    public List<int> achievementsDatas;
}