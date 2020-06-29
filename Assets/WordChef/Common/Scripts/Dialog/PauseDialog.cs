using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using Superpow;
using System;

public class PauseDialog : Dialog
{
    [SerializeField] private GameObject _iconTask;

    public static PauseDialog instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    protected override void Start()
    {
        base.Start();
        CheckShowIconTaskComplete();
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
