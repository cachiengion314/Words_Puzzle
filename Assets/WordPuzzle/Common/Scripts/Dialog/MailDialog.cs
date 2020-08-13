using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
//using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;
using Utilities.Common;

public class MailDialog : Dialog
{
    public RectTransform scrollViewContent;
    protected override void Start()
    {
        base.Start();

        //NotifyMailDialogData.instance.mailDialog = this.gameObject.GetComponent<MailDialog>();

        NotifyMailDialogData.instance.scrollViewContent = scrollViewContent.gameObject;
        RectTransform ingameNotifyTranf = Instantiate(NotifyMailDialogData.instance.ingameNotifyPrefb);
        ingameNotifyTranf.SetParent(NotifyMailDialogData.instance.scrollViewContent.transform);
        ingameNotifyTranf.localScale = Vector3.one;
        NotifyMailDialogData.instance.ingameNotify = ingameNotifyTranf.GetComponent<IngameNotify>();

        // Create a messenger 
        NotifyMailDialogData.instance.ingameNotify.CreateNotify(NotifyMailDialogData.instance.Tittle, NotifyMailDialogData.instance.Contain);

    }
    public bool RemoveLatestNotify()
    {
        bool isRemoveSucces = NotifyMailDialogData.instance.RemovePlayerPrefsNotifyAt(NotifyMailDialogData.instance.notifyData.Count - 1);
        return isRemoveSucces;
    }
    public static void CreateNewNotify(string tittle, string contain)
    {
        NotifyMailDialogData.instance.Tittle = tittle;
        NotifyMailDialogData.instance.Contain = contain;

        NotifyMailDialogData.instance.IsShowBefore = false;
    }
    private void OnDestroy()
    {
        NotifyMailDialogData.instance.scrollViewContent = null;
    }
    public void CloseMailDialog()
    {
        if (!NotifyMailDialogData.instance.IsShowBefore)
        {
            NotifyMailDialogData.instance.IsShowBefore = true;
            NotifyMailDialogData.MailBttAction?.Invoke();
        }
        if (HomeController.instance != null)
            HomeController.instance.CheckShowFreeBooster();
        RemoveLatestNotify();
        Close();
    }
}
