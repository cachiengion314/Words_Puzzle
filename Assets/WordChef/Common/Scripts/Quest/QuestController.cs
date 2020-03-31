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

    void Start()
    {
        if (!CPlayerPrefs.HasKey("DAILY_DATA"))
            CPlayerPrefs.SetInt("DAILY_DATA", UnityEngine.Random.Range(0, _dailyTaskDatas.Count));
        indexData = CPlayerPrefs.GetInt("DAILY_DATA", 0);
        for (int i = 0; i < _dailyTaskDatas[indexData].quests.Count; i++)
        {
            var qs = _dailyTaskDatas[indexData].quests[i];
            qs.Run();
            qs.gameObject.SetActive(false);
        }
        for (int i = 0; i < achievementContent.transform.childCount; i++)
        {
            var quest = achievementContent.transform.GetChild(i).gameObject.GetComponent<Quest>();
            quest.Run();
        }
        UpdateNextDay();
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

    void DailyActive()
    {
        for (int i = 0; i < _dailyTaskDatas.Count; i++)
        {
            if (i == indexData)
            {
                foreach (var ques in _dailyTaskDatas[indexData].quests)
                {
                    ques.gameObject.SetActive(true);
                    ques.Run();
                }
            }
            else
            {
                foreach (var ques in _dailyTaskDatas[i].quests)
                {
                    ques.Refresh();
                    ques.gameObject.SetActive(false);
                }
            }
        }
    }

    void UpdateDailyQuest()
    {
        if (DateTime.Compare(DateTime.Now, nextDay) >= 0)
        {
            UpdateNextDay();
            DailyActive();
        }
        else
        {
            DailyActive();
        }
        StartCoroutine(CountDownTimeRefresh());
    }

    void UpdateNextDay()
    {
        var timeRefresh = DateTime.Now.Date + TimeSpan.FromSeconds(_timeRefresh * 3600);
        if (CPlayerPrefs.HasKey("DAY_REFRESH"))
        {
            var time = CPlayerPrefs.GetLong("DAY_REFRESH");
            nextDay = DateTime.FromBinary(time);
        }
        else
        {
            nextDay = DateTime.FromBinary(timeRefresh.Ticks);
            CPlayerPrefs.SetLong("DAY_REFRESH", timeRefresh.Ticks);
        }
        if (DateTime.Compare(DateTime.Now, nextDay) > 0)
        {
            nextDay = DateTime.Today.AddDays(1) + TimeSpan.FromSeconds(_timeRefresh * 3600);
            CPlayerPrefs.SetLong("DAY_REFRESH", nextDay.Ticks);
            CPlayerPrefs.SetInt("DAILY_DATA", UnityEngine.Random.Range(0, _dailyTaskDatas.Count));
            indexData = CPlayerPrefs.GetInt("DAILY_DATA");
        }
    }


}

[Serializable]
public class DailyTaskData
{
    public List<Quest> quests;
}