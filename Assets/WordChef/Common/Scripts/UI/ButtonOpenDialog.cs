using UnityEngine;
using System.Collections;

public class ButtonOpenDialog : MyButton {

    public string contentTitle;
    public DialogType dialogType;
    public DialogShow dialogShow;

    public override void OnButtonClick()
    {
        base.OnButtonClick();
        DialogController.instance.ShowDialog(dialogType, dialogShow, contentTitle);
    }
}
