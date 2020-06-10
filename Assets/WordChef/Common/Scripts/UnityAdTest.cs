﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class UnityAdTest : MonoBehaviour, IUnityAdsListener, IAds
{
    public static UnityAdTest instance;
    public Action UpdateProgress;
    private string androidGameId = "3645143"; // this string is a constant value and cannot be changed
    private bool testMode = true;

    private string myPlacementId = "rewardedVideo"; // this string is a constant value and cannot be changed

    public string bannerPlacementId = "bannerPlacement"; // this string is a constant value and cannot be changed

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //////////////////////////////////////////////
        Advertisement.Initialize(androidGameId, testMode);
        Advertisement.AddListener(this);

        //StartCoroutine(ShowBannerWhenReady());
    }

    private IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(bannerPlacementId))
        {
            yield return new WaitForSeconds(.5f);
        }
        Advertisement.Banner.Load();
        Advertisement.Banner.Show(bannerPlacementId);
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
    }
    public void DisplayInterstitialAds()
    {
        Advertisement.Show();
    }
    public void DisplayVideoAds()
    {
        Advertisement.Show(myPlacementId);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.

            Debug.Log("You get a Reward!!!");
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            Debug.Log("You don't get a Reward!!");
        }
        else if (showResult == ShowResult.Failed)
        {
            AdsManager.instance._adsController = AdmobController.instance;
            AdsManager.instance.ShowVideoAds();

            Debug.LogWarning("The ad did not finish due to an error.");
        }
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
        AdsManager.instance.onAdsRewarded?.Invoke();
    }

    /// <summary>
    /// //////////////////// Implement interface
    /// </summary>
    public void ShowVideoAds()
    {
        Advertisement.Show(myPlacementId);
    }

    public void ShowBannerAds()
    {

    }

    public void ShowInterstitialAds()
    {

    }

    public void LoadVideoAds()
    {

    }
}

