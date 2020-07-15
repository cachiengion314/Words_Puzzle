using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Utilities.Components;

[System.Serializable]
public class Quest : MonoBehaviour
{
    public int idQuest;
    public string nameTask;
    public Image bgQuest;
    public TaskType taskType;
    [Space]
    public QuestGoal goal;
    public ComboType combo;
    [SerializeField]
    private Sprite _spriteTask;
    [SerializeField]
    GameObject titleText;
    [SerializeField]
    GameObject rewardText;

    [SerializeField] private Slider _fillProgress;
    [SerializeField] private Image _progressMask;
    [SerializeField] private Image _imageProgress;
    [SerializeField] private Image _bgProgress;
    [SerializeField] private Button _btnGo;
    [SerializeField] private Button _btnReward;
    [SerializeField] private Image _iconComplete;
    [SerializeField] private Image _iconVComplete;
    [SerializeField] private Image _iconStar;
    [SerializeField] private Image _iconTask;
    [SerializeField] private TextMeshProUGUI _textProgress;

    [HideInInspector] public bool taskComplete;
    [HideInInspector] public bool taskCollected;
    RectTransform rt;
    float maxWidth;

    public Sprite SpriteTask
    {
        get
        {
            return _spriteTask;
        }
        set
        {
            _spriteTask = value;
        }
    }

    void Awake()
    {
        rt = _progressMask.GetComponent<RectTransform>();
        maxWidth = rt.rect.width;
    }

    void OnEnable()
    {
        if (taskType == TaskType.ACHIEVEMENT)
        {
            goal.requiredAmount = CPlayerPrefs.GetInt("Completed_" + idQuest, goal.requiredAmount);
            _fillProgress.maxValue = goal.requiredAmount;
        }
        UpdateProgress();
    }

    public void Run()
    {
        _iconTask.sprite = _spriteTask;
        _iconTask.SetNativeSize();
        _iconComplete.gameObject.SetActive(false);
        switch (taskType)
        {
            case TaskType.DAILY:
                ShowQuestDaily();
                break;
            case TaskType.ACHIEVEMENT:
                ShowQuestAchie();
                break;
        }
    }

    public void Refresh()
    {
        ClearTaskDailyByKey();
        ClearTask((taskType == TaskType.DAILY ? "Completed_Daily_" : "Completed_") + idQuest);
        ShowQuestDaily();
    }

    public void LoadThemeData()
    {
        var currTheme = ThemesControl.instance.CurrTheme;
        _btnGo.image.sprite = currTheme.uiData.objectivesData.btnGo;
        _btnReward.image.sprite = currTheme.uiData.objectivesData.btnReward;
        _iconVComplete.sprite = currTheme.uiData.objectivesData.iconComplete;
        _iconStar.sprite = currTheme.uiData.objectivesData.iconStar;
        _bgProgress.sprite = currTheme.uiData.objectivesData.bgProgress;
        _imageProgress.sprite = currTheme.uiData.objectivesData.imageProgress;
        _progressMask.sprite = currTheme.uiData.objectivesData.progressMask;
        _textProgress.color = currTheme.uiData.objectivesData.colorTextProgress;
        rewardText.GetComponent<TextMeshProUGUI>().color = currTheme.fontData.colorContentDialog;
        titleText.GetComponent<TextMeshProUGUI>().color = currTheme.fontData.colorContentDialog;

        _imageProgress.SetNativeSize();
        _progressMask.SetNativeSize();
        _bgProgress.SetNativeSize();
    }

    private void ResetupAchie()
    {
        goal.amountResetup = goal.requiredAmount * 2;
        goal.requiredAmount = goal.amountResetup;
        if (taskType == TaskType.ACHIEVEMENT)
            CPlayerPrefs.SetInt("Completed_" + idQuest, goal.requiredAmount);
        ClearTaskAchievementByKey();
        ShowQuestAchie();
        ObjectiveManager.instance.ResetupAchie(idQuest, goal.requiredAmount);
        nameTask = nameTask + goal.requiredAmount;
    }

