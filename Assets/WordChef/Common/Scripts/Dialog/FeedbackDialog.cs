using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class FeedbackDialog : PauseDialog
{
    protected override void Start()
    {
        base.Start();
    }
    public void OnLevelWordsFeedbackClick()
    {
        Sound.instance.PlayButton();
        DialogController.instance.ShowDialog(DialogType.LevelWordsFeedbackDialog, DialogShow.STACK_DONT_HIDEN);
    }
    public void OnMissingWordsFeedbackClick()
    {
        Sound.instance.PlayButton();
        DialogController.instance.ShowDialog(DialogType.MissingWordsFeedbackDialog, DialogShow.STACK_DONT_HIDEN);
    }
    public void OnCloseClick()
    {
        Close();
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
