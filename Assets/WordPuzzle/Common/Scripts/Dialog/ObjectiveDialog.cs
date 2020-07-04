using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField]
    private Image _iconTaskDaily;
    [SerializeField]
    private Image _iconTaskAchie;
    [Space]
    [SerializeField] private Color _colorOff;
    [SerializeField] private Color _colorOn;
    [SerializeField] private Sprite _spriteTaskOn;
    [SerializeField] private Sprite _spriteTaskOff;
    [Space]
    [SerializeField] private List<Quest> _dailys;
    [SerializeField] private List<Quest> _achievements;
    [Header("UI THEME")]
    [SerializeField] private Image _iconStar;
    [SerializeField] private Image _iconAdd;
    [SerializeField] private Image _bgCurrency;
    [SerializeField] private TextMeshProUGUI _textNumberStar;

    HomeController homecontroller;

    protected override void Start()
    {
        base.Start();
        if (MainController.instance != null)
        {
            MainController.instance.canvasCollect.gameObject.SetActive(true);
            CheckTheme();
        }
        if (HomeController.instance != null)
            homecontroller = HomeController.instance;
        CheckTaskComplete();
        TurnOnIconTask(_iconTaskAchie, false);
        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
            SetTabActive(dailyTask, dailyBtn, true);
            SetTabActive(achievement, achieveBtn, false);
        });
    }

    private void Update()
    {
        CheckTaskComplete();
    }

    private void CheckTheme()
    {
        var currTheme = ThemesControl.instance.CurrTheme;

        _iconStar.sprite = currTheme.uiData.iconStar;
        _iconAdd.sprite = currTheme.uiData.iconAdd;
        _bgCurrency.sprite = currTheme.uiData.bgCurrency;

        _iconStar.SetNativeSize();
        _iconAdd.SetNativeSize();
        _bgCurrency.SetNativeSize();

        _textNumberStar.font = currTheme.fontData.fontAsset;
        _textNumberStar.fontSizeMax = currTheme.fontData.fontSizeMaxNumStar;
        _textNumberStar.color = currTheme.fontData.colorTextNumStar;
    }

    public void OnDailyOpen()
    {
        SetTabActive(dailyTask, dailyBtn, true);
        SetTabActive(achievement, achieveBtn, false);
        TurnOnIconTask(_iconTaskDaily, true);
        TurnOnIconTask(_iconTaskAchie, false);
        DialogCallEventFirebase("DailyTab");
    }
    public void OnAchiveveOpen()
    {
        SetTabActive(dailyTask, dailyBtn, false);
        SetTabActive(achievement, achieveBtn, true);
        TurnOnIconTask(_iconTaskAchie, true);
        TurnOnIconTask(_iconTaskDaily, false);
        DialogCallEventFirebase("AchiveveTab");
    }

    public void OnAcceptClick()
    {
        gameObject.GetComponent<Dialog>().Close();
        if (homecontroller != null)
            homecontroller.OnClick(0);
    }

    private void TurnOnIconTask(Image icon, bool turnOn)
    {
        icon.sprite = turnOn ? _spriteTaskOn : _spriteTaskOff;
        icon.SetNativeSize();
    }

    private void CheckTaskComplete()
    {
        if (!_iconTaskDaily.gameObject.activeSelf && !_iconTaskAchie.gameObject.activeSelf && !ObjectiveManager.instance.Icon.activeSelf)
            return;
        var hasTaskDailyComplete = _dailys.Any(task => task.taskComplete && !task.taskCollected);
        var hasTaskAchieComplete = _achievements.Any(task => task.taskComplete && !task.taskCollected);
        _iconTaskDaily.gameObject.SetActive(hasTaskDailyComplete);
        _iconTaskAchie.gameObject.SetActive(hasTaskAchieComplete);
    }

    void SetTabActive(GameObject tab, GameObject tabBtn, bool status)
    {
        tab.SetActive(status);
        tabBtn.transform.Find("On").gameObject.SetActive(status);
        tabBtn.transform.Find("IconOn").gameObject.SetActive(status);
        tabBtn.transform.Find("IconOff").gameObject.SetActive(!status);
        if (status)
        {
            tabBtn.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = _colorOn;

        }
        else
        {
            tabBtn.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = _colorOff;
        }
    }

    public override void Close()
    {
        base.Close();
        //if (MainController.instance != null)
        //    MainController.instance.beeController.OnBeeButtonClick();
    }
}
