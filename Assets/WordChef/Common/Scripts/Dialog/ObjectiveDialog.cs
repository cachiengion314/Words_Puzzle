using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveDialog : Dialog
{
    [SerializeField]
    GameObject dailyTask;
    [SerializeField]
    GameObject achievement;
    [SerializeField]
    GameObject dailyBtn;
    [SerializeField]
    GameObject achieveBtn;

    GameObject homecontroller;
    
    protected override void Start()
    {
        base.Start();
        homecontroller = GameObject.FindGameObjectWithTag("HomeController");
        SetTabActive(dailyTask, dailyBtn, true);
        SetTabActive(achievement, achieveBtn, false);


    }
    public void OnDailyOpen()
    {
        SetTabActive(dailyTask, dailyBtn, true);
        SetTabActive(achievement, achieveBtn, false);
    }
    public void OnAchiveveOpen()
    {
        SetTabActive(dailyTask, dailyBtn, false);
        SetTabActive(achievement, achieveBtn, true);
    }
    
    public void OnAcceptClick()
    {
        gameObject.GetComponent<Dialog>().Close();
        homecontroller.GetComponent<HomeController>().OnClick(0);
    }


    void SetTabActive(GameObject tab, GameObject tabBtn, bool status)
    {
        tab.SetActive(status);
        tabBtn.transform.Find("On").gameObject.SetActive(status);
        if(status)
            tabBtn.transform.Find("Text").GetComponent<Text>().color = new Vector4(1f, 1f, 1f, 1f);
        else
            tabBtn.transform.Find("Text").GetComponent<Text>().color = new Vector4(0,0,0,1f);
    }
}
