using UnityEngine;
using System.Collections;

public class ButtonOpenDialog : MyButton {

    public string contentTitle;
    public string contentMesage;
    public DialogType dialogType;
    public DialogShow dialogShow;

    public override void OnButtonClick()
    {
        ConfigController.instance.isShopHint = false;
        base.OnButtonClick();
        DialogController.instance.ShowDialog(dialogType, dialogShow, contentTitle, contentMesage);
    }
}
