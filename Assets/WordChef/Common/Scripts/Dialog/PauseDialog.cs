using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using Superpow;

public class PauseDialog : Dialog
{

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
        DialogController.instance.ShowDialog(DialogType.ComingSoon, DialogShow.STACK_DONT_HIDEN, "Themes", "This feature is coming soon. Please try again later!");
    }

    public void OnTaskClick()
    {
        var numlevels = Utils.GetNumLevels(GameState.currentWorld, GameState.currentSubWorld);
        var currlevel = (GameState.currentLevel + numlevels * (GameState.currentSubWorld + MainController.instance.gameData.words.Count * GameState.currentWorld)) + 1;
        Sound.instance.Play(Sound.Others.PopupOpen);
        if ((currlevel < 11 && !CPlayerPrefs.HasKey("OBJ_TUTORIAL")) || (Prefs.countLevelDaily < 2 && !CPlayerPrefs.HasKey("OBJ_TUTORIAL")))
            DialogController.instance.ShowDialog(DialogType.ComingSoon, DialogShow.STACK_DONT_HIDEN, "Objectives", "This feature is not unlocked. Keep it up!");
        else
            DialogController.instance.ShowDialog(DialogType.Objective, DialogShow.STACK_DONT_HIDEN);
    }

    public void OnHomeClick()
    {
        if (WordRegion.instance != null)
            WordRegion.instance.btnRewardAds.GetComponent<RewardController>().overLay.SetActive(true);
        DialogOverlay.instance.Overlay.enabled = false;
        GetComponent<Image>().enabled = false;
        CUtils.LoadScene(Const.SCENE_HOME, false);
        Close();
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
   
}
