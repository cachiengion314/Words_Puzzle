using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [HideInInspector] public int MinLevelToLoadBanner;
    [HideInInspector] public int MinLevelToLoadRewardVideo;
    [HideInInspector] public int PercentToloadInterstitial;
    [HideInInspector] public int MinLevelToLoadInterstitial;

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
            Debug.Log("Show Ads FB");
            SceneAnimate.Instance.ShowOverLayPauseGame(true);
        }
        else
        {
            if (UnityAdTest.instance.IsInitialized() && UnityAdTest.instance.IsLoaded())
            {
                _adsController = UnityAdTest.instance;
                _adsController.ShowVideoAds();
                Debug.Log("Show Ads UNITY ADS");
                TweenControl.GetInstance().DelayCall(transform, 1f,()=> {
                    if (UnityAdTest.instance.IsShowing())
                        SceneAnimate.Instance.ShowOverLayPauseGame(true);
                    else
                    {
                        if (showToast)
                            Toast.instance.ShowMessage("Rewarded video is not ready");
                        adsNotReadyYetCallback?.Invoke();
                    }
                });
            }
            else
            {
                if (AdmobController.instance.rewardBasedVideo.IsLoaded())
                {
                    _adsController = AdmobController.instance;
                    _adsController.ShowVideoAds();
                    Debug.Log("Show Ads Admob");
                    SceneAnimate.Instance.ShowOverLayPauseGame(true);
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

    private void ShowInterstitial(bool showToast = true, Action adsNotReadyYetCallback = null, Action noInternetCallback = null, Action adsComplete = null)
    {
        if (CUtils.IsAdsRemoved())
        {
            adsComplete?.Invoke();
            return;
        }

        if (AudienceNetworkFbAd.instance.isIntersLoaded)
        {
            _adsController = AudienceNetworkFbAd.instance;
            _adsController.ShowInterstitialAds();
            SceneAnimate.Instance.ShowOverLayPauseGame(true);
            Debug.Log("Show Interstitial Ads FB");
        }
        else
        {
            if (UnityAdTest.instance.IsInitialized() && UnityAdTest.instance.IsLoadedInterstitial())
            {
                _adsController = UnityAdTest.instance;
                _adsController.ShowInterstitialAds();
                if (UnityAdTest.instance.IsShowing())
                    SceneAnimate.Instance.ShowOverLayPauseGame(true);
                else
                    adsNotReadyYetCallback?.Invoke();
                Debug.Log("Show Interstitial Ads UNITY ADS");
            }
            else
            {
                if (AdmobController.instance.interstitial != null && AdmobController.instance.interstitial.IsLoaded())
                {
                    _adsController = AdmobController.instance;
                    _adsController.ShowInterstitialAds();
                    SceneAnimate.Instance.ShowOverLayPauseGame(true);
                    Debug.Log("Show Interstitial Ads Admob");
                }
                else
                {
                    adsNotReadyYetCallback?.Invoke();
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

    public bool AdsIsLoaded(bool showToast = false, Text textNoti = null, TextMeshProUGUI textMeshNoti = null, Action checkComplete = null)
    {
        if (AudienceNetworkFbAd.instance.isLoaded || AdmobController.instance.rewardBasedVideo.IsLoaded() || UnityAdTest.instance.IsLoaded())
            return true;
        else
        {
            CUtils.CheckConnection(this, (result) =>
            {
                if (result == 0)
                {
                    if (showToast)
                        Toast.instance.ShowMessage("Rewarded video is not ready");
                    if (textNoti != null)
                        textNoti.text = "Rewarded video is not ready";
                    if (textMeshNoti != null)
                        textMeshNoti.text = "Rewarded video is not ready";
                    _isLoading = false;
                    LoadDataAds();
                    checkComplete?.Invoke();
                }
                else
                {
                    if (showToast)
                        Toast.instance.ShowMessage("No Internet Connection");
                    if (textNoti != null)
                        textNoti.text = "No Internet Connection";
                    if (textMeshNoti != null)
                        textMeshNoti.text = "No Internet Connection";
                    checkComplete?.Invoke();
                }
            });
            return false;
        }
    }

    #region Show Ads Handle
    public void ShowVideoAds(bool showToast = true, Action adsNotReadyYetCallback = null, Action noInternetCallback = null)
    {
        StartCoroutine(ShowVideo(showToast, adsNotReadyYetCallback, noInternetCallback));
    }

    public void ShowBannerAds()
    {
        _adsController.ShowBannerAds();
    }

    public void ShowInterstitialAds(Action onCompleteAds = null)
    {
        CUtils.CheckConnection(this, (result) =>
        {
            if (result == 0)
            {
                float percent = (float)PercentToloadInterstitial / 100f;
                float randomNumber = UnityEngine.Random.Range(0f, 1f);
                Debug.Log("RandomNumber: " + randomNumber);
                Debug.Log("percent: " + percent);
                if (randomNumber <= percent)
                {
                    ShowInterstitial(true, () =>
                    {
                        onCompleteAds?.Invoke();
                    }, null, () =>
                    {
                        onCompleteAds?.Invoke();
                    });
                }
            }
            else
            {
                onCompleteAds?.Invoke();
            }
        });
    }
    #endregion

}
