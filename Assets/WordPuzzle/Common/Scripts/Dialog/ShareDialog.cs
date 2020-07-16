using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShareDialog : Dialog
{
    private string facebookPackageName = "com.facebook.katana";
    private string gmailPackageName = "com.google.android.gm";
    private string messengerPackageName = "com.facebook.orca";

    [Header("THEME UI CHANGE")]
    [SerializeField] private Image _btnOther;
    [SerializeField] private Image _btnFacebook;
    [SerializeField] private Image _btnMessager;
    [SerializeField] private Image _btnMail;
    [SerializeField] private Image _iconOther;
    [SerializeField] private Image _iconFacebook;
    [SerializeField] private Image _iconMessager;
    [SerializeField] private Image _iconMail;
    [SerializeField] private TextMeshProUGUI _txtOther;
    [SerializeField] private TextMeshProUGUI _txtFacebook;
    [SerializeField] private TextMeshProUGUI _txtMessager;
    [SerializeField] private TextMeshProUGUI _txtMail;
    [SerializeField] private TextMeshProUGUI _txtTitle;

    protected override void Start()
    {
        base.Start();
        CheckTheme();
    }

    private void CheckTheme()
    {
        if(MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            _btnOther.sprite = currTheme.uiData.helpData.btnNomal;
            _btnFacebook.sprite = currTheme.uiData.helpData.btnFb;
            _btnMessager.sprite = currTheme.uiData.helpData.btnMessager;
            _btnMail.sprite = currTheme.uiData.helpData.btnMail;

            _iconOther.sprite = currTheme.uiData.helpData.iconOther;
            _iconFacebook.sprite = currTheme.uiData.helpData.iconFb;
            _iconMessager.sprite = currTheme.uiData.helpData.iconMessager;
            _iconMail.sprite = currTheme.uiData.helpData.iconMail;

            _btnOther.SetNativeSize();
            _btnFacebook.SetNativeSize();
            _btnMessager.SetNativeSize();
            _btnMail.SetNativeSize();
            _iconOther.SetNativeSize();
            _iconFacebook.SetNativeSize();
            _iconMessager.SetNativeSize();
            _iconMail.SetNativeSize();

            _txtOther.color = _txtFacebook.color = _txtMessager.color = _txtMail.color = _txtTitle .color = currTheme.fontData.colorContentDialog;
        }
    }

    public void OnCloseClick()
    {
        Close();
        MainController.instance.beeController.OnBeeButtonClick();
    }
    public void OnMessengerClick()
    {
        Sound.instance.PlayButton();
        NativeShareInvoker.instance.TakeScreenShotAndShareDelay(messengerPackageName);
        DialogCallEventFirebase("share_messenger");
        Close();
    }
    public void OnFacebookClick()
    {
        Sound.instance.PlayButton();
        NativeShareInvoker.instance.TakeScreenShotAndShareDelay(facebookPackageName);
        DialogCallEventFirebase("share_facebook");
        Close();
    }
    public void OnGmailClick()
    {
        Sound.instance.PlayButton();
        NativeShareInvoker.instance.TakeScreenShotAndShareDelay(gmailPackageName);
        DialogCallEventFirebase("share_mail");
        Close();
    }
    public void OnAllAppClick()
    {
        Sound.instance.PlayButton();
        NativeShareInvoker.instance.TakeScreenShotAndShareDelay();
        DialogCallEventFirebase("share_others");
        Close();
    }

    private void DialogCallEventFirebase(string nameEvent)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("share_dialog", nameEvent, nameEvent);
    }
}
