using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseDialog : Dialog {

    protected override void Start()
    {
        base.Start();
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
