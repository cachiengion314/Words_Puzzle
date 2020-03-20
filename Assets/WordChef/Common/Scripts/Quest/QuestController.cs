using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class QuestController : MonoBehaviour
{
    [SerializeField]
    GameObject dailyTaskContent;
    [SerializeField]
    GameObject achievementContent;
    [SerializeField] private float _timeRefresh = 12;
    [SerializeField] private TextMeshProUGUI _textRealtime;
    private List<Quest> _oldQuest;
    private Stack<Quest> curDailyquests;
    private DateTime nextDay;
    private double valueTime;

    void Awake()
    {
        curDailyquests = new Stack<Quest>(dailyTaskContent.transform.childCount);
        for (int i = 0; i < dailyTaskContent.transform.childCount; i++)
        {
            var qs = dailyTaskContent.transform.GetChild(i).gameObject.GetComponent<Quest>();
            qs.Run();
            qs.gameObject.SetActive(false);
            curDailyquests.Push(qs);
        }
        for (int i = 0; i < achievementContent.transform.childCount; i++)
        {
            var quest = achievementContent.transform.GetChild(i).gameObject.GetComponent<Quest>();
            quest.Run();
        }
        UpdateNextDay();
        Debug.Log(nextDay);
    }

    void Start()
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

    void DailyActive()
    {
        _oldQuest = new List<Quest>();
        for (int i = 0; i < dailyTaskContent.transform.childCount; i++)
        {
            var quest = curDailyquests.Pop();
            quest.gameObject.SetActive(true);
            quest.Run();
            _oldQuest.Add(quest);
        }
    }

    private void ShuffleTask()
    {
        for (int i = 0; i < curDailyquests.Count; i++)
        {
            var quest = curDailyquests.Pop();
            quest.Refresh();
            quest.gameObject.SetActive(false);
            _oldQuest.Add(quest);
        }
    }
    void UpdateDailyQuest()
    {
        if (DateTime.Compare(DateTime.Now, nextDay) >= 0)
        {
            ShuffleTask();
            foreach (var qs in _oldQuest)
            {
                qs.Refresh();
                qs.gameObject.SetActive(false);
                curDailyquests.Push(qs);
            }
            DailyActive();
            UpdateNextDay();
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
        }
    }


}
