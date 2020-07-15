﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [HideInInspector] public int MinLevelToLoadRewardVideo;
    [HideInInspector] public int PercentToloadInterstitial;

    private bool _isLoading;

    void Awake()
    {
        if (instance == null)
            instance = this;

        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    void Start()
    {
        LoadDataAds();
    }

    public void LoadDataAds()
    {
        if (_isLoading)
            return;
        if (AudienceNetworkFbAd.instance != null)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AudienceNetworkFbAd.instance.LoadVideoAds();
            AudienceNetworkFbAd.instance.LoadInterstitial();
#endif
        }
        if (AdmobController.instance != null)
        {
            AdmobController.instance.RequestRewardBasedVideo();
            AdmobController.instance.RequestInterstitial();
        }

        if (UnityAdTest.instance != null)
        {
            UnityAdTest.instance.ReloadVideoAds();
        }
        _isLoading = true;
    }
    private void ChangedActiveScene(Scene current, Scene next)
    {
        LoadDataAds();
    }
    private IEnumerator ShowVideo(bool showToast = true, Action adsNotReadyYetCallback = null, Action noInternetCallback = null)
    {
        yield return new WaitForSeconds(0.1f);

        if (AudienceNetworkFbAd.instance.isLoaded)
        {
            _adsController = AudienceNetworkFbAd.instance;
            _adsController.ShowVideoAds();
        }
        else
        {
            if (UnityAdTest.instance.IsLoaded())
            {
                _adsController = UnityAdTest.instance;
                _adsController.ShowVideoAds();
            }
            else
            {
                if (AdmobController.instance.rewardBasedVideo.IsLoaded())
                {
                    _adsController = AdmobController.instance;
                    _adsController.ShowVideoAds();
                }
                else
                {
                    if (WordRegion.instance != null && WordRegion.instance.BtnADS != null)
                        WordRegion.instance.BtnADS._btnAds.interactable = true;
                    CUtils.CheckConnection(this, (result) =>
                    {
                        if (result == 0)
                        {
                            if (showToast)
                                Toast.instance.ShowMessage("Rewarded video is not ready");
                            _isLoading = false;
                            LoadDataAds();
                            adsNotReadyYetCallback?.Invoke();
                        }
                        else
                        {
                            if (showToast)
                                Toast.instance.ShowMessage("No Internet Connection");
                            noInternetCallback?.Invoke();
                        }
                    });
                }
            }
        }
    }

    private void ShowInterstitial(bool showToast = true, Action adsNotReadyYetCallback = null, Action noInternetCallback = null)
    {
        if (CUtils.IsAdsRemoved()) return;

        if (AudienceNetworkFbAd.instance.isIntersLoaded)
        {
            _adsController = AudienceNetworkFbAd.instance;
            _adsController.ShowInterstitialAds();
        }
        else
        {
            if (UnityAdTest.instance.IsLoadedInterstitial())
            {
                _adsController = UnityAdTest.instance;
                _adsController.ShowInterstitialAds();
            }
            else
            {
                if (AdmobController.instance.interstitial != null && AdmobController.instance.interstitial.IsLoaded())
                {
                    _adsController = AdmobController.instance;
                    _adsController.ShowInterstitialAds();
                }
                else
                {
                    //CUtils.CheckConnection(this, (result) =>
                    //{
                    //    if (result == 0)
                    //    {
                    //        if (showToast)
                    //            Toast.instance.ShowMessage("This feature can not be used right now. Please try again later!");
                    //        LoadDataAds();
                    //        adsNotReadyYetCallback?.Invoke();
                    //    }
                    //    else
                    //    {
                    //        if (showToast)
                    //            Toast.instance.ShowMessage("No Internet Connection");
                    //        noInternetCallback?.Invoke();
                    //    }
                    //});
                }
            }
        }
    }

    public bool AdsIsLoaded()
    {
        if (AudienceNetworkFbAd.instance.isLoaded || AdmobController.instance.rewardBasedVideo.IsLoaded() || UnityAdTest.instance.IsLoaded())
            return true;
        else
            return false;
    }

    #region Show Ads Handle
    public void ShowVideoAds(bool showToast = true, Action adsNotReadyYetCallback = null, Action noInternetCallback = null)
    {
        int currentLevel = AudienceNetworkBanner.instance.CheckCurrentLevel();
        if (currentLevel >= MinLevelToLoadRewardVideo)
        {
            StartCoroutine(ShowVideo(showToast, adsNotReadyYetCallback, noInternetCallback));
        }
        else
        {
            Toast.instance.ShowMessage("Rewarded video is not ready");
        }
    }

    public void ShowBannerAds()
    {
        _adsController.ShowBannerAds();
    }

    public void ShowInterstitialAds()
    {
        float percent = PercentToloadInterstitial / 100;
        float randomNumber = UnityEngine.Random.Range(0f, 1f);
        if (randomNumber <= percent)
        {
            ShowInterstitial();
        }
    }
    #endregion

}
