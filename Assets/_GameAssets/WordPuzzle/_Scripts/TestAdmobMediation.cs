using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAdsMediationTestSuite.Api;
using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Api.Mediation.UnityAds;

public class TestAdmobMediation : MonoBehaviour
{
    public string videoAdsId;
    public string interstitialAdsId;

    private InterstitialAd _interstitialAd;

    private void Start()
    {
        MobileAds.Initialize((isComplete) =>
        {
            MediationTestSuite.OnMediationTestSuiteDismissed += HandleMediationTestSuiteDismissed;
            RequestRewardBasedVideo();
            RequestInstertial();
        });
    }

    private void RequestRewardBasedVideo()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = videoAdsId;
#endif
        UnityAds.SetGDPRConsentMetaData(true);
        var request = new AdRequest.Builder()
  .AddTestDevice("2077ef9a63d2b398840261c8221a0c9b")
  .Build();
        MediationTestSuite.AdRequest = request;
        RewardBasedVideoAd.Instance.OnAdLoaded += HandleAdsLoaded;
        RewardBasedVideoAd.Instance.OnAdOpening += HandleAdsOpen;
        RewardBasedVideoAd.Instance.LoadAd(request, adUnitId);

    }

    private void RequestInstertial()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = interstitialAdsId;
#endif

        var request = new AdRequest.Builder()
                      .AddTestDevice("2077ef9a63d2b398840261c8221a0c9b")
                      .Build();
        MediationTestSuite.AdRequest = request;
        _interstitialAd = new InterstitialAd(adUnitId);
        _interstitialAd.OnAdLoaded += HandleAdsLoadedInstertial;
        _interstitialAd.OnAdOpening += HandleAdsOpenInstertial;
        _interstitialAd.LoadAd(request);
    }

    private void HandleAdsOpenInstertial(object sender, EventArgs e)
    {
        Debug.Log("Is Opening Instertial!");
        RequestInstertial();
    }

    private void HandleAdsLoadedInstertial(object sender, EventArgs e)
    {
        Debug.Log("Is Loaded Instertial!");
    }

    private void HandleAdsOpen(object sender, EventArgs e)
    {
        Debug.Log("Is Opening !");
        RequestRewardBasedVideo();
    }

    void HandleAdsLoaded(object sender, EventArgs e)
    {
        Debug.Log("Is Loaded !");
    }

    private void HandleMediationTestSuiteDismissed(object sender, EventArgs e)
    {
        Debug.Log("Done Ads Mediation !");
    }

    public void ShowReward()
    {
        MediationTestSuite.Show();
    }

    public void ShowInstertial()
    {
        MediationTestSuite.Show();
    }

    public void ShowRewardNormal()
    {
        RewardBasedVideoAd.Instance.Show();
    }

    public void ShowInstertialNormal()
    {
        _interstitialAd.Show();
    }
}
