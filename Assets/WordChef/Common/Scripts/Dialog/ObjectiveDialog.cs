using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [Space]
    [SerializeField] private Color _colorOff;
    [SerializeField] private Color _colorOn;

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
        if (homecontroller != null)
            homecontroller.GetComponent<HomeController>().OnClick(0);
    }


    void SetTabActive(GameObject tab, GameObject tabBtn, bool status)
    {
        tab.SetActive(status);
        tabBtn.transform.Find("On").gameObject.SetActive(status);
        tabBtn.transform.Find("IconOn").gameObject.SetActive(status);
        tabBtn.transform.Find("IconOff").gameObject.SetActive(!status);
        if (status)
            tabBtn.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = _colorOn;
        else
            tabBtn.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = _colorOff;
    }

    public override void Close()
    {
        base.Close();
        //if (MainController.instance != null)
        //    MainController.instance.beeController.OnBeeButtonClick();
    }
}
