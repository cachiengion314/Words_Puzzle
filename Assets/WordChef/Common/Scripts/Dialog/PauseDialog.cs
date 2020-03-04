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
        Sound.instance.PlayButton();
        Close();
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void OnMenuClick()
    {
        CUtils.LoadScene(1, true);
        Sound.instance.PlayButton();
        Close();
    }

    public void OnSettingsClick()
    {
        Sound.instance.PlayButton();
        DialogController.instance.ShowDialog(DialogType.Settings);
    }

    public void OnHomeClick()
    {
        CUtils.LoadScene(0, false);
        Sound.instance.PlayButton();
        Close();
    }

    public void OnShareClick()
    {
        Sound.instance.PlayButton();
        Close();
    }
    public void OnSettingClick()
    {
        Sound.instance.PlayButton();
        DialogController.instance.ShowDialog(DialogType.Settings, DialogShow.STACK);
    }

    public virtual void OnHowToPlayClick()
    {
        Sound.instance.PlayButton();
        DialogController.instance.ShowDialog(DialogType.HowtoPlay);
    }
}
