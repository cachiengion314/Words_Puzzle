﻿using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.RemoteConfig;
using TMPro;
public class RemoteConfigFirebase : MonoBehaviour
{
    public TextMeshProUGUI textPrefab;
    public void FetchFireBase()
    {
        FetchDataAsync();
    }
    public void ShowData()
    {
        Debug.Log("testParameterNam: " +
           FirebaseRemoteConfig.GetValue("testParameterNam").LongValue);
        textPrefab.text = "testParameterNam: " +
            FirebaseRemoteConfig.GetValue("testParameterNam").StringValue;
        /*
        Debug.Log("config_test_string: " +
             FirebaseRemoteConfig.GetValue("config_test_string").StringValue);
        Debug.Log("config_test_float: " +
              FirebaseRemoteConfig.GetValue("config_test_float").DoubleValue);
        Debug.Log("config_test_bool: " +
                FirebaseRemoteConfig.GetValue("config_test_bool").BooleanValue);
        */
    }

    // Start a fetch request.
    public Task FetchDataAsync()
    {
        Debug.Log("Fetching data...");
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
            Debug.Log("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            Debug.Log("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            Debug.Log("Fetch completed successfully!");
        }

        var info = FirebaseRemoteConfig.Info;
        switch (info.LastFetchStatus)
        {
            case LastFetchStatus.Success:
                FirebaseRemoteConfig.ActivateFetched();
                Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                    info.FetchTime));
                break;
            case LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case FetchFailureReason.Error:
                        Debug.Log("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case LastFetchStatus.Pending:
                Debug.Log("Latest Fetch call still pending.");
                break;
        }
    }
}