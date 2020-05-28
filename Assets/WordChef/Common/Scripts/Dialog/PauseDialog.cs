using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PauseDialog : Dialog {

    protected override void Start()
    {
        base.Start();
    }
    public void OnFeedbackClick()
    {
        Sound.instance.PlayButton();
        SendMail();
    }
    public void OnHelpClick()
    {
        Sound.instance.PlayButton();
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
    private void SendMail()
    {
        string email = "hello@percas.vn";
        string subject = MyEscapeURL("Your FeedBack");
        string body = MyEscapeURL("Please say somthing");

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }
    private string MyEscapeURL(string URL)
    {
        return UnityWebRequest.EscapeURL(URL).Replace("+", "%20");
    }
}
