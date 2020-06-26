﻿using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.RemoteConfig;
using TMPro;
public class RemoteConfigFirebase : MonoBehaviour
{
    //public TextMeshProUGUI textPrefab;
    public static RemoteConfigFirebase instance;
    public Action notifyIngameCall;
    private string tittle;
    private string contain;
    private bool notifyIngameOn;
    [HideInInspector] public bool isShowNoti;

    private bool isNeedToFetch = true;

    private void Awake()
    {
        instance = this;
        notifyIngameCall += () =>
        {
            if (!notifyIngameOn) return;

            bool isNeedToNotify = NotifyMailDialogData.instance.Tittle != tittle;

            if (!NotifyMailDialogData.instance.IsShowBefore && !CPlayerPrefs.GetBool("FIRST")
           || isNeedToNotify && !CPlayerPrefs.GetBool("FIRST"))
            {
                MailDialog.CreateNewNotify(tittle, contain);
                DialogController.instance.ShowDialog(DialogType.Mail, DialogShow.STACK_DONT_HIDEN);
                isShowNoti = true;
            }
            else
            {
                if (HomeController.instance != null)
                    HomeController.instance.CheckShowFreeBooster();
            }
        };
    }
    private void Update()
    {
        if (!isNeedToFetch) return;
        isNeedToFetch = false;

        FetchFireBase();

        Invoke("ShowData", 1.9f);
    }
    public void FetchFireBase()
    {
        FetchDataAsync();
    }
    private string ConvertFirebaseStringToNormal(string firebasestr)
    {
        string[] tempFirebaseStrArr = firebasestr.Split(new char[1] { '"' });
        firebasestr = null;
        foreach (string item in tempFirebaseStrArr)
        {
            firebasestr += item;
        }

        return firebasestr;
    }
    public void ShowData()
    {
        if (!notifyIngameOn) return;

        tittle = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("TittleMail").StringValue);
        contain = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("ContainMail").StringValue);

        bool isNeedToNotify = NotifyMailDialogData.instance.Tittle != tittle;

        if (!NotifyMailDialogData.instance.IsShowBefore && CPlayerPrefs.GetBool("FIRST")
            || isNeedToNotify && CPlayerPrefs.GetBool("FIRST"))
        {
            MailDialog.CreateNewNotify(tittle, contain);
            DialogController.instance.ShowDialog(DialogType.Mail, DialogShow.STACK_DONT_HIDEN);
        }
    }

    // Start a fetch request.
    public Task FetchDataAsync()
    {
        //Debug.Log("Fetching data...");
        // FetchAsync only fetches new data if the current data is older than the provided
        // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
        // By default the timespan is 12 hours, and for production apps, this is a good
        // number.  For this example though, it's set to a timespan of zero, so that
        // changes in the console will always show up immediately.

        Task fetchTask = FirebaseRemoteConfig.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWith(FetchComplete);
    }

    void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            //Debug.Log("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            //Debug.Log("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            //Debug.Log("Fetch completed successfully!");
            // don't put callback in here. It doesn work properly.
        }

        var info = FirebaseRemoteConfig.Info;
        switch (info.LastFetchStatus)
        {
            case LastFetchStatus.Success:
                FirebaseRemoteConfig.ActivateFetched();
                //Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                //    info.FetchTime));
                break;
            case LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case FetchFailureReason.Error:
                        //Debug.Log("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        //Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case LastFetchStatus.Pending:
                //Debug.Log("Latest Fetch call still pending.");
                break;
        }
    }
}