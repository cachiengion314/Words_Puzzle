using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShareDialog : Dialog
{
    private string facebookPackageName = "com.facebook.katana";
    private string gmailPackageName = "com.google.android.gm";
    private string messengerPackageName = "com.facebook.orca";


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
