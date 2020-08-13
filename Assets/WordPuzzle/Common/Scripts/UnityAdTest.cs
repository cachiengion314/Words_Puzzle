using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class UnityAdTest : MonoBehaviour, IUnityAdsListener, IAds
{
    public static UnityAdTest instance;
    public Action UpdateProgress;
    private readonly string androidGameId = "3685957"; // this string is a constant value and cannot be changed
    private readonly bool testMode = false;

    public string myPlacementId = "rewardedVideo";
    public string myInterstitialId = "myInterstitialId";
    public string bannerPlacementId = "bannerPlacement";
    public bool isAdPlaySuccessAndPlayerCanClickClose;

    private void Awake()
    {
        instance = this;
    }
    //private void Start()
    //{
    //    Advertisement.Initialize(androidGameId, testMode);
    //    Advertisement.AddListener(this);
    //}
    public bool IsLoaded()
    {
        if (myPlacementId == null) return false;
        else
            return Advertisement.IsReady(myPlacementId);
    }
    public bool IsInitialized()
    {
        return Advertisement.isInitialized;
    }
    public bool IsShowing()
    {
        return Advertisement.isShowing;
    }

    public bool IsLoadedInterstitial()
    {
        if (myInterstitialId == null) return false;
        else
            return Advertisement.IsReady(myInterstitialId);
    }
    public void DisplayInterstitialAds()
    {
        Advertisement.Show(myInterstitialId);
    }
    public void ReloadVideoAds()
    {
        Advertisement.Load(myPlacementId);
    }
    public void DisplayVideoAds()
    {
        Advertisement.Show(myPlacementId);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId != ConfigController.instance.config.unityAdsId.interstitialLevel)
        {
            // Define conditional logic for each ad completion status:
            if (showResult == ShowResult.Finished)
            {
                // Reward the user for watching the ad to completion.
                isAdPlaySuccessAndPlayerCanClickClose = true;
                AdsManager.instance.onAdsRewarded?.Invoke();
                //Debug.Log("You get a Reward!!!");
            }
            else if (showResult == ShowResult.Skipped)
            {
                // Do not reward the user for skipping the ad.
                //Debug.Log("You don't get a Reward!!");
                isAdPlaySuccessAndPlayerCanClickClose = true;
                AdsManager.instance.onAdsClose?.Invoke();
            }
            else if (showResult == ShowResult.Failed)
            {
                Debug.Log("UNITY Ads Load Failed!");
                AdsManager.instance.onAdsClose?.Invoke();
            }
        }
        else if (placementId == ConfigController.instance.config.unityAdsId.interstitialLevel)
        {
            if (showResult == ShowResult.Finished)
            {
                // Reward the user for watching the ad to completion.
                isAdPlaySuccessAndPlayerCanClickClose = true;
                //Debug.Log("You get a Reward!!!");
            }
            else if (showResult == ShowResult.Skipped)
            {
                // Do not reward the user for skipping the ad.
                //Debug.Log("You don't get a Reward!!");
                isAdPlaySuccessAndPlayerCanClickClose = true;
                AdsManager.instance.onAdsClose?.Invoke();
            }
            else if (showResult == ShowResult.Failed)
            {
                Debug.Log("UNITY Ads Load Failed!");
                AdsManager.instance.onAdsClose?.Invoke();
            }
        }
        SceneAnimate.Instance.ShowOverLayPauseGame(false);
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == myPlacementId)
        {
            // Advertisement.Show(myPlacementId);
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    /// <summary>
    /// //////////////////// Implement interface
    /// </summary>
    public void ShowVideoAds(Action adsNotReadyYetCallback = null, Action noInternetCallback = null)
    {
        Advertisement.Show(myPlacementId);
        ReloadVideoAds();
    }

    public void ShowBannerAds()
    {

    }

    public void ShowInterstitialAds()
    {
        DisplayInterstitialAds();
    }
}

