using UnityEngine;
using System.Collections;
using System;

public class OkDialog : Dialog {
    public Action onOkClick;
    public virtual void OnOkClick()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        if (onOkClick != null) onOkClick();
        Sound.instance.Play(Sound.Others.PopupOpen);
        Close();
    }
}
