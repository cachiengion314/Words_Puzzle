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
    [SerializeField] private Image _foreGround;
    [SerializeField] private Image _btnDailyOn;
    [SerializeField] private Image _iconDailyOn;
    [SerializeField] private Image _iconDailyOff;
    [SerializeField] private Image _btnAchiveOn;
    [SerializeField] private Image _iconAchiveOn;
    [SerializeField] private Image _iconAchiveOff;
    [SerializeField] private Image _bgNote;
    [SerializeField] private TextMeshProUGUI _textTimeNote;
    [SerializeField] private TextMeshProUGUI _textRefreshNote;

    HomeController homecontroller;

    protected override void Awake()
    {
        base.Awake();
        CheckTheme();
    }

    protected override void Start()
    {
        base.Start();
        if (MainController.instance != null)
        {
            MainController.instance.canvasCollect.gameObject.SetActive(true);
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
        _colorOff = currTheme.fontData.colorContentDialog;
        _iconStar.sprite = currTheme.uiData.iconStar;
        _iconAdd.sprite = currTheme.uiData.iconAdd;
        _bgCurrency.sprite = currTheme.uiData.bgCurrency;

        _iconStar.SetNativeSize();
        _iconAdd.SetNativeSize();
        _bgCurrency.SetNativeSize();

        _textNumberStar.font = currTheme.fontData.fontAsset;
        _textNumberStar.fontSizeMax = currTheme.fontData.fontSizeMaxNumStar;
        _textNumberStar.color = currTheme.fontData.colorTextNumStar;
        _textRefreshNote.color = currTheme.fontData.colorContentDialog;
        _textTimeNote.color = currTheme.fontData.colorContentDialog;

        _foreGround.sprite = currTheme.uiData.objectivesData.foreGround;
        _btnDailyOn.sprite = currTheme.uiData.objectivesData.btnDailyOn;
        _iconDailyOn.sprite = currTheme.uiData.objectivesData.iconDailyOn;
        _iconDailyOff.sprite = currTheme.uiData.objectivesData.iconDailyOff;

        _btnAchiveOn.sprite = currTheme.uiData.objectivesData.btnAchiveOn;
        _iconAchiveOn.sprite = currTheme.uiData.objectivesData.iconAchiveOn;
        _iconAchiveOff.sprite = currTheme.uiData.objectivesData.iconAchiveOff;
        _bgNote.sprite = currTheme.uiData.objectivesData.bgNote;

        foreach (var task in _dailys)
        {
            task.bgQuest.sprite = currTheme.uiData.objectivesData.bgQuestDaily;
            task.LoadThemeData();
            CheckIconTask(task, currTheme);
        }
        foreach (var task in _achievements)
        {
            task.bgQuest.sprite = currTheme.uiData.objectivesData.bgQuestAchive;
            task.LoadThemeData();
            CheckIconTask(task, currTheme);
        }
    }

    private void CheckIconTask(Quest task, ThemesData currTheme)
    {
        if (task.goal.goalType == GoalType.Spelling)
            task.SpriteTask = currTheme.uiData.objectivesData.spelling;
        else if (task.goal.goalType == GoalType.LevelClear)
            task.SpriteTask = currTheme.uiData.objectivesData.levelClear;
        else if (task.goal.goalType == GoalType.ChappterClear)
            task.SpriteTask = currTheme.uiData.objectivesData.chappterClear;
        else if (task.goal.goalType == GoalType.Booster)
            task.SpriteTask = currTheme.uiData.objectivesData.booster;
        else if (task.goal.goalType == GoalType.ExtraWord)
            task.SpriteTask = currTheme.uiData.objectivesData.extraWord;
        else if (task.goal.goalType == GoalType.LevelMisspelling)
            task.SpriteTask = currTheme.uiData.objectivesData.levelMisspelling;
        else if (task.goal.goalType == GoalType.Combos && task.combo == ComboType.good)
            task.SpriteTask = currTheme.uiData.objectivesData.good;
        else if (task.goal.goalType == GoalType.Combos && task.combo == ComboType.great)
            task.SpriteTask = currTheme.uiData.objectivesData.great;
        else if (task.goal.goalType == GoalType.Combos && task.combo == ComboType.amazing)
            task.SpriteTask = currTheme.uiData.objectivesData.amazing;
        else if (task.goal.goalType == GoalType.Combos && task.combo == ComboType.awesome)
            task.SpriteTask = currTheme.uiData.objectivesData.awesome;
        else if (task.goal.goalType == GoalType.Combos && task.combo == ComboType.excelent)
            task.SpriteTask = currTheme.uiData.objectivesData.excelent;
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
