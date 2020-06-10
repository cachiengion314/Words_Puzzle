using AudienceNetwork;
using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardVideoController : MonoBehaviour
{
    public Action onRewardedCallback;
    public Action<bool> onUpdateBtnAdsCallback;
    public GameObject content;
    public GameObject adAvailableTextHolder;
    public TimerText timerText;

    private const string ACTION_NAME = "rewarded_video";

    void Awake()
    {
        InitEventAdmob();
    }

    private void InitEventAdmob()
    {
        if (timerText != null) timerText.onCountDownComplete += OnCountDownComplete;

#if UNITY_ANDROID || UNITY_IOS
        Timer.Schedule(this, 0.1f, AddEvents);

        if (!IsAvailableToShow())
        {
            content.SetActive(false);
            if (IsAdAvailable() && !IsActionAvailable())
            {
                int remainTime = (int)(ConfigController.instance.config.rewardedVideoPeriod - CUtils.GetActionDeltaTime(ACTION_NAME));
                ShowTimerText(remainTime);
            }
        }

        InvokeRepeating("IUpdate", 1, 1);
#else
        //gameObject.SetActive(false);
#endif
    }

    private void AddEvents()
    {        
        if (AdmobController.instance.rewardBasedVideo != null)
        {
            AdmobController.instance.rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        }
        else
        {
            Debug.Log("Video reward null");
        }        
    }
    private void IUpdate()
    {
        content.SetActive(IsAvailableToShow());
        onUpdateBtnAdsCallback?.Invoke(IsAvailableToShow());
    }

    private void ShowTimerText(int time)
    {
        if (adAvailableTextHolder != null)
        {
            adAvailableTextHolder.SetActive(true);
            timerText.SetTime(0);
            timerText.Run();
        }
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        CancelInvoke("IUpdate");
        content.SetActive(false);
        //gameObject.SetActive(false);
        ShowTimerText(ConfigController.instance.config.rewardedVideoPeriod);
        onRewardedCallback?.Invoke();
    }

    private void OnCountDownComplete()
    {
        adAvailableTextHolder.SetActive(false);
        if (IsAdAvailable())
        {
            content.SetActive(true);
        }
    }

    public bool IsAvailableToShow()
    {
        return IsActionAvailable() && IsAdAvailable();
    }

    private bool IsActionAvailable()
    {
        return CUtils.IsActionAvailable(ACTION_NAME, ConfigController.instance.config.rewardedVideoPeriod);
    }

    private bool IsAdAvailable()
    {
        if (AdmobController.instance.rewardBasedVideo == null) return false;
        bool isLoaded = AdmobController.instance.rewardBasedVideo.IsLoaded();
        return isLoaded;
    }

    private void OnDestroy()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (AdmobController.instance.rewardBasedVideo != null)
        {
            AdmobController.instance.rewardBasedVideo.OnAdRewarded -= HandleRewardBasedVideoRewarded;
        }
#endif
    }


    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            if (adAvailableTextHolder.activeSelf)
            {
                int remainTime = (int)(ConfigController.instance.config.rewardedVideoPeriod - CUtils.GetActionDeltaTime(ACTION_NAME));
                ShowTimerText(remainTime);
            }
        }
    }

}
