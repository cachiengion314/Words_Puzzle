using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class QuestController : MonoBehaviour
{
    [SerializeField] private List<DailyTaskData> _dailyTaskDatas;
    [SerializeField]
    GameObject dailyTaskContent;
    [SerializeField]
    GameObject achievementContent;
    [SerializeField] private float _timeRefresh = 12;
    [SerializeField] private TextMeshProUGUI _textRealtime;
    private List<Quest> _oldQuest;
    private DateTime nextDay;
    private double valueTime;
    private int indexData;

    void OnEnable()
    {
        if (!CPlayerPrefs.HasKey("DAILY_DATA"))
            CPlayerPrefs.SetInt("DAILY_DATA", UnityEngine.Random.Range(0, _dailyTaskDatas.Count));
        indexData = CPlayerPrefs.GetInt("DAILY_DATA", 0);
        for (int i = 0; i < _dailyTaskDatas[indexData].quests.Count; i++)
        {
            var qs = _dailyTaskDatas[indexData].quests[i];
            qs.Run();
            qs.gameObject.SetActive(true);
        }
        for (int i = 0; i < achievementContent.transform.childCount; i++)
        {
            var quest = achievementContent.transform.GetChild(i).gameObject.GetComponent<Quest>();
            quest.Run();
            quest.gameObject.SetActive(true);
        }
        UpdateNextDay();
    }

    private void Start()
    {
        UpdateDailyQuest();
    }


    private IEnumerator CountDownTimeRefresh()
    {
        while (DateTime.Compare(DateTime.Now, nextDay) < 0)
        {
            var result = nextDay - DateTime.Now;
            valueTime = (int)(result.TotalSeconds);
            _textRealtime.text = TimeSpan.FromSeconds(valueTime).ToString();
            yield return new WaitForSeconds(1);
            if (valueTime <= 0)
                UpdateDailyQuest();
        }
    }

    void DailyActive(bool refresh = false)
    {
        for (int i = 0; i < dailyTaskContent.transform.childCount; i++)
        {
            var quest = dailyTaskContent.transform.GetChild(i).GetComponent<Quest>();
            if (refresh)
                quest.Refresh();
            quest.gameObject.SetActive(false);
        }

        foreach (var ques in _dailyTaskDatas[indexData].quests)
        {
            ques.gameObject.SetActive(true);
            ques.Run();
        }

        if (!CPlayerPrefs.HasKey("OBJ_TUTORIAL") && Prefs.countLevelDaily >= _dailyTaskDatas[0].quests[0].goal.requiredAmount)
        {
            var canvas = _dailyTaskDatas[0].quests[0].GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingLayerName = "UI2";
            BlockScreen.instance.Block(true);
            TweenControl.GetInstance().DelayCall(transform, 0.5f, () =>
            {
                TutorialController.instance.ShowPopObjectiveTut();
                BlockScreen.instance.Block(false);
            });
        }
    }

    void UpdateDailyQuest()
    {
        if (DateTime.Compare(DateTime.Now, nextDay) >= 0)
        {
            CPlayerPrefs.SetBool("IS_REFRESH", false);
            UpdateNextDay();
            DailyActive(true);
        }
        else
        {
            DailyActive();
        }
        StartCoroutine(CountDownTimeRefresh());
    }

    void UpdateNextDay()
    {
        var isRefresh = CPlayerPrefs.GetBool("IS_REFRESH", false);
        var timeRefresh = DateTime.Now.Date + TimeSpan.FromSeconds(_timeRefresh * 3600);
        if (CPlayerPrefs.HasKey("DAY_REFRESH"))
        {
            var time = CPlayerPrefs.GetLong("DAY_REFRESH");
            nextDay = DateTime.FromBinary(time);
            Debug.Log("NextDay: " + nextDay);
        }
        else
        {
            nextDay = DateTime.FromBinary(timeRefresh.Ticks);
            CPlayerPrefs.SetLong("DAY_REFRESH", timeRefresh.Ticks);
            Debug.Log("NextDay New: " + nextDay);
        }
        if (DateTime.Compare(DateTime.Now, nextDay) > 0 && !isRefresh)
        {
            CPlayerPrefs.SetBool("IS_REFRESH", true);
            nextDay = DateTime.Now.Date.AddDays(1)/* + TimeSpan.FromSeconds(_timeRefresh * 3600)*/;
            Debug.Log("NextDay New Refresh: " + nextDay);
            CPlayerPrefs.SetLong("DAY_REFRESH", nextDay.Ticks);
            CPlayerPrefs.SetInt("DAILY_DATA", UnityEngine.Random.Range(0, _dailyTaskDatas.Count));
            indexData = CPlayerPrefs.GetInt("DAILY_DATA", 0);
            DailyActive(true);
        }
    }

    private void OnDisable()
    {
        TweenControl.GetInstance().KillDelayCall(transform);
    }
}

[Serializable]
public class DailyTaskData
{
    public List<Quest> quests;
}