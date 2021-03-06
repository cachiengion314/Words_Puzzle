﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum DialogType
{
    PromoteQuit,
    PromotePopup,
    QuitGame,
    Pause,
    Settings,
    YesNo,
    Ok,
    Shop,
    InviteFriends,
    HowtoPlay,
    ExtraWord,
    Win,
    RewardedVideo,
    RateGame,
    Shop2,
    Objective,
    Dictionary,
    Bee,
    Facebook,
    Mail,
    DailyGifts,
    WeekenSale,
    MeanDialog,
    MeanInGameDialog,
    LimitedSaleDialog,
    FreeStars,
    ChickenBank,
    OpenWordDictionary,
    ComingSoon,
    FreeStarsPlay,
    ShareDialog,
    FeedbackDialog,
    MissingWordsFeedbackDialog,
    LevelWordsFeedbackDialog,
    ContactUsDialog,
    Themes,
    CollectFreestarPlay,
    DontLikeAds,
    FlagMean,
    UnlockTheFlag
};

public enum DialogShow
{
    DONT_SHOW_IF_OTHERS_SHOWING,
    REPLACE_CURRENT,
    STACK,
    SHOW_PREVIOUS,
    OVER_CURRENT,
    STACK_DONT_HIDEN,
};

public class DialogController : MonoBehaviour
{
    public static DialogController instance;

    [HideInInspector]
    public Dialog current;
    //[HideInInspector]
    public Dialog[] baseDialogs;

    public Action onDialogsOpened;
    public Action onDialogsClosed;
    public Action onDialogsCompleteClosed;
    public Stack<Dialog> dialogs = new Stack<Dialog>();

    public void Awake()
    {
        instance = this;
    }

    public void ShowDialog(int type)
    {
        ShowDialog((DialogType)type, DialogShow.DONT_SHOW_IF_OTHERS_SHOWING);
    }

    public void ShowDialog(DialogType type, DialogShow option = DialogShow.REPLACE_CURRENT, string contentTitle = null, string contentMessage = null, bool hidenThisOverlay = false)
    {
        Dialog dialog = GetDialog(type);
        ShowDialog(dialog, option);
        if (contentTitle != null || contentTitle != "")
            dialog.SetTitleContent(contentTitle);
        if (contentMessage != null || contentMessage != "")
            dialog.SetMessageContent(contentMessage);
        if (hidenThisOverlay)
            dialog.HidenOverlay();
    }

    public void ShowYesNoDialog(string title, string content, Action onYesListener, Action onNoListenter, DialogShow option = DialogShow.REPLACE_CURRENT)
    {
        var dialog = (YesNoDialog)GetDialog(DialogType.YesNo);
        if (dialog.title != null) dialog.title.SetText(title);
        if (dialog.message != null) dialog.message.SetText(content);
        dialog.onYesClick = onYesListener;
        dialog.onNoClick = onNoListenter;
        ShowDialog(dialog, option);
    }

    public void ShowOkDialog(string title, string content, Action onOkListener, DialogShow option = DialogShow.REPLACE_CURRENT)
    {
        var dialog = (OkDialog)GetDialog(DialogType.Ok);
        if (dialog.title != null) dialog.title.SetText(title);
        if (dialog.message != null) dialog.message.SetText(content);
        dialog.onOkClick = onOkListener;
        ShowDialog(dialog, option);
    }

    public void ShowDialog(Dialog dialog, DialogShow option = DialogShow.REPLACE_CURRENT)
    {
        if (current != null)
        {
            if (option == DialogShow.DONT_SHOW_IF_OTHERS_SHOWING)
            {
                Destroy(dialog.gameObject);
                return;
            }
            else if (option == DialogShow.REPLACE_CURRENT)
            {
                current.Close();
            }
            else if (option == DialogShow.STACK)
            {
                current.Hide();
            }
            else if (option == DialogShow.STACK_DONT_HIDEN)
            {
                current.DontHiden();
            }
        }

        current = dialog;
        if (option != DialogShow.SHOW_PREVIOUS)
        {
            current.onDialogOpened += OnOneDialogOpened;
            current.onDialogClosed += OnOneDialogClosed;
            current.onDialogCompleteClosed += OnOneDialogCompleteClosed;
            dialogs.Push(current);
        }

        if (current.resestAnim)
            current.Show();
        else
            current.ShowNoAnim();
        onDialogsOpened?.Invoke();
    }

    public Dialog GetDialog(DialogType type)
    {
        Dialog dialog = baseDialogs[(int)type];
        dialog.dialogType = type;
        return (Dialog)Instantiate(dialog, transform.position, transform.rotation);
    }

    public void CloseCurrentDialog()
    {
        if (current != null)
            current.Close();
    }

    public void CloseDialog(DialogType type)
    {
        if (current == null) return;
        if (current.dialogType == type)
        {
            current.Close();
        }
    }

    public bool IsDialogShowing()
    {
        return current != null;
    }

    public bool IsDialogShowing(DialogType type)
    {
        if (current == null) return false;
        return current.dialogType == type;
    }

    private void OnOneDialogOpened(Dialog dialog)
    {

    }

    private void OnOneDialogClosed(Dialog dialog)
    {
        if (current == dialog)
        {
            current = null;
            dialogs.Pop();
            if (onDialogsClosed != null && dialogs.Count == 0)
                onDialogsClosed();

            if (dialogs.Count > 0)
            {
                ShowDialog(dialogs.Peek(), DialogShow.SHOW_PREVIOUS);
            }
        }
    }

    private void OnOneDialogCompleteClosed()
    {
        onDialogsCompleteClosed?.Invoke();
    }

}
