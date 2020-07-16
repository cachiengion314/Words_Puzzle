using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using Superpow;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseDialog : Dialog
{
    [SerializeField] private GameObject _iconTask;
    [Header("THEME UI CHANGE")]
    [SerializeField] private Image _iconBtnHome;
    [SerializeField] private Image _iconBtnTheme;
    [SerializeField] private Image _iconBtnSetting;
    [SerializeField] private Image _iconBtnTask;
    [SerializeField] private Image _iconBtnFeddback;
    [SerializeField] private Image _iconBtnHelp;
    [SerializeField] private Image _iconHome;
    [SerializeField] private Image _iconTheme;
    [SerializeField] private Image _iconSetting;
    [SerializeField] private Image _iconObjective;
    [SerializeField] private Image _iconFeedback;
    [SerializeField] private Image _iconHelp;
    [SerializeField] private TextMeshProUGUI _txtHome;
    [SerializeField] private TextMeshProUGUI _txtTheme;
    [SerializeField] private TextMeshProUGUI _txtSetting;
    [SerializeField] private TextMeshProUGUI _txtObjective;
    [SerializeField] private TextMeshProUGUI _txtFeedback;
    [SerializeField] private TextMeshProUGUI _txtHelp;

    public static PauseDialog instance;

    protected override void Awake()
    {
        base.Awake();
        if (instance == null)
            instance = this;
        CheckTheme();
    }

    protected override void Start()
    {
        base.Start();
        CheckTheme();
        CheckShowIconTaskComplete();
    }

    private void CheckTheme()
    {
        if(MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            _iconBtnHome.sprite = currTheme.uiData.menuData.iconBtnNormal;
            _iconBtnTheme.sprite = currTheme.uiData.menuData.iconBtnNormal;
            _iconBtnSetting.sprite = currTheme.uiData.menuData.iconBtnNormal;
            _iconBtnTask.sprite = currTheme.uiData.menuData.iconBtnTask;
            _iconBtnFeddback.sprite = currTheme.uiData.menuData.iconBtnHelpFeddback;
            _iconBtnHelp.sprite = currTheme.uiData.menuData.iconBtnHelpFeddback;
            _iconHome.sprite = currTheme.uiData.menuData.iconHome;
            _iconTheme.sprite = currTheme.uiData.menuData.iconTheme;
            _iconSetting.sprite = currTheme.uiData.menuData.iconSetting;
            _iconObjective.sprite = currTheme.uiData.menuData.iconObjective;
            _iconFeedback.sprite = currTheme.uiData.menuData.iconFeedback;
            _iconHelp.sprite = currTheme.uiData.menuData.iconHelp;

            _iconHome.SetNativeSize();
            _iconTheme.SetNativeSize();
            _iconSetting.SetNativeSize();
            _iconObjective.SetNativeSize();
            _iconFeedback.SetNativeSize();
            _iconHelp.SetNativeSize();

            _txtHome.color = _txtTheme.color = _txtSetting.color = _txtObjective.color = _txtFeedback.color = _txtHelp.color = currTheme.fontData.colorContentDialog;
        }
    }

    public void CheckShowIconTaskComplete()
    {
        if (_iconTask != null) 
            _iconTask.SetActive(ObjectiveManager.instance.Icon.activeSelf);
    }

    public void OnFeedbackClick()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        DialogController.instance.ShowDialog(DialogType.FeedbackDialog, DialogShow.STACK_DONT_HIDEN);
    }
    public void OnHelpClick()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        DialogController.instance.ShowDialog(DialogType.ShareDialog, DialogShow.REPLACE_CURRENT);
    }
    public void OnContinueClick()
    {
        Close();
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void OnMenuClick()
    {
        CUtils.LoadScene(Const.SCENE_CHAPTER, true);
        Close();
    }

    public void OnSettingsClick()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        DialogController.instance.ShowDialog(DialogType.Settings, DialogShow.STACK_DONT_HIDEN);
    }

    public void OnThemesClick()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        //DialogController.instance.ShowDialog(DialogType.ComingSoon, DialogShow.STACK_DONT_HIDEN, "Themes", "This feature is coming soon. Please try again later!");
        DialogController.instance.ShowDialog(DialogType.Themes, DialogShow.STACK_DONT_HIDEN);

        AudienceNetworkBanner.instance.DisposeAllBannerAd();
    }

    public void OnTaskClick()
    {
        var numlevels = Utils.GetNumLevels(GameState.currentWorld, GameState.currentSubWorld);
        var currlevel = (GameState.currentLevel + numlevels * GameState.currentSubWorld + MainController.instance.gameData.words[0].subWords.Count * numlevels * GameState.currentWorld) + 1;
        Sound.instance.Play(Sound.Others.PopupOpen);
        if ((currlevel < 11 && !CPlayerPrefs.HasKey("OBJ_TUTORIAL")) || (Prefs.countLevelDaily < 2 && !CPlayerPrefs.HasKey("OBJ_TUTORIAL")))
            DialogController.instance.ShowDialog(DialogType.ComingSoon, DialogShow.STACK_DONT_HIDEN, "Objectives", "This feature is not unlocked.\nKeep it up!");
        else
            DialogController.instance.ShowDialog(DialogType.Objective, DialogShow.STACK_DONT_HIDEN);
    }

    public void OnHomeClick()
    {
        CUtils.LoadScene(Const.SCENE_HOME, false);

        AudienceNetworkBanner.instance.DisposeAllBannerAd();
        //Close();
    }

    public void OnShareClick()
    {
        Close();
    }
    //public void OnSettingClick()
    //{
    //    Sound.instance.Play(Sound.Others.PopupOpen);
    //    DialogController.instance.ShowDialog(DialogType.Settings, DialogShow.STACK_DONT_HIDEN);
    //}

    public virtual void OnHowToPlayClick()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        DialogController.instance.ShowDialog(DialogType.HowtoPlay);
    }

    void OnDestroy()
    {
        Close();
    }
}
