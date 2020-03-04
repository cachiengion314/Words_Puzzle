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

    Queue<Quest> curDailyquests;
    Queue<Quest> hideQuests;
    DateTime nextDay;
    DateTime nowDay;
    bool isUpdateDailyQuest;
    bool once;
    bool isClick;


    private void Start()
    {
        once = true;
        isUpdateDailyQuest = false;
        curDailyquests = new Queue<Quest>();
        hideQuests = new Queue<Quest>();
        foreach(Transform child in dailyTaskContent.transform)
        {
            child.gameObject.SetActive(false);
            Quest quest = child.gameObject.GetComponent<Quest>();
            curDailyquests.Enqueue(quest);
        }


        //Debug.Log(nextDay);
        UpdateNextDay();
        Debug.Log(nextDay);
        UpdateDailyQuest();
    }
    private void Update()
    {
        //UpdateDailyQuest();
        //Debug.Log("hideQuests " + hideQuests.Count);
        //Debug.Log("curDailyquests" + curDailyquests.Count);

    }
    void DailyActive()
    {
        once = false; 
        for (int i = 0; i < 3; i++)
        {
            Quest questActived = curDailyquests.Dequeue();
            hideQuests.Enqueue(questActived);
            questActived.gameObject.SetActive(true);

        }
    }
    void UpdateDailyQuest()
    {
        //UpdateNextDay();
        if (DateTime.Compare(DateTime.Now, nextDay) > 0)
        {
            Debug.Log("jsdjsajdasd");
            once = true;
            isUpdateDailyQuest = true;
            foreach (Quest quest in hideQuests)
            {
                quest.gameObject.SetActive(false);
                Quest newQuest = hideQuests.Dequeue();
                curDailyquests.Enqueue(newQuest);
            }
            DailyActive();
            UpdateNextDay();
        }
        else
        {
            DailyActive();
            UpdateNextDay();
            isUpdateDailyQuest = false;
        }

    }

    void UpdateNextDay()
    {
        if (once)
        {
            Debug.Log("nextDay is default");
            nextDay = DateTime.Today.AddDays(1);
            once = false;
        }
        else if (isUpdateDailyQuest)
        {
            Debug.Log("Update day complete");
            nextDay = DateTime.Today.AddDays(1);
        }
    }


}
