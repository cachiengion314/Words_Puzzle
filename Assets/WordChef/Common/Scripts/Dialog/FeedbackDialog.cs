using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

public class FeedbackDialog : Dialog
{
    private string fromEmail = "goofyart314@gmail.com"; // your Gmail Account From Where You Want To Send Email
    private string toEmail = "cachiengion314@gmail.com";
    private string subject = "SubjectName";
    private string body = "Body of the email";

    private string password = "songngu31419932016"; // YourGmailAccountPassword


    public void OnLevelWordsFeedbackClick()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        DialogController.instance.ShowDialog(DialogType.LevelWordsFeedbackDialog, DialogShow.STACK_DONT_HIDEN);
    }
    public void OnMissingWordsFeedbackClick()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        DialogController.instance.ShowDialog(DialogType.MissingWordsFeedbackDialog, DialogShow.STACK_DONT_HIDEN);
    }
    public void ContactUsDialog()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        DialogController.instance.ShowDialog(DialogType.ContactUsDialog, DialogShow.STACK_DONT_HIDEN);
    }
    public void OnCloseClick()
    {
        Close();
    }   
    public void SendMail()
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(fromEmail);
        mail.To.Add(toEmail);
        mail.Subject = subject;
        mail.Body = body;
        // you can use others too.
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com", 587);
        //smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential(fromEmail, password) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        { return true; };
        smtpServer.Send(mail);

        Close();
    }
    public void SendURLMail()
    {
        string email = "hello@percas.vn";
        string subject = MyEscapeURL("FeedBack");
        string body = MyEscapeURL("You can tell us anything you want");

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }
    private string MyEscapeURL(string URL)
    {
        return UnityWebRequest.EscapeURL(URL).Replace("+", "%20");
    }
}
