using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using UnityEngine.UI;

public class FeedbackDialog : Dialog
{
    [Header("THEME UI CHANGE")]
    [SerializeField] private Image _btnMissingWord;
    [SerializeField] private Image _btnLevelWord;
    [SerializeField] private Image _btnContact;
    [SerializeField] private Image _iconMissingWord;
    [SerializeField] private Image _iconLevelWord;
    [SerializeField] private Image _iconContact;
    [SerializeField] private Text _txtMissingWord;
    [SerializeField] private Text _txtLevelWord;
    [SerializeField] private Text _txtContact;
    [SerializeField] private List<Image> _bgOptions;


    private string fromEmail = "goofyart314@gmail.com"; // your Gmail Account From Where You Want To Send Email
    private string toEmail = "cachiengion314@gmail.com";
    private string subject = "SubjectName";
    private string body = "Body of the email";

    private string password = "null"; // YourGmailAccountPassword


    protected override void Start()
    {
        base.Start();
        CheckTheme();
    }

    private void CheckTheme()
    {
        if (MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            _btnMissingWord.sprite = currTheme.uiData.feedbackData.btnMissingWord;
            _btnLevelWord.sprite = currTheme.uiData.feedbackData.btnLevelWord;
            _btnContact.sprite = currTheme.uiData.feedbackData.btnContact;
            _iconMissingWord.sprite = currTheme.uiData.feedbackData.iconMissingWord;
            _iconLevelWord.sprite = currTheme.uiData.feedbackData.iconLevelWord;
            _iconContact.sprite = currTheme.uiData.feedbackData.iconContact;

            //if (_bgOption != null) _bgOption.SetNativeSize();
            _btnMissingWord.SetNativeSize();
            _btnLevelWord.SetNativeSize();
            _btnContact.SetNativeSize();
            _iconMissingWord.SetNativeSize();
            _iconLevelWord.SetNativeSize();
            _iconContact.SetNativeSize();

            _txtMissingWord.color = _txtLevelWord.color = _txtContact.color = currTheme.fontData.colorContentDialog;

            foreach (var option in _bgOptions)
            {
                if (option != null)
                    option.sprite = currTheme.uiData.feedbackData.bgOption;
            }
        }
    }

    public void OnContactUsClick()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        DialogController.instance.ShowDialog(DialogType.ContactUsDialog, DialogShow.STACK_DONT_HIDEN);
    }
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
