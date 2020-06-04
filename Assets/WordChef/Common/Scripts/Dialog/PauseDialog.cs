using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

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
        DialogController.instance.ShowDialog(DialogType.ComingSoon, DialogShow.STACK_DONT_HIDEN, "Themes");
    }

    public void OnHomeClick()
    {
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
