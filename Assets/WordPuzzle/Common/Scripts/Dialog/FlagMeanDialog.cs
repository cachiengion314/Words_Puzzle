using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagMeanDialog : Dialog
{
    private new void Start()
    {
        base.Start();

    }
    public void OnClickCloseFlagMeanDialog()
    {
        TweenControl.GetInstance().ScaleFromOne(DictionaryDialog.instance.flagMeanDialog.gameObject, 0.3f);
    }
}
