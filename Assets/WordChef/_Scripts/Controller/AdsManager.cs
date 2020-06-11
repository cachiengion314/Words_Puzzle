using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    public Action onAdsClose;
    public Action onAdsOpened;
    public Action onAdsFailedToLoad;
    public Action onAdsLeftApplication;
    public Action onAdsLoaded;
    public Action onAdsRewarded;
    public Action onAdsStarted;

    public IAds _adsController;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }


    #region Show Ads Handle
    public void LoadVideoAds()
    {
        _adsController.LoadVideoAds();
    }
    public void ShowVideoAds(Action adsNotReadyYetCallback = null, Action noInternetCallback = null)
    {
        _adsController.ShowVideoAds(adsNotReadyYetCallback, noInternetCallback);
    }

    public void ShowBannerAds()
    {
        _adsController.ShowBannerAds();
    }

    public void ShowInterstitialAds()
    {
        _adsController.ShowInterstitialAds();
    }
    #endregion

}
