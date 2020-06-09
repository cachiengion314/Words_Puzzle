﻿using System;
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

    [SerializeField] private AdsController _adsController;

    void Awake()
    {
        if (instance == null)
            instance = this;
        if (_adsController == null)
            _adsController = GetComponent<AdsController>();
    }


    #region Show Ads Handle
    public void ShowVideoAds()
    {
        _adsController.ShowVideoAds();
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
