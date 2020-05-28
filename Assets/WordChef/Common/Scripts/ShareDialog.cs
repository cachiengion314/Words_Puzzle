using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShareDialog : Dialog
{
    private string facebookPackageName = "com.facebook.katana";
    private string gmailPackageName = "com.google.android.gm";
    protected override void Start()
    {
        base.Start();
    }
    public void OnFacebookClick()
    {
        Sound.instance.PlayButton();
        NativeShareInvoker.instance.TakeScreenShotAndShareDelay(facebookPackageName);
        Close();
    }
    public void OnGmailClick()
    {
        Sound.instance.PlayButton();
        NativeShareInvoker.instance.TakeScreenShotAndShareDelay(gmailPackageName);
        Close();
    }
    public void OnAllAppClick()
    {
        Sound.instance.PlayButton();
        NativeShareInvoker.instance.TakeScreenShotAndShareDelay();
        Close();
    }
}
