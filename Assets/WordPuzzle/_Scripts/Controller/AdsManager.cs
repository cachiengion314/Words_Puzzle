using System;
using System.Collections;
using TMPro;
using UnityEngine;
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
    public int MinLevelToLoadBanner;
    public int MinLevelToLoadRewardVideo;
    public int PercentToloadInterstitial;
    public int MinLevelToLoadInterstitial;

    private bool _isLoading;
    public bool IsLoading
    {
        get
        {
            return _isLoading;
        }
        set
        {
            _isLoading = value;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        LoadDataAds();
    }
    public void LoadDataAds()
    {
        if (_isLoading)
            return;

        AudienceNetworkFbAd.instance.rewardIdFaceAds = ConfigController.instance.config.facebookAdsId.rewardedFreeStars;
        AudienceNetworkFbAd.instance.intersititialIdFaceAds = ConfigController.instance.config.facebookAdsId.rewardedFreeStars;
#if UNITY_ANDROID && !UNITY_EDITOR
        if (AudienceNetworkFbAd.instance != null)
        {
            AudienceNetworkFbAd.instance.LoadVideoAds();
            AudienceNetworkFbAd.instance.LoadInterstitial();
        }
#endif

        AdmobController.instance.videoAdsId = ConfigController.instance.config.admob.admob_free_stars;
        AdmobController.instance.interstitialAdsId = ConfigController.instance.config.admob.admob_level_transition;
        AdmobController.instance.bannerAdsId = ConfigController.instance.config.admob.admob_banner;
        if (AdmobController.instance != null)
        {
            AdmobController.instance.InitRewardedVideo();
            AdmobController.instance.RequestRewardBasedVideo();
            AdmobController.instance.RequestInterstitial();
        }

        //if (UnityAdTest.instance != null)
        //{
        //    UnityAdTest.instance.ReloadVideoAds();
        //    UnityAdTest.instance.isAdPlaySuccessAndPlayerCanClickClose = false;
        //}
        //_isLoading = true;
    }
    private IEnumerator ShowVideo(bool showToast = true, Action adsNotReadyYetCallback = null, Action noInternetCallback = null)
    {
        yield return new WaitForSeconds(0.1f);

        if (AudienceNetworkFbAd.instance.isLoaded)
        {
            _adsController = AudienceNetworkFbAd.instance;
            _adsController.ShowVideoAds(adsNotReadyYetCallback, noInternetCallback);
            Debug.Log("Show Ads FB");
            SceneAnimate.Instance.ShowOverLayPauseGame(true);
        }
        else
        {
            //if (UnityAdTest.instance.IsInitialized() && UnityAdTest.instance.IsLoaded())
            //{
            //    _adsController = UnityAdTest.instance;
            //    _adsController.ShowVideoAds(adsNotReadyYetCallback, noInternetCallback);
            //    Debug.Log("Show Ads UNITY ADS");
            //    if (!UnityAdTest.instance.isAdPlaySuccessAndPlayerCanClickClose)
            //    {
            //        TweenControl.GetInstance().DelayCall(transform, 1f, () =>
            //        {
            //            if (UnityAdTest.instance.IsShowing())
            //                SceneAnimate.Instance.ShowOverLayPauseGame(true);
            //            else
            //            {
            //                if (showToast)
            //                    Toast.instance.ShowMessage("Rewarded video is not ready");
            //                adsNotReadyYetCallback?.Invoke();
            //            }
            //        });
            //    }
            //}
            //else
            //{
            if (AdmobController.instance.rewardBasedVideo.IsLoaded())
            {
                _adsController = AdmobController.instance;
                _adsController.ShowVideoAds(adsNotReadyYetCallback, noInternetCallback);
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
            //}
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
#if UNITY_EDITOR
            adsComplete?.Invoke();
            SceneAnimate.Instance.ShowOverLayPauseGame(false);
#endif
        }
        else
        {
            //if (UnityAdTest.instance.IsInitialized() && UnityAdTest.instance.IsLoadedInterstitial())
            //{
            //    _adsController = UnityAdTest.instance;
            //    _adsController.ShowInterstitialAds();
            //    if (UnityAdTest.instance.IsShowing())
            //        SceneAnimate.Instance.ShowOverLayPauseGame(true);
            //    else
            //        adsNotReadyYetCallback?.Invoke();
            //    Debug.Log("Show Interstitial Ads UNITY ADS");
            //}
            //else
            //{
            if (AdmobController.instance.interstitial != null && AdmobController.instance.interstitial.IsLoaded())
            {
                _adsController = AdmobController.instance;
                _adsController.ShowInterstitialAds();
                SceneAnimate.Instance.ShowOverLayPauseGame(true);
                Debug.Log("Show Interstitial Ads Admob");
#if UNITY_EDITOR
                adsComplete?.Invoke();
                SceneAnimate.Instance.ShowOverLayPauseGame(false);
#endif
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
            //}
        }
    }

    public bool AdsIsLoaded(bool showToast = false, Text textNoti = null, TextMeshProUGUI textMeshNoti = null, Action checkComplete = null)
    {
        if (AudienceNetworkFbAd.instance.isLoaded || AdmobController.instance.rewardBasedVideo.IsLoaded()/* || UnityAdTest.instance.IsLoaded()*/)
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