    void Update()
    {
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        if (_fillProgress.value >= goal.requiredAmount)
        {
            _fillProgress.value = _fillProgress.maxValue;
            ShowReward(true);
        }
        _textProgress.text = _fillProgress.value + " / " + goal.requiredAmount;

        if (_fillProgress.maxValue == 0) return;

        float progress = Mathf.Clamp(_fillProgress.value / _fillProgress.maxValue, 0, 1);
        rt.sizeDelta = new Vector2(maxWidth * progress, rt.rect.height);
    }

    private void ShowReward(bool show)
    {
        taskComplete = show;
        _btnReward.gameObject.SetActive(show);
        _iconComplete.gameObject.SetActive(false);
        _btnGo.gameObject.SetActive(MainController.instance != null ? false : true);
        var iscompleted = CPlayerPrefs.GetBool((taskType == TaskType.DAILY ? "Completed_Daily_" : "Completed_") + idQuest, false);
        taskCollected = iscompleted;
        if (iscompleted)
        {
            _btnReward.gameObject.SetActive(false);
            _btnGo.gameObject.SetActive(false);
            _iconComplete.gameObject.SetActive(true);
            _fillProgress.value = _fillProgress.maxValue;
        }
    }


    public void OnReward()
    {
        if (MainController.instance != null)
        {
            MainController.instance.canvasFx.gameObject.SetActive(EffectController.instance.IsEffectOn);
            MainController.instance.canvasCollect.gameObject.SetActive(true);
        }
        if (GetComponent<Canvas>() != null)
            GetComponent<Canvas>().overrideSorting = false;
        CPlayerPrefs.SetBool("OBJ_TUTORIAL", true);
        if (TutorialController.instance != null)
            TutorialController.instance.HidenPopTut();
        StartCoroutine(ShowEffectCollect(goal.reward));
        //CurrencyController.CreditBalance(goal.reward);
        _btnReward.gameObject.SetActive(false);
        _iconComplete.gameObject.SetActive(true);
        CPlayerPrefs.SetBool((taskType == TaskType.DAILY ? "Completed_Daily_" : "Completed_") + idQuest, true);
        switch (taskType)
        {
            case TaskType.DAILY:
                ClearTaskDailyByKey();
                break;
            case TaskType.ACHIEVEMENT:
                ResetupAchie();
                break;
        }
        ObjectiveManager.instance.CheckTaskComplete();
        if (PauseDialog.instance != null)
            PauseDialog.instance.CheckShowIconTaskComplete();
        Firebase.Analytics.FirebaseAnalytics.LogEvent(
          Firebase.Analytics.FirebaseAnalytics.EventUnlockAchievement,
          new Firebase.Analytics.Parameter[] {
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterAchievementId, nameTask),
          }
        );
    }

    private IEnumerator ShowEffectCollect(int value)
    {
        MonoUtils.instance.ShowTotalStarCollect(value, null);
        var result = value / 5;
        for (int i = 0; i < value; i++)
        {
            if (i < 5)
            {
                MonoUtils.instance.ShowEffect(result, null, null, _btnReward.transform);
            }
            yield return new WaitForSeconds(0.06f);
        }
    }

    void ShowQuestDaily()
    {
        //gameObject.GetComponent<SimpleTMPButton>().labelTMP.SetText("X" + goal.requiredAmount);
        _fillProgress.maxValue = goal.requiredAmount;
        switch (goal.goalType)
        {
            case GoalType.Spelling:
                titleText.GetComponent<TextMeshProUGUI>().text = "Spell " + goal.requiredAmount + " Word" + ((goal.requiredAmount == 1) ? "" : "s");
                rewardText.GetComponent<TextMeshProUGUI>().text = "X" + goal.reward.ToString();
                _fillProgress.value = Prefs.countSpellDaily;
                break;
            case GoalType.LevelClear:
                titleText.GetComponent<TextMeshProUGUI>().text = "Clear " + goal.requiredAmount + " level" + ((goal.requiredAmount == 1) ? "" : "s");
                rewardText.GetComponent<TextMeshProUGUI>().text = "X" + goal.reward.ToString();
                _fillProgress.value = Prefs.countLevelDaily;
                break;
            case GoalType.ChappterClear:
                titleText.GetComponent<TextMeshProUGUI>().text = "Clear " + goal.requiredAmount + " Chapter" + ((goal.requiredAmount == 1) ? "" : "s");
                rewardText.GetComponent<TextMeshProUGUI>().text = "X" + goal.reward.ToString();
                _fillProgress.value = Prefs.countChapterDaily;
                break;
            case GoalType.ExtraWord:
                titleText.GetComponent<TextMeshProUGUI>().text = "Collect " + goal.requiredAmount + " extra word" + ((goal.requiredAmount == 1) ? "" : "s");
                rewardText.GetComponent<TextMeshProUGUI>().text = "X" + goal.reward.ToString();
                _fillProgress.value = Prefs.countExtraDaily;
                break;
            case GoalType.Booster:
                titleText.GetComponent<TextMeshProUGUI>().text = "Use " + goal.requiredAmount + " booster" + ((goal.requiredAmount == 1) ? "" : "s");
                rewardText.GetComponent<TextMeshProUGUI>().text = "X" + goal.reward.ToString();
                _fillProgress.value = Prefs.countBoosterDaily;
                break;
            case GoalType.LevelMisspelling:
                titleText.GetComponent<TextMeshProUGUI>().text = "Clear any " + goal.requiredAmount + " level" + ((goal.requiredAmount == 1) ? "" : "s") + " without misspelling";
                rewardText.GetComponent<TextMeshProUGUI>().text = "X" + goal.reward.ToString();
                _fillProgress.value = Prefs.countLevelMisspellingDaily;
                break;
            case GoalType.Combos:
                if (combo == ComboType.amazing)
                {
                    titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " AMAZING combo" + ((goal.requiredAmount == 1) ? "" : "s");
                    _fillProgress.value = Prefs.countAmazingDaily;
                }
                else if (combo == ComboType.awesome)
                {
                    titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " AWESOME combo" + ((goal.requiredAmount == 1) ? "" : "s");
                    _fillProgress.value = Prefs.countAwesomeDaily;
                }
                else if (combo == ComboType.excelent)
                {
                    titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " EXCELENT combo" + ((goal.requiredAmount == 1) ? "" : "s");
                    _fillProgress.value = Prefs.countExcellentDaily;
                }
                else if (combo == ComboType.good)
                {
                    titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " GOOD combo" + ((goal.requiredAmount == 1) ? "" : "s");
                    _fillProgress.value = Prefs.countGoodDaily;
                }
                else if (combo == ComboType.great)
                {
                    titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " GREAT combo" + ((goal.requiredAmount == 1) ? "" : "s");
                    _fillProgress.value = Prefs.countGreatDaily;
                }
                rewardText.GetComponent<TextMeshProUGUI>().text = "X" + goal.reward.ToString();
                break;
        }
        if (_fillProgress.value < _fillProgress.maxValue)
            ShowReward(false);
    }

    void ShowQuestAchie()
    {
        //gameObject.GetComponent<SimpleTMPButton>().labelTMP.SetText("X" + goal.requiredAmount);
        if (taskType == TaskType.ACHIEVEMENT)
        {
            goal.requiredAmount = CPlayerPrefs.GetInt("Completed_" + idQuest, goal.requiredAmount);
            nameTask = nameTask + goal.requiredAmount;
        }
        _fillProgress.maxValue = goal.requiredAmount;
        if (_fillProgress.value < _fillProgress.maxValue)
        {
            switch (goal.goalType)
            {
                case GoalType.Spelling:
                    titleText.GetComponent<TextMeshProUGUI>().text = "Spell " + goal.requiredAmount + " Word" + ((goal.requiredAmount == 1) ? "" : "s");
                    rewardText.GetComponent<TextMeshProUGUI>().text = "X" + goal.reward.ToString();
                    _fillProgress.value = Prefs.countSpell;
                    break;
                case GoalType.LevelClear:
                    titleText.GetComponent<TextMeshProUGUI>().text = "Clear " + goal.requiredAmount + " level" + ((goal.requiredAmount == 1) ? "" : "s");
                    rewardText.GetComponent<TextMeshProUGUI>().text = "X" + goal.reward.ToString();
                    _fillProgress.value = Prefs.countLevel;
                    break;
                case GoalType.ChappterClear:
                    titleText.GetComponent<TextMeshProUGUI>().text = "Clear " + goal.requiredAmount + " Chapter" + ((goal.requiredAmount == 1) ? "" : "s");
                    rewardText.GetComponent<TextMeshProUGUI>().text = "X" + goal.reward.ToString();
                    _fillProgress.value = Prefs.countChapter;
                    break;
                case GoalType.ExtraWord:
                    titleText.GetComponent<TextMeshProUGUI>().text = "Collect " + goal.requiredAmount + " extra word" + ((goal.requiredAmount == 1) ? "" : "s");
                    rewardText.GetComponent<TextMeshProUGUI>().text = "X" + goal.reward.ToString();
                    _fillProgress.value = Prefs.countExtra;
                    break;
                case GoalType.Booster:
                    titleText.GetComponent<TextMeshProUGUI>().text = "Use " + goal.requiredAmount + " booster" + ((goal.requiredAmount == 1) ? "" : "s");
                    rewardText.GetComponent<TextMeshProUGUI>().text = "X" + goal.reward.ToString();
                    _fillProgress.value = Prefs.countBooster;
                    break;
                case GoalType.LevelMisspelling:
                    titleText.GetComponent<TextMeshProUGUI>().text = "Clear any " + goal.requiredAmount + " level" + ((goal.requiredAmount == 1) ? "" : "s") + " without misspelling";
                    rewardText.GetComponent<TextMeshProUGUI>().text = "X" + goal.reward.ToString();
                    _fillProgress.value = Prefs.countLevelMisspelling;
                    break;
                case GoalType.Combos:
                    if (combo == ComboType.amazing)
                    {
                        titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " AMAZING combo" + ((goal.requiredAmount == 1) ? "" : "s");
                        _fillProgress.value = Prefs.countAmazing;
                    }
                    else if (combo == ComboType.awesome)
                    {
                        titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " AWESOME combo" + ((goal.requiredAmount == 1) ? "" : "s");
                        _fillProgress.value = Prefs.countAwesome;
                    }
                    else if (combo == ComboType.excelent)
                    {
                        titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " EXCELENT combo" + ((goal.requiredAmount == 1) ? "" : "s");
                        _fillProgress.value = Prefs.countExcellent;
                    }
                    else if (combo == ComboType.good)
                    {
                        titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " GOOD combo" + ((goal.requiredAmount == 1) ? "" : "s");
                        _fillProgress.value = Prefs.countGood;
                    }
                    else if (combo == ComboType.great)
                    {
                        titleText.GetComponent<TextMeshProUGUI>().text = "Get " + goal.requiredAmount + " GREAT combo" + ((goal.requiredAmount == 1) ? "" : "s");
                        _fillProgress.value = Prefs.countGreat;
                    }
                    rewardText.GetComponent<TextMeshProUGUI>().text = "X" + goal.reward.ToString();
                    break;
            }
            ShowReward(false);
        }
    }

    private void ClearTaskDailyByKey()
    {
        switch (goal.goalType)
        {
            case GoalType.Spelling:
                GameState.countSpellDaily = -1;
                ClearTask(Const.SPELLING_DAILY);
                break;
            case GoalType.LevelClear:
                GameState.countLevelDaily = -1;
                ClearTask(Const.LEVEL_CLEAR_DAILY);
                break;
            case GoalType.ChappterClear:
                GameState.countChapterDaily = -1;
                ClearTask(Const.CHAPTER_CLEAR_DAILY);
                break;
            case GoalType.ExtraWord:
                GameState.countExtraDaily = -1;
                ClearTask(Const.EXTRA_WORD_DAILY);
                break;
            case GoalType.Booster:
                GameState.countBoosterDaily = -1;
                ClearTask(Const.BOOSTER_DAILY);
                break;
            case GoalType.LevelMisspelling:
                GameState.countLevelMisspellingDaily = -1;
                ClearTask(Const.LEVEL_MISSPELLING_DAILY);
                break;
            case GoalType.Combos:
                if (combo == ComboType.amazing)
                {
                    GameState.amazingCountDaily = -1;
                    ClearTask(Const.AMAZING_COMBO_DAILY);
                }
                else if (combo == ComboType.awesome)
                {
                    GameState.awesomeCountDaily = -1;
                    ClearTask(Const.AWESOME_COMBO_DAILY);
                }
                else if (combo == ComboType.excelent)
                {
                    GameState.excelentCountDaily = -1;
                    ClearTask(Const.EXCELLENT_COMBO_DAILY);
                }
                else if (combo == ComboType.good)
                {
                    GameState.goodCountDaily = -1;
                    ClearTask(Const.GOOD_COMBO_DAILY);
                }
                else if (combo == ComboType.great)
                {
                    GameState.greatCountDaily = -1;
                    ClearTask(Const.GREAT_COMBO_DAILY);
                }
                break;
        }
    }

    private void ClearTaskAchievementByKey()
    {
        switch (goal.goalType)
        {
            case GoalType.Spelling:
                GameState.countSpell = -1;
                ClearTask(Const.SPELLING);
                break;
            case GoalType.LevelClear:
                GameState.countLevel = -1;
                ClearTask(Const.LEVEL_CLEAR);
                break;
            case GoalType.ChappterClear:
                GameState.countChapter = -1;
                ClearTask(Const.CHAPTER_CLEAR);
                break;
            case GoalType.ExtraWord:
                GameState.countExtra = -1;
                ClearTask(Const.EXTRA_WORD);
                break;
            case GoalType.Booster:
                GameState.countBooster = -1;
                ClearTask(Const.BOOSTER);
                break;
            case GoalType.LevelMisspelling:
                GameState.countLevelMisspelling = -1;
                ClearTask(Const.LEVEL_MISSPELLING);
                break;
            case GoalType.Combos:
                if (combo == ComboType.amazing)
                {
                    GameState.amazingCount = -1;
                    ClearTask(Const.AMAZING_COMBO);
                }
                else if (combo == ComboType.awesome)
                {
                    GameState.awesomeCount = -1;
                    ClearTask(Const.AWESOME_COMBO);
                }
                else if (combo == ComboType.excelent)
                {
                    GameState.excelentCount = -1;
                    ClearTask(Const.EXCELLENT_COMBO);
                }
                else if (combo == ComboType.good)
                {
                    GameState.goodCount = -1;
                    ClearTask(Const.GOOD_COMBO);
                }
                else if (combo == ComboType.great)
                {
                    GameState.greatCount = -1;
                    ClearTask(Const.GREAT_COMBO);
                }
                break;
        }
    }

    /// <summary>
    /// Clear Task by key name
    /// </summary>
    /// <param name="key">Key name</param>
    private void ClearTask(string key)
    {
        CPlayerPrefs.DeleteKey(key);
    }

    /// <summary>
    /// Clear All Task
    /// </summary>
    private void ClearTask()
    {
        CPlayerPrefs.DeleteKey(Const.SPELLING);
        CPlayerPrefs.DeleteKey(Const.LEVEL_CLEAR);
        CPlayerPrefs.DeleteKey(Const.CHAPTER_CLEAR);
        CPlayerPrefs.DeleteKey(Const.GOOD_COMBO);
        CPlayerPrefs.DeleteKey(Const.GREAT_COMBO);
        CPlayerPrefs.DeleteKey(Const.AMAZING_COMBO);
        CPlayerPrefs.DeleteKey(Const.AWESOME_COMBO);
        CPlayerPrefs.DeleteKey(Const.EXCELLENT_COMBO);
    }
}


